using UnityEngine;
using System.Collections;

public class Delivery_Start : MonoBehaviour
{
    //--- Public Variables ---//
    public Delivery_End m_endZone;



    //--- Unity Methods ---//
    private void Start()
    {
        // Disable the end zone object
        m_endZone.gameObject.SetActive(false);
    }



    //--- Methods ---//
    public void HandlePlayerInteraction()
    {
        // Activate the end zone object
        m_endZone.gameObject.SetActive(true);

        // Deactivate this object
        this.gameObject.SetActive(false);
    }
}
