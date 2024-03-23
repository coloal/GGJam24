using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CodeGraph
{
    [NodeInfo("Start", "Flow Nodes/Start", false, color: "#2A9134")]
    public class StartNode : CodeGraphNode
    {

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            return base.OnNextNode(graphAsset, bSwipedLeft);
        }
    }
}
