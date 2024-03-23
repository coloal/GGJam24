using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Fork Card", "Card Nodes/Fork Card", color: "#80A4ED")]
    public class ForkCardNode : CodeGraphNode
    {
        [ExposedProperty()]
        public StoryCardTemplate card;

        public ForkCardNode()
        {
            outputs.Clear();
            outputs.Add("Left");
            outputs.Add("Right");
            
        }


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new StoryStep(card);
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
