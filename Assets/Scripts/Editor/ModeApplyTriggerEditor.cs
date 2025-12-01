using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(GameModeManager))]
public class ModeApplyTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        GameModeManager manager = target as GameModeManager;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Apply", GUILayout.Width(120), GUILayout.Height(30)))
        {
            //manager.Apply();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}