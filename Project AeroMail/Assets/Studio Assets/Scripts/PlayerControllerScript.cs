using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine;


public class PlayerControllerScript : MonoBehaviour
{

    //---Private Variables---//
    private Rigidbody rBody;
    private InputMaster controls;
    private Camera_Follow cam;

    //---Speed Variables---//
    [Header("Player Movement Controller")]
    [Tooltip("Controls the default speed of the plane")]
    public float movementSpeed = 150.0f; //What is the base movement speed
    [Tooltip("Displays the planes current speed")]
    public float currentSpeed = 0.0f; //What is out current speed
    [Tooltip("What is the planes maximum Speed")]
    public float maxSpeed = 300.0f; //The max speed we can reach
    [Tooltip("Controls how fast plane yaws")]
    public float yawSpeed = 100.0f;
    [Tooltip("Controls how fast plane pitchs")]
    public float pitchSpeed = 100.0f;
    [Tooltip("Controls how fast plane rolls")]
    public float rollSpeed = 200.0f;

    //---Momentum Variables--//
    [Header("Player Momentum Controller")]
    [Tooltip("COntrols how fast the player accelerates when going up (Higher the number the faster they slow down)")]
    public float accelerationUp = 5.0f; //Acceleration for when we are going up, seemed to be too high if it was the same as down
    [Tooltip("Controls how fast the player accelerates when going down (Higher the numbe the faster they speed up)")]
    public float accelerationDown = 5.0f; //Acceleration for when going down
    private float acceleration = 0.0f; //Variable to hold acceleration
    private const float maxAngle = 90.0f; // What is the maximum angle the plane can go to until it is accelerating the fastest

    //---Tipping Variables---///
    [Header("Player Tipping Controller")]
    [Tooltip("Controls speed at which the plane starts to tip")]
    public float tippingPoint = 15.0f;//At what speed will the plane start tipping
    [Tooltip("Controls how fast the plane tips")]
    public float tippingSpeed = 0.1f;//How fast the plane tips over with it lost speed
    private float step = 0.0f; //This is to make the plane lerp to the downward postiion if their speed decrease too low

    //---Vent Variables---///
    //[Header("Vent Manipulation Controller")]
    //[Tooltip("Controls how hard the vent pushes the player")]
    //public float ventPower = 50.0f;

    //---Controls Input Variables---///
    private float rollCheck;
    private float pitchCheck;
    private float yawCheck;

    //---Flight Variables---///
    private Vector3 pitch = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 yaw = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
    private float rollAngle;


    // Start is called before the first frame update
    void Awake()
    {
        controls = new InputMaster();
        //controls.PlayerController.PlaneMovement.performed += ctx => yawPitch = ctx.ReadValue<Vector2>();
        controls.PlayerController.Roll.performed += ctx => rollCheck = ctx.ReadValue<float>();
        controls.PlayerController.Yaw.performed += ctx => yawCheck = ctx.ReadValue<float>();
        controls.PlayerController.Pitch.performed += ctx => pitchCheck = ctx.ReadValue<float>();
        rBody = GetComponent<Rigidbody>();


        //Start plane off at the regular speed
        currentSpeed = movementSpeed;
        rBody.velocity = transform.forward * currentSpeed;

        //Get the camera
        cam = FindObjectOfType<Camera_Follow>();
    }

    private void OnEnable()
    {
        controls.PlayerController.Enable();
    }

    private void OnDisable()
    {
        controls.PlayerController.Disable();
    }

    private void FixedUpdate()
    {
    
        //When we have a proper model we can change trasnform.right to transform.forward
        moveDirection = transform.forward * movementSpeed;

        yaw = yawCheck * yawSpeed * Time.deltaTime * transform.right;
        pitch = pitchCheck * pitchSpeed * Time.deltaTime * transform.up;

        moveDirection += pitch + yaw;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        
        /*
        //Make the plane roll
        //rollAngle -= rollCheck * rollSpeed * Time.deltaTime;
        //My attempt of doing the animation through code
        if(rollAngle <= 14.0f)
        {
            rollAngle -= yawCheck * rollSpeed * Time.deltaTime;
        }
        transform.rotation *= Quaternion.AngleAxis(rollAngle, Vector3.forward);
        */

        //Calculation the momentum
        //MomentumCalculation();


        /*
        //This doesnt need to be here since there is not momentum or speed lose/gain
        //Does it need to start lerping downward
        if (currentSpeed < tippingPoint)
        {
            reachedTippingPoint();
        }
        else
        {
            step = 0.0f;
        }
        */
        
        //Make this plane move forward
        Movement();

        InputCheck();

    }


    private void Movement()
    {
        //This adds on the acceleration assuming its not that maxSpeed already and if the acceleration is greater than 0
        //if (currentSpeed < maxSpeed || acceleration < 0.0f)
        //{
        //    currentSpeed += acceleration;
        //}
        rBody.velocity = transform.forward * currentSpeed;
    }

    private void MomentumCalculation()
    {
        /*Based on angle from direction from a plane
        |d dot u|
        ---------
        |d| dot |u|

        //Where d is the direction of the plane and u is the plane
        //We can use the normal of the plane to find the complmentary angle or the prjection vector on the plane
        */

        //First we must normalize the velocity vector to get the direction 
        Vector3 normVel = Vector3.Normalize(rBody.velocity);

        //Find the angle from the direction to the plane
        float angleInRads = Mathf.Asin((Mathf.Abs((normVel.x * Vector3.up.x) +
                                           (normVel.y * Vector3.up.y) +
                                           (normVel.z * Vector3.up.z)) / 
                                           (Mathf.Abs(Mathf.Sqrt(Mathf.Pow(normVel.x,2)+
                                                                Mathf.Pow(normVel.y, 2)+
                                                                Mathf.Pow(normVel.z, 2)) 
                                                                *
                                                     Mathf.Sqrt(Mathf.Pow(Vector3.up.x, 2) +
                                                                Mathf.Pow(Vector3.up.y, 2) +
                                                                Mathf.Pow(Vector3.up.z, 2))))));

        //Convert the angle in Radians to Degrees
        float angleInDegs = angleInRads * 180 / Mathf.PI;
        //Debug.Log(angleInDegs);


        //Check to see if the plane if going up or down with the new angle
        float upOrDown = normVel.y;
        //If we are going up or down and the pplane is angled more than 15 degrees that start accelerating. Gives a bit of leeway to  fly straight
        if (upOrDown > 0.0f && angleInDegs > 15.0)
        {
            acceleration = -accelerationUp * (angleInDegs / maxAngle);

        }
        else if (upOrDown < 0.0f && angleInDegs > 15.0)
        {
            acceleration = accelerationDown * (angleInDegs / maxAngle);
        }
        else
        {
            acceleration = 0.0f;
        }
    }

    private void reachedTippingPoint()
    {
        //Lerps the plane to downward position
        step += tippingSpeed * Time.deltaTime;
        Vector3 lerpDirection = Vector3.Lerp(Vector3.Normalize(moveDirection), Vector3.down, step);
        transform.rotation = Quaternion.LookRotation(lerpDirection);
        transform.rotation *= Quaternion.AngleAxis(rollAngle, Vector3.forward);
    }

    private void InputCheck()
    {
        if (rollCheck == 0.0f && pitchCheck == 0.0f && yawCheck == 0.0f)
        {
            cam.gettingInput = false;
        }
        else
        {
            cam.gettingInput = true;
        }
    }


    //---Collision Functions---//

    private void OnCollisionEnter(Collision other)
    {
        //Attempt to fix collision
        Debug.Log(other.contacts[0].normal);
        Vector3 colDirection = Vector3.Lerp(Vector3.Normalize(moveDirection), other.contacts[0].normal, 100.0f);
        transform.rotation = Quaternion.LookRotation(colDirection);
        transform.rotation *= Quaternion.AngleAxis(rollAngle, Vector3.forward);
    }


    private void OnTriggerEnter(Collider vent)
    {

        VentScript ventProperty = vent.GetComponent<VentScript>();
        Debug.Log(ventProperty.ventPower);
        if (vent.tag == "Vent")
        {
            /*Momentum base implementation of the vents
            Vector3 ventDirection = Vector3.Lerp(Vector3.Normalize(moveDirection), vent.transform.up, 0.7f);
            transform.rotation = Quaternion.LookRotation(ventDirection);
            transform.rotation *= Quaternion.AngleAxis(rollAngle, Vector3.forward);
            acceleration = ventProperty.ventPower;
            */

            //Transform base vent implementation
            //Debug.Log(vent.transform.up * ventProperty.ventPower);
            //Vector3 ventDirection = Vector3.Lerp( transform.position, transform.position + (vent.transform.up * ventProperty.ventPower), 0.7f);
            transform.position += vent.transform.up * ventProperty.ventPower;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        acceleration = 0.0f;
    }

}
