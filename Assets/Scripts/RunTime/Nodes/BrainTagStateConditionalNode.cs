
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
            nodeColor = new Color(73.0f / 255.0f, 220.0f / 255.0f, 177.0f / 255.0f);
        }

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            int port = GameManager.Instance.ProvideBrainManager().IsState(Picker.selectedTag, Picker.selectedTagState) ? 0 : 1;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}

