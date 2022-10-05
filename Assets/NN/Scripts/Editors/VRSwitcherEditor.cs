using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CustomEditor(typeof(VRSwitcher))]
public class VRSwitcherEditor : Editor
{
    VRSwitcher switcher;
    private void OnEnable()
    {
        switcher = (VRSwitcher)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button(switcher.IsVRMode ? "VR" : "PC"))
        {
            switcher.IsVRMode = !switcher.IsVRMode;
        }
    }
}
