using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Camera_Follow : MonoBehaviour
{
    //---Setup Variables---//
    [SerializeField] private CinemachineFreeLook cmFL;
    [SerializeField] private Transform playerTarget;
    //Checks to see if we are receiving input
    [HideInInspector] public bool gettingInput = true;
    //This bool is to check if the cmFL was previously turned off so we can set the cmFL back to the middle
    private bool previousGetInput = false;

    //---Camera Control Variables---//
    [Tooltip("Time spent smoothing to new position. Smaller more time spent smoothing ")]
    public float smoothSpeed = 1.0f;
    public Vector3 offset;


    private void Start()
    {
        //cmFL = false;
        cmFL.gameObject.SetActive(false);

    }
    private void FixedUpdate()
    {

        Vector3 desiredPostion = playerTarget.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed );
        transform.position = smoothPosition;

        transform.LookAt(playerTarget);

        //transform.rotation = Quaternion.Lerp(transform.rotation, playerTarget.transform.rotation, 5f * Time.deltaTime);



        //Toggle Freelook if getting input
        //EnableFreeLook();
    }



    private void EnableFreeLook()
    {
        //See PLayerControllerScript for the actual switching of the boolean. 
        if (gettingInput)
        {
            cmFL.gameObject.SetActive(false);
            previousGetInput = false;
        }
        else
        {

            cmFL.gameObject.SetActive(true);
            if (!previousGetInput)
            {
                cmFL.m_YAxis.Value = 0.5f;
                cmFL.m_XAxis.Value = 0.0f;
                previousGetInput = true;
            }
        }

        //cmFL.gameObject.SetActive(!gettingInput);

    }





}
