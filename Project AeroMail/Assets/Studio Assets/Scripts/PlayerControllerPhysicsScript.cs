using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerPhysicsScript : MonoBehaviour
{

    //---Public Variables---//

    public InputMaster controls;



    //---Private Variables---//
    private Rigidbody rBody;
    private Transform tBody;

    private float movementSpeed = 100.0f;
    private float yawSpeed =    500.0f;
    private float pitchSpeed =  500.0f;
    private float rollSpeed =   500.0f;

    //---Controls Input Variables---///
    private Vector2 yawPitch;
    private float rollCheck;
    private float pitchCheck;
    private float yawCheck;

    //---Wing Variables
    private float thrustPower = 10.0f;
    private float thrustUp = 10.0f;
    private float thrustForward = 10.0f;
    private float thurstUpVetn = 1000.0f;

    private float drag = 10.0f;
    private Vector3 liftFactor = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 forwardVelocity;

    //---Drag Variables---//
    private float resistanceFactor = 0.5f;
    private float radius = 1.0f;
    private float area;
    private float maxResistance;


    //---Calculation Variables---///
    private Vector3 pitch = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 yaw = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 roll = new Vector3(0.0f, 0.0f, 0.0f);
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
        tBody = GetComponent<Transform>();

        area = Mathf.PI * Mathf.Pow(radius, 2);
        maxResistance = area * Mathf.Pow(resistanceFactor, 3);
    }

    private void OnEnable()
    {
        controls.PlayerController.Enable();
    }

    private void Disable()
    {
        controls.PlayerController.Disable();
    }

    private void Update()
    {
        LiftOff();
        DirectionCalculation();
        CalculateDrag();
        //Make this plane move forward
        Movement();

    }

    private void Movement()
    {
        rBody.velocity = transform.forward * movementSpeed;
    }

    private void DirectionCalculation()
    {
        float y = yawCheck * yawSpeed * Time.deltaTime;
        float p = pitchCheck * pitchSpeed * Time.deltaTime;
        float r = rollCheck * rollSpeed * Time.deltaTime;
        //Add forces
        rBody.AddTorque(transform.up * y);
        //Change -p to p if you want to inverse
        rBody.AddTorque(transform.right * -p);
        rBody.AddTorque(transform.forward * -r);
    }

    void LiftOff()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
            rBody.AddRelativeForce(0, thrustUp, thrustForward, ForceMode.Impulse);
    }

    private float CalculateResistance()
    {
        Vector3 direction =  rBody.velocity;

        float angle = Vector3.Angle(transform.forward, direction);

        return Mathf.Abs(Mathf.Sin(angle));
    }

    private float PlaneMagnitude()
    {
        return rBody.velocity.magnitude;
    }

    private void CalculateDrag()
    {

        float magnitude = maxResistance * CalculateResistance() * PlaneMagnitude();
        Vector3 direction = transform.forward.normalized * -1;
        Debug.Log(magnitude);
        rBody.AddRelativeForce(direction * magnitude);
    }
}
