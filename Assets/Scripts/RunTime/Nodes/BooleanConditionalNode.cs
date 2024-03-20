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
        public BrainTag Condition;

        

        public BooleanConditionalNode()
        {
            outputs.Clear();
            outputs.Add("True");
            outputs.Add("False");
            nodeColor = new Color(73.0f / 255.0f, 220.0f / 255.0f, 177.0f / 255.0f); 
        }

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {

            int port = GameManager.Instance.ProvideBrainManager().GetTag(Condition) ? 0 : 1;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }

}

