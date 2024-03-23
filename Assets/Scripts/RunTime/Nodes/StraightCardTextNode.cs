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


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            if(this.card != null)
            {
                StoryCardTemplate cardcopy = Object.Instantiate(this.card);
                cardcopy.Background = CardText;
                cardcopy.LeftText = this.LeftText;
                cardcopy.RightText = this.RightText;
                stepInfo = new StoryStep(cardcopy);
            }
            else
            {
                stepInfo = new StoryStep(null);
            }
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
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


