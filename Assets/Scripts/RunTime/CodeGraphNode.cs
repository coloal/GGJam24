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

        public List<string> outputs = new List<string>();

        public string id => guid;
        public Rect Position => position;

        
        public CodeGraphNode()
        {
            newGuid();
            outputs.Add("Next");
        }

        private void newGuid()
        {
            guid = Guid.NewGuid().ToString();
        }

        public void SetPosition(Rect position)
        {
            this.position = position;
        }
        public virtual bool GetNodeCard(out CardTemplate card)
        {
            card = null;
            return false;
        }
        public virtual string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
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
