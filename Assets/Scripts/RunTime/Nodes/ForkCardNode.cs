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


        public override bool GetNodeCard(out StoryCardTemplate card)
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

}
