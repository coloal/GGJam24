using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeInfo("Fork Card", "Card Nodes/Fork Card")]
public class ForkCardNode : CodeGraphNode
{
    [ExposedProperty()]
    public CardTemplate card;
    
    public ForkCardNode()
    {
        outputs.Clear();
        outputs.Add("Left");
        outputs.Add("Right");
    }

    public override bool GetNodeCard(out CardTemplate card)
    {
        card = this.card;
        return true;
    }

    public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
    {
        int port = bSwipedLeft ? 0 : 1;
        CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
        if (nextNode != null)
        {
            return nextNode.id;
        }
        return string.Empty;
    }
}
