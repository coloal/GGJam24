using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Straight Card Text", "Card Nodes/Straight Card Text", color: "#80A4ED")]
    public class StraightCardTextNode : CodeGraphNode
    {

        [ExposedProperty()]
        [TextArea(3, 3)]
        public string CardText;

        [ExposedProperty()]
        [TextArea(1, 1)]
        public string LeftText;

        [ExposedProperty()]
        [TextArea(1, 1)]
        public string RightText;

        [ExposedProperty()]
        public StoryCardTemplate card;


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            if(this.card != null)
            {
                StoryCardTemplate cardcopy = Object.Instantiate(this.card);
                cardcopy.Text = CardText.Replace("\n", " ").Replace("///", "\n"); 
                cardcopy.LeftText = LeftText.Replace("\n", " ").Replace("///", "\n"); 
                cardcopy.RightText = RightText.Replace("\n", " ").Replace("///", "\n"); 
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


