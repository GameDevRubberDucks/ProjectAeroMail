using UnityEngine;
using UnityEditor;

// Custom inspector for the CloudGenerator_Controller object
[CustomEditor(typeof(CloudGenerator_Controller))]
public class CloudGenerator_Editor : Editor
{
    //--- Private Variables ---//
    private CloudGenerator_Controller m_targetScript;



    //--- Unity Methods ---//
    public override void OnInspectorGUI()
    {
        // Grab the target script if needed
        if (!m_targetScript)
            m_targetScript = (CloudGenerator_Controller)target;

        // Show the original inspector for the controls
        DrawDefaultInspector();

        // Add a button to generate clouds
        if (GUILayout.Button("Generate Clouds"))
            m_targetScript.GenerateClouds();

        // Also optionally have a button to delete the clouds, assuming there are some
        EditorGUI.BeginDisabledGroup(!m_targetScript.CanDestroyClouds());
        {
            if (GUILayout.Button("Destroy Clouds"))
                m_targetScript.DeleteClouds();
        }
        EditorGUI.EndDisabledGroup();
    }
}