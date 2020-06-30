using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentSet
{
    [SerializeField] public List<GameObject> m_setObjs;

    public ComponentSet()
    {
        m_setObjs = new List<GameObject>();
    }
}

public class HouseGenerator : MonoBehaviour
{
    public Vector2 m_spawnOffsets = new Vector2(5.0f, 5.0f);
    public int m_gridRowLength = 10;
    public string m_namePrefixStr = "house";
    public List<ComponentSet> m_componentSets;
    public Material[] m_materials;

    private List<GameObject> m_spawnedChildren;

    public void GenerateStructures()
    {
        // Clear existing structures
        if (m_spawnedChildren != null)
            DeleteChildren();
        m_spawnedChildren = new List<GameObject>();

        SpawnNewBuildings();
    }

    public void DeleteChildren()
    {
        if (m_spawnedChildren == null)
            return;

        foreach (var obj in m_spawnedChildren)
            DestroyImmediate(obj);
    }

    private void SpawnNewBuildings()
    {
        Vector3 currentSpawnPos = this.transform.position;

        List<List<GameObject>> allObjectsIndividual = new List<List<GameObject>>();
        foreach (var set in m_componentSets)
            allObjectsIndividual.Add(set.m_setObjs);

        var allCombinations = GetAllCombinations(allObjectsIndividual);

        InstantiateCombinations(allCombinations);
    }

    private void InstantiateCombinations(List<List<GameObject>> _combinations)
    {
        int spawnIndex = 0;
       
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
                m_spawnedChildren.Add(parentObj);

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
    public List<List<GameObject>> GetAllCombinations(List<List<GameObject>> lists)
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
}
