using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn_PlayerController : MonoBehaviour
{
    public ParticleSystem particles;
    public GameObject m_respawnUI;

    private Respawn_Point[] m_allSpawnPoints;
    private Respawn_Zone m_currentRespawnZone;
    private bool m_canRespawn;

    // Start is called before the first frame update
    void Awake()
    {
        m_allSpawnPoints = GameObject.FindObjectsOfType<Respawn_Point>();
        m_canRespawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canRespawn && Input.GetKeyDown(KeyCode.R))
            Respawn();
    }

    private void OnCollisionEnter(Collision collision)
    { 
        if (collision.gameObject.tag == "Environment")
        {
            GetComponentInChildren<Renderer>().enabled = false;
            GetComponent<Player_Control>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;

            particles.Play();

            m_canRespawn = true;
            m_respawnUI.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn_Zone")
        {
            Respawn_Zone spawnZoneScript = other.gameObject.GetComponent<Respawn_Zone>();

            this.m_currentRespawnZone = spawnZoneScript;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Respawn_Zone")
        {
            this.m_currentRespawnZone = null;
        }
    }

    public Respawn_Point DetermineRespawnPoint()
    {
        if (m_currentRespawnZone == null)
        {
            // Find the closest point and spawn there
            float closestDist = Mathf.Infinity;
            Respawn_Point closestPoint = null;

            foreach(var spawnPoint in m_allSpawnPoints)
            {
                float distToPoint = Vector3.Distance(spawnPoint.Position, this.transform.position);

                if (distToPoint < closestDist)
                {
                    closestPoint = spawnPoint;
                    closestDist = distToPoint;
                }
            }

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
        var spawnPoint = DetermineRespawnPoint();

        this.transform.position = spawnPoint.Position;
        this.transform.rotation = spawnPoint.Rotation;

        GetComponentInChildren<Renderer>().enabled = true;
        GetComponent<Player_Control>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;

        m_canRespawn = false;
        m_respawnUI.SetActive(false);
    }
}
