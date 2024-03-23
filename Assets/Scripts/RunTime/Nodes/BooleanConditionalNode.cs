using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Bool Condition", "Condition Nodes/Bool Condition", color: "#06B184")] 
    public class BooleanConditionalNode : CodeGraphNode
    {
        [ExposedProperty()]
        public BrainTag Condition;

        

        public BooleanConditionalNode()
        {
            outputs.Clear();
            outputs.Add("True");
            outputs.Add("False");
            
        }

      
        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
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

