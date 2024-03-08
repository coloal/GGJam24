using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [System.Serializable]
    public class CodeGraphNode 
    {
        [SerializeField]
        private string guid;
        [SerializeField]
        private Rect position;

        public string typeName;

        public string id => guid;
        public Rect Position => position;

        
        public CodeGraphNode()
        {
            newGuid();
        }

        private void newGuid()
        {
            guid = Guid.NewGuid().ToString();
        }

        public void SetPosition(Rect position)
        {
            this.position = position;
        }

        public virtual string OnProcess(CodeGraphAsset graphAsset)
        {
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(guid, 0);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}
