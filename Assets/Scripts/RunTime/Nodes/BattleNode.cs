using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Battle", "Battle Nodes/Battle", color: "#632B30")]
    public class BattleNode : CodeGraphNode
    {
        [ExposedProperty()]
        public EnemyTemplate card;

        [ExposedProperty()]
        public bool IsBossFight;


        public BattleNode()
        {
            outputs.Clear();
            outputs.Add("Win Capture");
            outputs.Add("Win no Capture");
            outputs.Add("Lose Combat");
            outputs.Add("Game Over");
        }


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new CombatStep(card, IsBossFight);
            //TODO quitar esto
            GameManager.Instance.ProvideBrainManager().bIsBossFight = IsBossFight;
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
                case TurnResult.COMBAT_WON_NO_CAPTURE:
                    port = 1;
                    break;
                case TurnResult.COMBAT_LOST: 
                    port = 2;
                    break;
                case TurnResult.COMBAT_GAME_OVER:
                    port = 3;
                    break;
                default:
                    Debug.LogError("Wrong info was passed to combat node");
                    port = 3; 
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


    
