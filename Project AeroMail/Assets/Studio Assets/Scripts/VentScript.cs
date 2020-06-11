using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentScript : MonoBehaviour
{
    //Poor Implementation of the vents but it kinda works
    //--- Public Variables---//
    public float upForce = 10000.0f;
    public float forwardForce = 10000.0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Vent used Whirlwind");
            other.GetComponent<Rigidbody>().AddForce(0, upForce, forwardForce * other.transform.up.y, ForceMode.Acceleration);
        }
    }
}
