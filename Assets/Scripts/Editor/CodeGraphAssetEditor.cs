using System.Collections;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;


namespace CodeGraph.Editor
{
    [CustomEditor(typeof(CodeGraphAsset))]
    public class CodeGraphAssetEditor : UnityEditor.Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int intasceId, int index)
        {
            Object asset = EditorUtility.InstanceIDToObject(intasceId);
            if(asset.GetType() == typeof(CodeGraphAsset))
            {
                CodeGraphEditorWindow.Open((CodeGraphAsset)asset);
                return true;
            }
            return false;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open"))
            {
                CodeGraphEditorWindow.Open((CodeGraphAsset)target);
            }
        }
    }
}


