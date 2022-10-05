using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;


[CustomEditor(typeof(BetterToggle), true)]
[CanEditMultipleObjects]
public class BetterToggleEditor : ToggleEditor
{
    SerializedProperty m_onValueChangedReverse;
    protected override void OnEnable()
    {
        base.OnEnable();
        m_onValueChangedReverse = serializedObject.FindProperty("onValueChangedReverse");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // Draw the event notification options
        EditorGUILayout.PropertyField(m_onValueChangedReverse);

        serializedObject.ApplyModifiedProperties();
    }
}
