using UnityEngine;

public class Respawn_PlayerController : MonoBehaviour
{
    //--- Public Variables ---//
    public ParticleSystem m_crashParticles;
    public GameObject m_respawnUI;



    //--- Private Variables ---//
    private Respawn_Point[] m_allSpawnPoints;
    private Respawn_Zone m_currentRespawnZone;
    private bool m_canRespawn;



    //--- Unity methods ---//
    private void Awake()
    {
        // Init the private variables
        m_allSpawnPoints = GameObject.FindObjectsOfType<Respawn_Point>();
        m_currentRespawnZone = null;
        m_canRespawn = false;
    }

    private void Update()
    {
        // Look for an attempt to respawn but only if the player is actually able to do so
        if (m_canRespawn && Input.GetKeyDown(KeyCode.R))
            Respawn();
    }

    private void OnCollisionEnter(Collision collision)
    { 
        // Consider the player destroyed when they hit anything tagged as environmental objects
        if (collision.gameObject.tag == "Environment")
        {
            // The player has crashed so we should disable control among other things
            TogglePlayerCrashState(true);

            // Play effects for when they crashed
            m_crashParticles.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player enters a controlled respawn zone, we need to track it
        if (other.tag == "Respawn_Zone")
        {
            Respawn_Zone spawnZoneScript = other.gameObject.GetComponent<Respawn_Zone>();
            this.m_currentRespawnZone = spawnZoneScript;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player leaves a controlled respawn zone, we need to track it
        if (other.tag == "Respawn_Zone")
            this.m_currentRespawnZone = null;
    }

    public Respawn_Point DetermineRespawnPoint()
    {
        // If the player is in a respawn zone, we should use that zone's specific respawn point
        // Otherwise, we should just find the closest one
        if (m_currentRespawnZone == null)
        {
            // Need to store the closest one as we search
            float closestDist = Mathf.Infinity;
            Respawn_Point closestPoint = null;

            // Look through all of the points and find the closest one
            foreach(var spawnPoint in m_allSpawnPoints)
            {
                float distToPoint = Vector3.Distance(spawnPoint.Position, this.transform.position);

                if (distToPoint < closestDist)
                {
                    closestPoint = spawnPoint;
                    closestDist = distToPoint;
                }
            }

            // Return the closest point so we can spawn there
            return closestPoint;
        }
        else
        {
            // Spawn at the respawn zone's attached spawn point
            return m_currentRespawnZone.m_respawnPoint;
        }
    }

    public void Respawn()
    {
        // Determine the spawn point based on whether or not the player is in a respawn zone
        var spawnPoint = DetermineRespawnPoint();

        // Move the player to the spawn point and rotate them to face the point's direction
        this.transform.position = spawnPoint.Position;
        this.transform.rotation = spawnPoint.Rotation;

        // Enable player control
        TogglePlayerCrashState(false);
    }

    private void TogglePlayerCrashState(bool _hasCrashed)
    {
        // When the player has crashed, they should not be controllable anymore
        GetComponentInChildren<Renderer>().enabled = !_hasCrashed;
        GetComponent<Player_Control>().enabled = !_hasCrashed;
        GetComponent<Rigidbody>().isKinematic = _hasCrashed;

        // If they have crashed, they can respawn
        m_canRespawn = _hasCrashed;
        m_respawnUI.SetActive(_hasCrashed);
    }
}
