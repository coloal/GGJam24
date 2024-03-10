using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Bool Condition", "Condition Nodes/Bool Condition")] 
    public class BooleanConditionalNode : CodeGraphNode
    {
        [ExposedProperty()]
        public Tag IF;

        

        public BooleanConditionalNode()
        {
            outputs.Clear();
            outputs.Add("True");
            outputs.Add("False");
        }

        public override bool GetNodeCard(out CardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {

            int port = GameManager.Instance.ProvideBrainManager().GetTag(IF) ? 0 : 1;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }

}

