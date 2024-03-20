using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CodeGraph
{
    [NodeInfo("Start", "Flow Nodes/Start", false, color: "#FFEAEE")]
    public class StartNode : CodeGraphNode
    {


        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            return base.OnNextNode(graphAsset, bSwipedLeft);
        }
    }
}
