using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform bodyAnimationObj;
    public float movementSpeed = 100.0f;
    public float pitchSpeed = 50.0f;
    public float yawSpeed = 50.0f;
    public float rollMax = 60.0f;
    public bool isInvertedPitch = true;
    public float pitchAbsClampAmount = 80;



    //--- Private Variables ---//
    private float currentPitch;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        currentPitch = 0.0f;
    }

    private void Update()
    { 
        // Handle the Yaw and Pitch movements
        HandleYaw();
        HandlePitch();

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
}
