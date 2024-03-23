using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Battle", "Battle Nodes/Battle", color: "#632B30")]
    public class BattleNode : CodeGraphNode
    {
        [ExposedProperty()]
        public CombatCardTemplate card;

        public BattleNode()
        {
            outputs.Clear();
            outputs.Add("Win");
            outputs.Add("Lose");
            
        }


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new CombatStep(card);
            return true;
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


    
