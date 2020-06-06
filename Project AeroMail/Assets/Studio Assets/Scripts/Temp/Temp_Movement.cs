using UnityEngine;

public class Temp_Movement : MonoBehaviour
{
    //--- Public Variables ---//
    public float m_movementSpeed;



    //--- Unity Methods ---//
    private void Update()
    {
        // Only able to move on the x and z axes, no vertical
        float moveX = 0.0f;
        float moveZ = 0.0f;

        // Move forwards and backwards using W and S
        if (Input.GetKey(KeyCode.W))
            moveZ = m_movementSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            moveZ = -m_movementSpeed * Time.deltaTime;

        // Move left and right using A and D
        if (Input.GetKey(KeyCode.A))
            moveX = -m_movementSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            moveX = m_movementSpeed * Time.deltaTime;

        // Apply the movement
        this.transform.position += new Vector3(moveX, 0.0f, moveZ);
    }
}
