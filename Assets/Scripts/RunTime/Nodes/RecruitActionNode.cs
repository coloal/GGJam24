using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Recruit Action", "Action Nodes/Recruit Action", color: "#F57600")]
    public class RecruitActionNode : CodeGraphNode
    {
        [ExposedProperty()]
        [Tooltip("Carta a reclutar")]
        public CombatCardTemplate RecruitedCard;


        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            GameManager.Instance.ProvidePartyManager().AddPartyMember(RecruitedCard);
            return base.OnNextNode(graphAsset, turnResult);
        }
    }
}
