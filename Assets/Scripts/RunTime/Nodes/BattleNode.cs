using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Battle", "Battle Nodes/Battle", color: "#632B30")]
    public class BattleNode : CodeGraphNode
    {
        [ExposedProperty()]
        public EnemyTemplate enemy;

        [ExposedProperty()]
        public bool IsBossFight;


        public BattleNode()
        {
            outputs.Clear();
            outputs.Add("Win Capture");
            outputs.Add("Game Over");
        }


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new CombatStep(enemy, IsBossFight);
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            int port;
            switch (turnResult)
            {
                case TurnResult.COMBAT_WON_CAPTURE: 
                    port = 0;
                    break;
                case TurnResult.COMBAT_GAME_OVER:
                    port = 1;
                    break;
                default:
                    Debug.LogError("Wrong info was passed to combat node");
                    port = 1; 
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


    
