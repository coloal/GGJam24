using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [CreateAssetMenu(menuName = "Code Graph/New Graph")]
    public class CodeGraphAsset: ScriptableObject
    {
        [SerializeReference]
        private List<CodeGraphNode> nodes;

        public List<CodeGraphNode> Nodes => nodes;

        public CodeGraphAsset() 
        { 
            nodes = new List<CodeGraphNode>();
        }
    }
}

