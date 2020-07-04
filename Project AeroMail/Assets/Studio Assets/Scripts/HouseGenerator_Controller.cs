using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

// ComponentSet Class
// This is basically just a container for a list of objects
// Need this so that since Unity cannot display 2D lists by default, unless they are wrapped like this
[System.Serializable]
public class ComponentSet
{
    [SerializeField] public List<GameObject> m_setObjs;

    public ComponentSet()
    {
        m_setObjs = new List<GameObject>();
    }
}

public class HouseGenerator_Controller : MonoBehaviour
{
    //--- Public Variables ---//
    [Header("Saving Controls")]
    [Tooltip("The name of the file. Files will be saved like [name]_[#].prefab")] 
    public string m_namePrefixStr = "house";
    [Tooltip("The folder to save all the prefabs to. This MUST start with Assets/")] 
    public string m_saveLocation = "Assets/Studio Assets/Prefabs/Houses/";

    [Header("Spawning Controls")]
    [Tooltip("The distance between the spawned objects on the X and Z, respectively")] 
    public Vector2 m_spawnOffsets = new Vector2(5.0f, 5.0f);
    [Tooltip("The spawning occurs in a grid. This is how many objects get spawned in a row before moving to the next one")] 
    public int m_gridRowLength = 10;

    [Header("Renderable Information")]
    [Tooltip("The list of materials that the objects will be assigned when generated. All of the combinations will be made with each of the materials")]
    public Material[] m_materials;
    [Tooltip("The different sets of objects that can be combined. It is best if all objects in the set are relative to the center of the world of the intended object since all spawning occurs at (0,0,0)")]
    public List<ComponentSet> m_componentSets;



    //--- Private Variables ---//
    private List<GameObject> m_spawnedStructures;



    //--- Methods ---//
    public void GenerateStructures()
    {
        if (m_spawnedStructures != null)
            DeleteStructures();
        m_spawnedStructures = new List<GameObject>();

        SpawnNewBuildings();
    }

    public void DeleteStructures()
    {
        if (m_spawnedStructures == null)
            return;

        foreach (var obj in m_spawnedStructures)
            DestroyImmediate(obj);
    }

    public void SaveIndividualStructure(int _idx, bool _showDialog = true)
    {
        GameObject houseObj = m_spawnedStructures[_idx];

        // Get a unique file path based on the object's name (ensures it does not overwrite existing prefabs with the same name)
        string path = m_saveLocation;
        string filename = houseObj.name + ".prefab";
        string fullFilePath = path + filename;
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(fullFilePath);

        // Save to a prefab file and ensure the object in the scene becomes an instance
        PrefabUtility.SaveAsPrefabAssetAndConnect(houseObj, uniquePath, InteractionMode.AutomatedAction);

        if (_showDialog)
            EditorUtility.DisplayDialog("Save Succesful For [" + filename + "]", "The object has been succesfully saved as a prefab at [" + fullFilePath + "]", "OK");
    }

    public void SaveAllStructures()
    {
        for (int i = 0; i < m_spawnedStructures.Count; i++)
            SaveIndividualStructure(i, false);

        // Show a dialog for all of them at once, instead of one at a time
        EditorUtility.DisplayDialog("Save Succesful For All [" + m_spawnedStructures.Count + "] Objects", "The objects have been succesfully saved as individual prefabs", "OK");
    }

    public int WrapIdx(int _idx)
    {
        if (_idx < 0)
            _idx = m_spawnedStructures.Count - 1;
        else if (_idx >= m_spawnedStructures.Count)
            _idx = 0;

        return _idx;
    }



    //--- Utility Methods ---//
    private void SpawnNewBuildings()
    {
        Vector3 currentSpawnPos = this.transform.position;

        // Compile all of the objects together into a single 2D list so that the combination function can run on them
        List<List<GameObject>> allObjectsIndividual = new List<List<GameObject>>();
        foreach (var set in m_componentSets)
            allObjectsIndividual.Add(set.m_setObjs);

        var allCombinations = GetAllCombinations(allObjectsIndividual);

        InstantiateCombinations(allCombinations);
    }

    private void InstantiateCombinations(List<List<GameObject>> _combinations)
    {
        int spawnIndex = 0;
       
        // Spawn all of the combinations with each of the different materials on them
        // So if there are 10 component combinations and 2 materials, there will be 20 spawned objects
        foreach(var material in m_materials)
        {
            foreach (var combo in _combinations)
            {
                // Make grid
                float offsetX = m_spawnOffsets.x * (spawnIndex % m_gridRowLength);
                float offsetZ = m_spawnOffsets.y * (spawnIndex / m_gridRowLength);
                Vector3 spawnPos = this.transform.position + new Vector3(offsetX, 0.0f, offsetZ);

                // Spawn parent under this generator
                GameObject parentObj = new GameObject(m_namePrefixStr + "_" + spawnIndex);
                Transform parentTransform = parentObj.transform;
                parentTransform.parent = this.transform;
                parentTransform.position = spawnPos;
                m_spawnedStructures.Add(parentObj);

                // Spawn all components as children
                foreach (var component in combo)
                {
                    var newObj = Instantiate(component, spawnPos, Quaternion.identity, parentTransform);
                    newObj.GetComponentInChildren<Renderer>().material = material;
                }

                spawnIndex++;
            }
        }
    }

    // Adapted from Java code found here: https://stackoverflow.com/questions/17192796/generate-all-combinations-from-multiple-lists
    // Answer by Debosmit Ray
    // Finds all of the possible combinations of every object in a 2D list
    private List<List<GameObject>> GetAllCombinations(List<List<GameObject>> lists)
    {
        List<List<GameObject>> combinations = new List<List<GameObject>>();
        List<List<GameObject>> newCombinations;

        int index = 0;

        // extract each of the integers in the first list
        // and add each to ints as a new list
        foreach (GameObject i in lists[0])
        {
            List<GameObject> newList = new List<GameObject>();
            newList.Add(i);
            combinations.Add(newList);
        }
        index++;
        while (index < lists.Count)
        {
            List<GameObject> nextList = lists[index];
            newCombinations = new List<List<GameObject>>();
            foreach (List<GameObject> first in combinations)
            {
                foreach (GameObject second in nextList)
                {
                    List<GameObject> newList = new List<GameObject>();
                    newList.AddRange(first);
                    newList.Add(second);
                    newCombinations.Add(newList);
                }
            }
            combinations = newCombinations;

            index++;
        }

        return combinations;
    }



    //--- Getters ---//
    public GameObject GetSpawnedChild(int _idx)
    {
        if (m_spawnedStructures == null || m_spawnedStructures.Count == 0)
            return null;

        // Ensure the index is within a valid range
        _idx = WrapIdx(_idx);

        return m_spawnedStructures[_idx];
    }

    public GameObject[] GetAllSpawnedChildren()
    {
        return m_spawnedStructures.ToArray();
    }

    public bool GetHasSpawnedChildren()
    {
        return (m_spawnedStructures != null && m_spawnedStructures.Count > 0);
    }
}
