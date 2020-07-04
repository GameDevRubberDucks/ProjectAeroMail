using UnityEngine;
using UnityEditor;

// Custom inspector for the HouseGenerator_Controller object
[CustomEditor(typeof(HouseGenerator_Controller))]
public class HouseGenerator_Editor : Editor
{
    //--- Private Variables ---//
    private HouseGenerator_Controller m_targetScript;
    private GameObject m_previewObject;
    private Editor m_previewEditor;
    private int m_previewIndex = 0;



    //--- Unity Methods ---//
    public override void OnInspectorGUI()
    {
        if (m_targetScript == null)
            m_targetScript = (HouseGenerator_Controller)target;

        // Use the original inspector for all of the variables that come with the original class
        DrawDefaultInspector();
        GUILayout.Space(10.0f);

        // Generation UI Elements
        EditorGUILayout.LabelField("Structure Generation", EditorStyles.boldLabel);

        if (GUILayout.Button("Generate Structures"))
            m_targetScript.GenerateStructures();

        EditorGUI.BeginDisabledGroup(!m_targetScript.GetHasSpawnedChildren());
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Destroy Generated Structures"))
                {
                    m_targetScript.DeleteStructures();

                    // Reset the preview index for next time we generate
                    m_previewIndex = 0;
                }

                if (GUILayout.Button("Save Generated Structures"))
                    m_targetScript.SaveAllStructures();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUI.EndDisabledGroup();
        
        // Preview UI Elements
        GUILayout.Space(10.0f);
        EditorGUILayout.LabelField("Structure Preview", EditorStyles.boldLabel);

        string labelContents = (m_previewObject != null) ? "Previewing [" + m_previewObject.name + "]" : "No Preview Object";
        EditorGUILayout.LabelField(labelContents);

        EditorGUI.BeginDisabledGroup(m_previewObject == null);
        {
            EditorGUILayout.BeginHorizontal();
            {
                // Switch the previewed object
                if (GUILayout.Button("< Prev"))
                {
                    m_previewIndex--;

                    m_previewIndex = m_targetScript.WrapIdx(m_previewIndex);
                }
                else if (GUILayout.Button("Next >"))
                {
                    m_previewIndex++;

                    m_previewIndex = m_targetScript.WrapIdx(m_previewIndex);
                }
            }
            EditorGUILayout.EndHorizontal();

            // Save the previewed object
            string saveBtnLbl = (m_previewObject != null) ? "Save [" + m_previewObject.name + "] Individually" : "Generate Structures to Save One";
            if (GUILayout.Button(saveBtnLbl))
                m_targetScript.SaveIndividualStructure(m_previewIndex, true);
        }
        EditorGUI.EndDisabledGroup();

        // Create or destroy the editor as needed
        if (m_previewObject == null)
        {
            // No object, no editor
            if (m_previewEditor)
                DestroyImmediate(m_previewEditor);
        }

        // New object is set to be previewed so update the window
        if (m_previewObject != m_targetScript.GetSpawnedChild(m_previewIndex))
        {
            m_previewObject = m_targetScript.GetSpawnedChild(m_previewIndex);

            if (m_previewEditor)
                DestroyImmediate(m_previewEditor);

            m_previewEditor = Editor.CreateEditor(m_previewObject);
        }

        // Show the window if needed
        if (m_previewEditor != null)
        {
            m_previewEditor.DrawHeader();
            m_previewEditor.OnPreviewGUI(GUILayoutUtility.GetRect(300.0f, 300.0f), EditorStyles.whiteLabel);
        }
    }

    private void OnDisable()
    {
        // Cleanup the preview editor
        if (m_previewEditor)
            DestroyImmediate(m_previewEditor);
    }
}
