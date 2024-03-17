using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("End", "Flow Nodes/End")]
    public class EndNode : CodeGraphNode
    {
        public EndNode()
        {
            outputs.Clear();      
            nodeColor = Color.red;
        }
        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            GameManager.Instance.ProvideStoryManager().FinishGame();
            return string.Empty;
        }
    }

}
