using UnityEngine;

public class Delivery_Zone : MonoBehaviour
{
    //--- Public Variables ---//
    public Camera m_zoneCamera;



    //--- Methods ---//
    public void ToggleZoneCamera(bool _active)
    {
        // Enable or disable the camera according to the request
        m_zoneCamera.enabled = _active;
    }
}
