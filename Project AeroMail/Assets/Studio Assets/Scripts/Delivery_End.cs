using UnityEngine;
using System.Collections;

public class Delivery_End : MonoBehaviour
{
    //--- Public Variables ---//
    public Delivery_Start m_startZone;
    public Camera m_zoneCam;



    //--- Methods ---//
    public void HandlePlayerInteraction()
    {
        // Deactivate this object
        this.gameObject.SetActive(false);
    }
}
