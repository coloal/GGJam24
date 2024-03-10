
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("State Condition", "Condition Nodes/State Condition")]
    public class BrainTagStateConditionalNode : CodeGraphNode
    {
        [ExposedProperty()]
        public BrainTagOptionPicker Picker;
        

        public BrainTagStateConditionalNode()
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
            
            int port = 0;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}

