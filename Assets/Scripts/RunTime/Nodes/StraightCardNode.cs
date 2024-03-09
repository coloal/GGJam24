using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

[NodeInfo("Straight Card", "Card Nodes/Straight Card Node")]
public class StraightCardNode : CodeGraphNode
{
    [ExposedProperty()]
    public CardTemplate card;

    

    public override bool GetNodeCard(out CardTemplate card)
    {
        card = this.card;
        return true;
    }

    public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
    {
        CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, 0);
        if (nextNode != null)
        {
            return nextNode.id;
        }
        return string.Empty;
    }

}
