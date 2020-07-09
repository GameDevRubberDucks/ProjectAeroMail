using UnityEngine;

public class Menu_CloudController : MonoBehaviour
{
    //--- Public Variables ---//
    public GameObject[] m_cloudPrefabs;
    public Transform m_cloudSpawnLoc;
    public float m_spawnCooldownLength;



    //--- Private Variables ---//
    private bool m_timeSinceLastSpawn;

    // Start is called before the first frame update
    void Start()
    {
        // Init the private variables
        //m_timeSinceLastSpawn = m_spawnCooldownLength;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
