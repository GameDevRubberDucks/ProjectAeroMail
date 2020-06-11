using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetation_ShaderController : MonoBehaviour
{
    //--- Public Variables ---//
    public Transform m_playerObj;
    


    //--- Private Variables ---//
    private Material m_mat;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        // Grab the player's updated position and pass it to the shader
        Vector3 newPos = m_playerObj.position;
        m_mat.SetVector("_playerPosition", new Vector4(newPos.x, newPos.y, newPos.z, 0.0f));
    }
}
