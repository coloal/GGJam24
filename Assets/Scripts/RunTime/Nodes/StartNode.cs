using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CodeGraph
{
    [NodeInfo("Start", "Process/Start", false)]
    public class StartNode : CodeGraphNode
    {


        public override bool GetNodeCard(out CardTemplate card)
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
