using UnityEngine;
using Cinemachine;

public class Player_Control : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform bodyAnimationObj;
    public float movementSpeed = 100.0f;
    public float pitchSpeed = 50.0f;
    public float yawSpeed = 50.0f;
    public float rollMax = 60.0f;
    public bool isInvertedPitch = true;
    public float pitchAbsClampAmount = 80;
    public CinemachineFreeLook cmFL;
    public float boostSpeedMultiplier = 2.0f;
    public float boostYawDivisor = 2.0f;



    //--- Private Variables ---//
    private float currentPitch;
    private float baseMoveSpeed;
    private float baseYawSpeed;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        currentPitch = 0.0f;
        baseMoveSpeed = movementSpeed;
        baseYawSpeed = yawSpeed;
    }

    private void Update()
    { 
        // Handle the Yaw and Pitch movements
        HandleYaw();
        HandlePitch();

        // Toggles free look by pressing space
        InputCheck();

        // Speedboost while holding shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = baseMoveSpeed * boostSpeedMultiplier;
            yawSpeed = baseYawSpeed / boostYawDivisor;
        }
        else
        {
            movementSpeed = baseMoveSpeed;
            yawSpeed = baseYawSpeed;
        }

        // Toggle the y-axis inversion by pressing 'I'
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInvertedYAxis();

        // Animate the roll effect
        Vector3 rotAngles = bodyAnimationObj.localRotation.eulerAngles;
        rotAngles.z = rollMax * Input.GetAxis("Horizontal");
        bodyAnimationObj.localRotation = Quaternion.Euler(rotAngles);

        // Always move forward
        float movementAmount = movementSpeed * Time.deltaTime;
        Vector3 movementVec = transform.forward * movementAmount;
        transform.position += movementVec;
    }


    //--- Methods ---//
    public void HandleYaw()
    {
        // Calculate the amount of movement on the yaw
        float yawInput = Input.GetAxis("Horizontal");
        float yawAmount = yawInput * yawSpeed * Time.deltaTime;

        // Rotate according to the yaw
        Vector3 rotAngles = transform.rotation.eulerAngles;
        float newRotYaw = rotAngles.y + yawAmount;
        transform.rotation = Quaternion.Euler(rotAngles.x, newRotYaw, rotAngles.z);
    }

    public void HandlePitch()
    {
        // Calculate the amount of movement on the pitch
        float pitchInput = Input.GetAxis("Vertical");
        pitchInput = (isInvertedPitch) ? -pitchInput : pitchInput;
        float pitchAmount = pitchInput * pitchSpeed * Time.deltaTime;

        // Since we are using +ve and -ve angles, its best to store the pitch separately and then have unity convert it
        // Calculate and clamp the new value between -ve and +ve absolute maxes
        currentPitch += pitchAmount;
        currentPitch = Mathf.Clamp(currentPitch, -pitchAbsClampAmount, pitchAbsClampAmount);

        // Apply the new pitch value and have Unity convert it to the right angle automatically
        Vector3 rotAngles = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(currentPitch, rotAngles.y, rotAngles.z);
    }

    public void InputCheck()
    {
        bool check = Input.GetKey(KeyCode.C);

        if (check)
        {
            cmFL.m_XAxis.m_InputAxisName = "Mouse X";
            cmFL.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            cmFL.m_XAxis.m_InputAxisName = null;
            cmFL.m_YAxis.m_InputAxisName = null;
            cmFL.m_YAxis.Value = 0.5f;
            cmFL.m_XAxis.Value = 0.0f;
        }
    }

    public void ToggleInvertedYAxis()
    {
        isInvertedPitch = !isInvertedPitch;
    }
}
