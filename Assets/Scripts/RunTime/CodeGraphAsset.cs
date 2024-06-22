using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace CodeGraph
{
    [CreateAssetMenu(menuName = "Code Graph/New Graph")]
    public class CodeGraphAsset: ScriptableObject
    {
        [SerializeReference]
        private List<CodeGraphNode> nodes;
        [SerializeField]
        private List<CodeGraphConnection> connections;

        public Dictionary<string, CodeGraphNode> nodeDictionary;
        public List<CodeGraphNode> Nodes => nodes;
        public List <CodeGraphConnection> Connections => connections;


        public CodeGraphAsset() 
        {
            nodes = new List<CodeGraphNode>();
            connections = new List<CodeGraphConnection>();
        }

        public void Init()
        {
            nodeDictionary = new Dictionary<string, CodeGraphNode>();
            foreach (CodeGraphNode node in nodes)
            {
                nodeDictionary.Add(node.id, node);
            }
        }

        public CodeGraphNode GetStartNode()
        {
            
            StartNode[] startNodes = Nodes.OfType<StartNode>().ToArray();
            if(startNodes.Length > 0 )
            {
                return startNodes[0];
            }
            return null;
            
        }

        public CodeGraphNode GetNode(string nextNode)
        {
            if(nodeDictionary.TryGetValue(nextNode, out CodeGraphNode node))
            {
                return node;
            }
            return null;
        }

        public CodeGraphNode GetNodeConnected(string outputNodeId, int index)
        {
            foreach(CodeGraphConnection connection in connections)
            {
                if(connection.outputPort.nodeId == outputNodeId && connection.outputPort.portIndex == index)
                {
                    string nodeId = connection.inputPort.nodeId;
                    CodeGraphNode inputNode = nodeDictionary[nodeId];
                    return inputNode;
                }
            }
            return null;
        }
    }
}

