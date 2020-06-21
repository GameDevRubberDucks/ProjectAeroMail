using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //--- Public Variables ---//
    public Camera camera;
    public Transform cameraLookAtPoint;
    public Transform playerBody;



    //--- Unity Methods ---//
    private void Update()
    {
        // Calculate the target look at position
        Vector3 targetLookAtPosition = cameraLookAtPoint.position;

        // Face towards the target
        Vector3 lookVec = targetLookAtPosition - camera.transform.position;
        lookVec.Normalize();
        camera.transform.forward = lookVec;
    }

    public void LookAhead(Vector3 _lookAheadPosition)
    {

    }
}
