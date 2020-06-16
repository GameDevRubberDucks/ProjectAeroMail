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



    //--- Unity Methods ---//
    void Update()
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

        // Rotate according to the pitch
        Vector3 rotAngles = transform.localRotation.eulerAngles;
        float newPitchYaw = rotAngles.x + pitchAmount;

        // Clamp the pitch to prevent going straight up or down
        if (newPitchYaw > 180.0f)
            newPitchYaw = Mathf.Clamp(-(360.0f - newPitchYaw), -pitchAbsClampAmount, pitchAbsClampAmount);

        transform.localRotation = Quaternion.Euler(newPitchYaw, rotAngles.y, rotAngles.z);
    }
}
