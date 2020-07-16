using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NVIDIA.Flex;
using System.Runtime.CompilerServices;

[CanEditMultipleObjects]
[CustomEditor(typeof(LeavesActor))]
public class LeavesActorEditor : FlexActorEditor
{
    SerializedProperty m_asset;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_asset = serializedObject.FindProperty("m_asset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.ContainerUI();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(m_asset, new GUIContent("Cloth Asset"));
        if (EditorGUI.EndChangeCheck()) m_recreateActor.intValue = 2;

        EditorGUILayout.Separator();

        base.ParticlesUI();

        EditorGUILayout.Separator();

        base.DebugUI();

        if (GUI.changed) serializedObject.ApplyModifiedProperties();
    }
}