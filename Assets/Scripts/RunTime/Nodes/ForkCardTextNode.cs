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


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            if (this.card != null)
            {
                StoryCardTemplate copiedcard = Object.Instantiate(card);
                copiedcard.Background = CardText;
                copiedcard.LeftText = this.LeftText;
                copiedcard.RightText = this.RightText;
                stepInfo = new StoryStep(card);
            }
            else
            {
                stepInfo = new StoryStep(null); 
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
