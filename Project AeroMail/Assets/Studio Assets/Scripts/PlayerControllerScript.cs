using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine;


public class PlayerControllerScript : MonoBehaviour
{

    //---Public Variables---//

    public InputMaster controls;


    //---Private Variables---//
    private Rigidbody rBody;

    private float movementSpeed = 100.0f;
    private float yawSpeed = 100.0f;
    private float pitchSpeed = 100.0f;
    private float rollSpeed = 400.0f;

    //---Controls Input Variables---///
    private Vector2 yawPitch;
    private float rollCheck;
    private float pitchCheck;
    private float yawCheck;
    //---Calculation Variables---///
    private Vector3 pitch = new Vector3(0.0f,0.0f,0.0f);
    private Vector3 yaw = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 roll = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
    private float rollAngle;

    [SerializeField] private CinemachineFreeLook cameraFL;

    // Start is called before the first frame update
    void Awake()
    {
        controls = new InputMaster();
        //controls.PlayerController.PlaneMovement.performed += ctx => yawPitch = ctx.ReadValue<Vector2>();
        controls.PlayerController.Roll.performed += ctx => rollCheck = ctx.ReadValue<float>();
        controls.PlayerController.Yaw.performed += ctx => yawCheck = ctx.ReadValue<float>();
        controls.PlayerController.Pitch.performed += ctx => pitchCheck = ctx.ReadValue<float>();
        rBody = GetComponent<Rigidbody>();
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
    
        //When we have a proper model we can change trasnform.right to transform.forward
        moveDirection = transform.forward * movementSpeed;

        yaw = yawCheck * yawSpeed * Time.deltaTime * transform.right;
        pitch = pitchCheck * pitchSpeed * Time.deltaTime * transform.up;

        rollAngle -= rollCheck * rollSpeed * Time.deltaTime;
        moveDirection += pitch + yaw;
        transform.rotation = Quaternion.LookRotation(moveDirection);
        transform.rotation *= Quaternion.AngleAxis(rollAngle, Vector3.forward);
        //Make this plane move forward
        Movement();
    }

    private void Movement()
    {
        rBody.velocity = transform.forward * movementSpeed;
    }





    //---Delete Later if not used---//

    //public void OnYaw(InputAction.CallbackContext context)
    //{
    //    Debug.Log(context.ReadValue<float>());
    //    //Controls the Yaw
    //    yaw = context.ReadValue<float>() * yawSpeed * Time.deltaTime * transform.right;
    //}
    //public void OnPitch(InputAction.CallbackContext context)
    //{
    //    //Controls the Pitch
    //    Debug.Log("And The Pitch");
    //
    //    pitch = context.ReadValue<float>() * pitchSpeed * Time.deltaTime  * transform.up;
    //    
    //}
    //public void OnRoll(InputAction.CallbackContext context)
    //{
    //    Debug.Log("They See Me Rollin'");
    //    //this.transform.Rotate(0.0f,0.0f, context.ReadValue<float>() * rollSpeed * Time.deltaTime);
    //    roll = context.ReadValue<float>() * rollSpeed * Time.deltaTime * transform.forward;
    //
    //}

}
