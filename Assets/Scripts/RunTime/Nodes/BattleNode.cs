using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Battle", "Battle Nodes/Battle")]
    public class BattleNode : CodeGraphNode
    {
        [ExposedProperty()]
        public CombatCardTemplate card;

        public BattleNode()
        {
            outputs.Clear();
            outputs.Add("Win");
            outputs.Add("Lose");
            nodeColor = new Color(99.0f/255.0f, 43.0f/255.0f, 48.0f/255.0f); 
        }


        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            //TODO iniciar batalla
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


    
