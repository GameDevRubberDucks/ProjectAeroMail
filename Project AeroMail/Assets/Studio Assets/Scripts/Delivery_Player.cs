using UnityEngine;
using System.Collections.Generic;

public class Delivery_Player : MonoBehaviour
{
    //--- Private Variables ---//
    private Delivery_TargetChangeEvent m_OnTargetZoneChanged;
    private Delivery_TargetListChangeEvent m_OnTargetListChanged;
    private List<Delivery_End> m_possibleTargets;
    private Delivery_End m_currentTarget;



    //--- Unity Methods ---//
    private void Start()
    {
        // Init the private variables
        m_possibleTargets = new List<Delivery_End>();
        CurrentTarget = null;
    }

    private void Update()
    {
        // Use Q and E to switch between targets
        if (Input.GetKeyDown(KeyCode.Q))
            PrevTarget();
        else if (Input.GetKeyDown(KeyCode.E))
            NextTarget();
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
        OnTargetListChanged.RemoveAllListeners();
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

        // This new target should always become the immediate focus
        CurrentTarget = _target;

        // Invoke the event since the list changed
        OnTargetListChanged.Invoke(m_possibleTargets.Count);
    }

    public void RemoveTargetFromList(Delivery_End _target)
    {
        // Get the index of the target
        int targetIndex = m_possibleTargets.IndexOf(_target);

        // Remove the target from the list
        m_possibleTargets.Remove(_target);

        // If the target is the active one, we need to switch to a different one
        // Otherwise, we can stay focused on the same one
        if (_target == m_currentTarget)
        {
            // If there is another target we can instantly jump to, do that. Otherwise, change the target to null
            if (m_possibleTargets.Count > 0)
            {
                // Go back to the previous target
                PrevTarget();
            }
            else
            {
                // There is no new target
                CurrentTarget = null;
            }
        }

        // Invoke the event since the list changed
        OnTargetListChanged.Invoke(m_possibleTargets.Count);
    }

    public void NextTarget()
    {
        // Ensure there is a target to change to
        if (m_possibleTargets.Count == 0)
            return;

        // Determine the index of the next target
        int currentIndex = m_possibleTargets.IndexOf(m_currentTarget);
        int nextIndex = currentIndex + 1;

        // Wrap the index if need be
        if (nextIndex >= m_possibleTargets.Count)
            nextIndex = 0;

        // Change to the new target
        CurrentTarget = m_possibleTargets[nextIndex];
    }

    public void PrevTarget()
    {
        // Ensure there is a target to change to
        if (m_possibleTargets.Count == 0)
            return;

        // Determine the index of the previous target
        int currentIndex = m_possibleTargets.IndexOf(m_currentTarget);
        int prevIndex = currentIndex - 1;

        // Wrap the index if need be
        if (prevIndex < 0)
            prevIndex = m_possibleTargets.Count - 1;

        // Change to the new target
        CurrentTarget = m_possibleTargets[prevIndex];
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

    public Delivery_TargetListChangeEvent OnTargetListChanged
    {
        get
        {
            // Ensure the event has been initialized first
            if (m_OnTargetListChanged == null)
                m_OnTargetListChanged = new Delivery_TargetListChangeEvent();

            // Return the event object
            return m_OnTargetListChanged;
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
