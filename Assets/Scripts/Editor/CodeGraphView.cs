using CodeGraph;
using CodeGraph.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeGraph.Editor
{
    public class CodeGraphView : GraphView
    {

        private CodeGraphAsset codeGraph;
        private SerializedObject serializedObject;
        private CodeGraphEditorWindow window;
        private CodeGraphWindowSearchProvider searchProvider;


        public List<CodeGraphEditorNode> graphNodes;
        public Dictionary<string, CodeGraphEditorNode> nodeDictionary;

        public CodeGraphEditorWindow Window => window;
        public CodeGraphView(SerializedObject serializedObject, CodeGraphEditorWindow window)
        {

            graphNodes = new List<CodeGraphEditorNode>();
            nodeDictionary = new Dictionary<string, CodeGraphEditorNode>();
            searchProvider = ScriptableObject.CreateInstance<CodeGraphWindowSearchProvider>();
            searchProvider.graph = this;
            nodeCreationRequest = ShowSearchWindow;
            StyleSheet style = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/USS/CodeGraphEditor.uss");
            styleSheets.Add(style);
            this.serializedObject = serializedObject;
            codeGraph = (CodeGraphAsset)serializedObject.targetObject;
            this.window = window;
            GridBackground background = new GridBackground();
            background.name = "Grid";
            Add(background);
            background.SendToBack();

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ClickSelector());

            DrawNodes();
            graphViewChanged += OnGpraphViewChangedEvent;
        }


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> allPorts = new List<Port>();
            List<Port> ports = new List<Port>();
            
            foreach(CodeGraphEditorNode node in graphNodes)
            {
                allPorts.AddRange(node.Ports); 
            }

            foreach (Port p in allPorts)
            {
                if(p == startPort) { continue; }
                if(p.node == startPort.node) { continue; }
                if(p.direction == startPort.direction) { continue; }
                if(p.portType == startPort.portType) 
                { 
                    ports.Add(p); 
                }
            }

            return ports;
        }

        private GraphViewChange OnGpraphViewChangedEvent(GraphViewChange graphViewChange)
        {
            if(graphViewChange.movedElements != null)
            {
                Undo.RecordObject(serializedObject.targetObject, "Moved Elements");
                foreach(CodeGraphEditorNode node in graphViewChange.movedElements.OfType<CodeGraphEditorNode>())
                {
                    node.SavePosition();
                }
            }
            if(graphViewChange.elementsToRemove != null)
            {
                List<CodeGraphEditorNode> nodes = graphViewChange.elementsToRemove.OfType<CodeGraphEditorNode>().ToList();
                if(nodes.Count > 0)
                {
                    Undo.RecordObject(serializedObject.targetObject, "Removed Node");
                    for(int i = nodes.Count - 1; i >= 0; i--)
                    {
                        RemoveNode(nodes[i]);
                    }
                }
                
            }
            return graphViewChange;
        }

        private void RemoveNode(CodeGraphEditorNode codeGraphEditorNode)
        {
            
            codeGraph.Nodes.Remove(codeGraphEditorNode.GraphNode);
            nodeDictionary.Remove(codeGraphEditorNode.GraphNode.id);
            graphNodes.Remove(codeGraphEditorNode);
            serializedObject.Update();
        }

        private void DrawNodes()
        {
            foreach(CodeGraphNode node in codeGraph.Nodes)
            {
                AddNodeToGraph(node);
            }
        }

        private void ShowSearchWindow(NodeCreationContext context)
        {
            searchProvider.target = (VisualElement)focusController.focusedElement;
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchProvider);
        }

        public void Add(CodeGraphNode node)
        {
            Undo.RecordObject(serializedObject.targetObject, "Added Node");

            codeGraph.Nodes.Add(node);
            
            serializedObject.Update();
            AddNodeToGraph(node);
        }

        private void AddNodeToGraph(CodeGraphNode node)
        {
            node.typeName = node.GetType().AssemblyQualifiedName;
            CodeGraphEditorNode editorNode = new CodeGraphEditorNode(node);
            editorNode.SetPosition(node.Position);
            graphNodes.Add(editorNode);
            nodeDictionary.Add(node.id, editorNode);
            AddElement(editorNode);
        }
    }

}
