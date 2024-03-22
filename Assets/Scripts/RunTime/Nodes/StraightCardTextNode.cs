using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Straight Card Text", "Card Nodes/Straight Card Text", color: "#80A4ED")]
    public class StraightCardTextNode : CodeGraphNode
    {

        [ExposedProperty()]
        public string CardText;

        [ExposedProperty()]
        public string LeftText;

        [ExposedProperty()]
        public string RightText;

        [ExposedProperty()]
        public StoryCardTemplate card;


        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            if(this.card != null)
            {
                card = Object.Instantiate(this.card);
                card.Background = CardText;
                card.LeftText = this.LeftText;
                card.RightText = this.RightText;
            }
            else
            {
                card = null;
            }
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
}


