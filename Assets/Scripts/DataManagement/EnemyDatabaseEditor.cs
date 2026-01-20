using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(EnemyDatabase))]
public class EnemyDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 1. Draw the default list so you can still edit your enemies
        DrawDefaultInspector();

        // Add some spacing
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Export Tools", EditorStyles.boldLabel);

        // 2. Create the Button
        // The button returns true only in the frame it is clicked
        if (GUILayout.Button("Save Database to JSON", GUILayout.Height(30)))
        {
            EnemyDatabase db = (EnemyDatabase)target;
            db.SaveToJson();
        }
    }
}
