using UnityEngine;
using UnityEngine.Assertions;
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
        m_onTargetZoneChanged.RemoveAllListeners();
    }



    //--- Methods ---//
    public void NextDelivery()
    {
        // Activate the next delivery in the sequence
        // TODO: Do something special if that is the end of the deliveries (ex: show game over?)
        ActivateDelivery(m_activeID + 1);
    }

    public void ActivateDelivery(int _id)
    {
        // Ensure the new ID is in the correct range
        Assert.IsTrue(_id >= 0 && _id < m_allDeliveries.Count, "_id must be a valid index for m_allDeliveries");

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
        get => m_onTargetZoneChanged;
    }

    public Delivery_Zone TargetZone
    {
        get => m_targetZone;
        set
        {
            // Store the new target zone value
            m_targetZone = value;

            // Trigger the event to indicate that the target zone has been updated
            m_onTargetZoneChanged.Invoke(m_targetZone);
        }
    }
}
