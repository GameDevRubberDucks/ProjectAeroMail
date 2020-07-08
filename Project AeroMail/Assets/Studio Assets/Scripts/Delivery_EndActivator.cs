using UnityEngine;

public class Delivery_EndActivator : MonoBehaviour
{
    //--- Public Variables ---//
    public GameObject m_connectedZone;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Also hide the zone at the beginning as well
        m_connectedZone.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player enters the zone, show the end zone object
        if (other.GetComponentInParent<Delivery_Player>() != null)
            m_connectedZone.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player leaves the zone, hide the end zone object
        if (other.GetComponentInParent<Delivery_Player>() != null)
            m_connectedZone.SetActive(false);
    }
}
