using UnityEngine;
using System.Collections.Generic;

public class CloudGenerator_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("References")]
    public GameObject m_cloudSpawnBox;
    public Transform m_cloudParent;

    [Header("Spawn Controls")]
    [Range(0, 1000)] public int m_numClouds;
    public float m_minCloudSize;
    public float m_maxCloudSize;
    public GameObject[] m_cloudPrefabs;



    //--- Unity Methods ---//
    private void Awake()
    {
        // Hide the cloud spawn box during play
        m_cloudSpawnBox.GetComponent<Renderer>().enabled = false;
    }



    //--- Methods ---//
    public void GenerateClouds()
    {
        // Determine the minimum and maximum spawn positions
        Bounds bounds = m_cloudSpawnBox.GetComponent<BoxCollider>().bounds;
        Vector3 minPos = bounds.min;
        Vector3 maxPos = bounds.max;

        // Delete any existing clouds
        DeleteClouds();

        // Randomly generate the clouds
        for (int i = 0; i < m_numClouds; i++)
        {
            // Determine the cloud object to spawn
            int randCloudIdx = Random.Range(0, m_cloudPrefabs.Length);
            GameObject cloudPrefab = m_cloudPrefabs[randCloudIdx];

            // Determine the spawn position
            float spawnX = Random.Range(minPos.x, maxPos.x);
            float spawnY = Random.Range(minPos.y, maxPos.y);
            float spawnZ = Random.Range(minPos.z, maxPos.z);
            Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);

            // Determine the spawn rotation
            float rotY = Random.Range(0.0f, 360.0f);
            Quaternion spawnRot = Quaternion.Euler(0.0f, rotY, 0.0f);

            // Determine the spawn scale
            float scale = Random.Range(m_minCloudSize, m_maxCloudSize);

            // Spawn the cloud as a child of the set parent object
            var cloud = Instantiate(cloudPrefab, spawnPos, spawnRot, m_cloudParent);
            cloud.transform.localScale = Vector3.one * scale;
        }
    }

    public void DeleteClouds()
    {
        // Delete any existing clouds
        while (m_cloudParent.childCount > 0)
            DestroyImmediate(m_cloudParent.GetChild(0).gameObject);
    }

    

    //--- Getters ---//
    public bool CanDestroyClouds()
    {
        return m_cloudParent.childCount > 0;
    }
}
