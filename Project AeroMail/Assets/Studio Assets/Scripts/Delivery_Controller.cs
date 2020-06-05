using UnityEngine;
using System.Collections.Generic;

public class Delivery_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    public List<Delivery> m_allDeliveries;



    //--- Private Variables ---//
    private Delivery_ZoneChangeEvent m_onTargetZoneChanged;
    private Delivery m_activeDelivery;
    private Delivery_Zone m_targetZone;
    private int m_activeID;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Init the private variables
        m_activeDelivery = null;
        m_targetZone = null;
        m_activeID = 0;

        // Activate the first delivery
        ActivateDelivery(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Delivery trigger zones need to be handled as the possible start or end of a mission
        if (other.tag == "Delivery_Zone")
            HandleZoneEntry(other.gameObject);
    }

    private void OnDisable()
    {
        // Clear all of the listeners that are left on the events
        OnTargetZoneChanged.RemoveAllListeners();
    }



    //--- Methods ---//
    public void NextDelivery()
    {
        // Determine the next delivery ID in the sequence
        int nextDeliveryID = m_activeID + 1;

        // If this is the end of the deliveries, do something special
        // Otherwise, simply begin the next delivery
        if (nextDeliveryID >= m_allDeliveries.Count)
        {
            // TODO: Add feedback
            Debug.Log("All deliveries completed");
        }
        else
        {
            // Activate the next delivery
            ActivateDelivery(nextDeliveryID);
        }      
    }

    public void ActivateDelivery(int _id)
    {
        // Set the new delivery ID
        m_activeID = _id;

        // Activate the delivery itself
        m_activeDelivery = m_allDeliveries[_id];

        // The new target zone should be the starting point for this delivery
        TargetZone = m_activeDelivery.m_startZone;
    }

    public void HandleZoneEntry(GameObject _zone)
    {
        // We first need to grab the zone component from the trigger's parent
        Delivery_Zone zoneComp = _zone.GetComponentInParent<Delivery_Zone>();

        // If the zone is part of the active mission, we need to handle it as such
        // Otherwise, we can just ignore it since this is an irrelevant zone
        if (m_activeDelivery.m_startZone == zoneComp)
        {
            // We just entered the starting zone for this delivery
            // This means that our new target is the ending zone for it
            // TODO: Add feedback here
            TargetZone = m_activeDelivery.m_endZone;
        }
        else if (m_activeDelivery.m_endZone == zoneComp)
        {
            // We just entered the ending zone for this delivery
            // This means that we should start the next delivery
            // TODO: Add feedback here
            NextDelivery();
        }
    }



    //--- Setters and Getters ---//
    public Delivery_ZoneChangeEvent OnTargetZoneChanged
    {
        get
        {
            // Ensure the event has been initialized first
            if (m_onTargetZoneChanged == null)
                m_onTargetZoneChanged = new Delivery_ZoneChangeEvent();

            // Return the event object
            return m_onTargetZoneChanged;
        } 
    }

    public Delivery_Zone TargetZone
    {
        get => m_targetZone;
        set
        {
            // Store the new target zone value
            m_targetZone = value;

            // Trigger the event to indicate that the target zone has been updated
            OnTargetZoneChanged.Invoke(m_targetZone);
        }
    }
}
