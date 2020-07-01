using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HouseGenerator))]
public class HouseGenerator_Editor : Editor
{
    SerializedObject sObj;
    SerializedProperty sListProp;

    GameObject previewObject;
    Editor previewEditor;

    int previewIndex = 0;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(50.0f);

        HouseGenerator targetScript = (HouseGenerator)target;

        if (GUILayout.Button("Create Buildings"))
        {
            targetScript.GenerateStructures();
        }

        if (GUILayout.Button("Destroy Buildings"))
        {
            targetScript.DeleteChildren();
        }

        if (GUILayout.Button("Save Generated Buildings"))
        {
            targetScript.SaveAllStructures();
        }

        GUILayout.Space(50.0f);

        string labelContents = (previewObject != null) ? "Previewing [" + previewObject.name + "]" : "No Preview Object";
        EditorGUILayout.LabelField(labelContents);
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("< Prev"))
            {
                previewIndex--;

                previewIndex = targetScript.WrapIdx(previewIndex);
            }
            else if (GUILayout.Button("Next >"))
            {
                previewIndex++;

                previewIndex = targetScript.WrapIdx(previewIndex);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (previewObject != null)
        {
            if (GUILayout.Button("Save [" + previewObject.name + "] Individually"))
            {
                targetScript.SaveIndividualStructure(previewIndex, true);
            }
        }
        else
        {
            // No object, no editor
            if (previewEditor)
                DestroyImmediate(previewEditor);
        }
        
        if (previewObject != targetScript.GetSpawnedChild(previewIndex))
        {
            previewObject = targetScript.GetSpawnedChild(previewIndex);

            if (previewEditor)
                DestroyImmediate(previewEditor);

            previewEditor = Editor.CreateEditor(previewObject);
        }
        if (previewEditor != null)
        {
            previewEditor.DrawHeader();
            previewEditor.OnPreviewGUI(GUILayoutUtility.GetRect(500.0f, 500.0f), EditorStyles.whiteLabel);
        }
    }

    private void OnDisable()
    {
        if (previewEditor)
            DestroyImmediate(previewEditor);
    }
}
