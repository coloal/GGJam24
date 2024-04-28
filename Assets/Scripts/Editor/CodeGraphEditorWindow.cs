using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace CodeGraph.Editor
{
    public class CodeGraphEditorWindow : EditorWindow
    {
        [SerializeField]
        private CodeGraphAsset currentGraph;
        [SerializeField]
        private CodeGraphView currentView;
        [SerializeField]
        private SerializedObject serializedObject;
        public CodeGraphAsset CurrentGraph => currentGraph;

        public static void Open(CodeGraphAsset target)
        {
            CodeGraphEditorWindow[] windows = Resources.FindObjectsOfTypeAll<CodeGraphEditorWindow>();
            foreach (CodeGraphEditorWindow window in windows)
            {
                if(window.currentGraph == target)
                {
                    window.Focus();
                    return;
                }
            }
            CodeGraphEditorWindow newWindow = CreateWindow<CodeGraphEditorWindow>(typeof(CodeGraphEditorWindow), typeof(SceneView));
            newWindow.titleContent = new GUIContent($"{target.name}", EditorGUIUtility.ObjectContent(null, typeof(CodeGraphAsset)).image);
            newWindow.Load(target);
        }

        private void OnEnable()
        {
            if(currentGraph != null)
            {
                DrawGraph();
            }
        }

        private void OnGUI()
        {
            if(currentGraph != null)
            {
                if(EditorUtility.IsDirty(currentGraph))
                {
                    hasUnsavedChanges = true;
                }
                else
                {
                    hasUnsavedChanges = false;
                }
            }
        }

        private void Load(CodeGraphAsset target)
        {
            currentGraph = target;
            DrawGraph();
        }

        private void DrawGraph()
        {
            serializedObject = new SerializedObject(currentGraph);
            currentView = new CodeGraphView(serializedObject, this);
            currentView.graphViewChanged += OnChange;
            rootVisualElement.Add(currentView);
        }

        private GraphViewChange OnChange(GraphViewChange graphViewChange)
        {
            EditorUtility.SetDirty(currentGraph);
            return graphViewChange;
        }
    }

}

