using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Fork Card Text", "Card Nodes/Fork Card Text", color: "#80A4ED")]
    public class ForkCardTextNode : CodeGraphNode
    {

        [ExposedProperty()]
        public string CardText;

        [ExposedProperty()]
        public string LeftText;

        [ExposedProperty()]
        public string RightText;

        [ExposedProperty()]
        public StoryCardTemplate card;


        public ForkCardTextNode()
        {
            outputs.Clear();
            outputs.Add("Left");
            outputs.Add("Right");
            
        }


        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            if (this.card != null)
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
            int port = bSwipedLeft ? 0 : 1;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }

}
