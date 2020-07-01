using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HouseGenerator))]
public class HouseGenerator_Editor : Editor
{
    SerializedObject sObj;
    SerializedProperty sListProp;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Create Buildings"))
        {
            HouseGenerator targetScript = (HouseGenerator)target;
            targetScript.GenerateStructures();
        }

        if (GUILayout.Button("Destroy Buildings"))
        {
            HouseGenerator targetScript = (HouseGenerator)target;
            targetScript.DeleteChildren();
        }

        if (GUILayout.Button("Save Generated Buildings"))
        {
            HouseGenerator targetScript = (HouseGenerator)target;
            targetScript.SaveStructures();
        }

        //if (sObj == null)
        //{
        //    sObj = new SerializedObject(target);
        //}

        //if (sListProp == null)
        //{
        //    sListProp = sObj.FindProperty("m_componentSets");

        //    if (sListProp == null)
        //    {
        //        sListProp = sObj.FindProperty("m_componentSets");

        //        if (sListProp == null)
        //        {
        //            Debug.Log("Cannot find the component list");
        //        }
        //    }   
        //}
        //else
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    {
        //        EditorGUILayout.LabelField("Component Sets [" + sListProp.arraySize + "]");
                
        //        if (GUILayout.Button("Add New Set"))
        //        { 
        //            sListProp.arraySize++;
        //        }
        //    }
        //    EditorGUILayout.EndHorizontal();
        //    EditorGUILayout.Space();

        //    for (int i = 0; i < sListProp.arraySize; i++)
        //    {
        //        //var subObj = sListProp.GetArrayElementAtIndex(i).Find
        //        //var subProp = subObj.FindProperty("m_setObjs");
                
        //        EditorGUILayout.LabelField("Set " + i.ToString());

        //        EditorGUILayout.PropertyField(sListProp.GetArrayElementAtIndex(i).FindPropertyRelative("m_setObjs"));
        //        //EditorGUILayout.PropertyField(subProp);
        //    }
        //}

        //sObj.ApplyModifiedProperties();
    }
}
