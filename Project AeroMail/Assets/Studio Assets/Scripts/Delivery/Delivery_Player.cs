using UnityEngine;
using System.Collections.Generic;

public class Delivery_Player : MonoBehaviour
{
    //--- Private Variables ---//
    private Delivery_TargetChangeEvent m_OnTargetZoneChanged;
    private List<Delivery_End> m_possibleTargets;
    private Delivery_End m_currentTarget;



    //--- Unity Methods ---//
    private void Start()
    {
        // Init the private variables
        m_possibleTargets = new List<Delivery_End>();
        CurrentTarget = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object is a delivery zone, we need to handle it
        if (other.tag == "Delivery_Zone")
            HandleZoneEntry(other.gameObject);
    }

    private void OnDisable()
    {
        // Clear all of the listeners that are left on the events
        OnTargetZoneChanged.RemoveAllListeners();
    }



    //--- Methods ---//
    public void HandleZoneEntry(GameObject _zoneObj)
    {
        // Determine if the zone is a start or end zone based on the script attached to it
        var startZoneComp = _zoneObj.GetComponentInParent<Delivery_Start>();
        var endZoneComp = _zoneObj.GetComponentInParent<Delivery_End>();

        // Handle the different zone types accordingly
        if (startZoneComp != null)
        {
            // Then, we should tell the start zone to deactivate itself and instead activate the end zone
            startZoneComp.HandlePlayerInteraction();
            
            // When hitting a start zone, we should add the corresponding end zone to our target list
            AddTargetToList(startZoneComp.m_endZone);
        }
        else if (endZoneComp != null)
        {
            // Then, we should tell the end zone to deactivate itself
            endZoneComp.HandlePlayerInteraction();

            // When hitting an end zone, we should remove it from our list of targets
            RemoveTargetFromList(endZoneComp);
        }
        else
        {
            // If we didn't find a zone, then we have an error for some reason
            Debug.LogError("No start or end zone component found on the zone object!");
        }
    }

    public void AddTargetToList(Delivery_End _target)
    {
        // Add the target to the list
        m_possibleTargets.Add(_target);

        // If there isn't a current target, this should become the new target
        if (m_currentTarget == null)
            CurrentTarget = _target;
    }

    public void RemoveTargetFromList(Delivery_End _target)
    {
        // Get the index of the target
        int targetIndex = m_possibleTargets.IndexOf(_target);

        // Remove the target from the list
        m_possibleTargets.Remove(_target);

        // If there is another target we can instantly jump to, do that. Otherwise, change the target to null
        if (m_possibleTargets.Count > 0)
        {
            // Go back to the previous target
            targetIndex--;

            // Wrap around if needed
            if (targetIndex < 0)
                targetIndex = m_possibleTargets.Count - 1;

            // Set the new target
            CurrentTarget = m_possibleTargets[targetIndex];
        }
        else
        {
            // There is no new target
            CurrentTarget = null;
        }
    }



    //--- Setters and Getters ---//
    public Delivery_TargetChangeEvent OnTargetZoneChanged
    {
        get
        {
            // Ensure the event has been initialized first
            if (m_OnTargetZoneChanged == null)
                m_OnTargetZoneChanged = new Delivery_TargetChangeEvent();

            // Return the event object
            return m_OnTargetZoneChanged;
        }
    }

    public Delivery_End CurrentTarget
    {
        get => m_currentTarget;
        set
        {
            // Set the new target value
            m_currentTarget = value;

            // Trigger the event
            OnTargetZoneChanged.Invoke(m_currentTarget);
        }
    }
}
