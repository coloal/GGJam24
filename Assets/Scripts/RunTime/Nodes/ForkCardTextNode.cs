using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Fork Card Text", "Card Nodes/Fork Card Text", color: "#80A4ED")]
    public class ForkCardTextNode : CodeGraphNode
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
                copiedcard.Background = CardText.Replace("\n", " ").Replace("///", "\n");
                copiedcard.LeftText = LeftText.Replace("\n", " ").Replace("///", "\n"); 
                copiedcard.RightText = RightText.Replace("\n", " ").Replace("///", "\n"));
                stepInfo = new StoryStep(copiedcard);
            }
            else
            {
                stepInfo = new StoryStep(null); 
            }
            return true;
            
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            int port;
            switch (turnResult)
            {
                case TurnResult.SWIPED_LEFT:
                    port = 0;
                    break;
                case TurnResult.SWIPED_RIGHT:
                    port = 1;
                    break;
                default:
                    Debug.LogError("Wrong result info was passed into Fork Card Node");
                    port = 0;
                    break;

            }
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }

}
