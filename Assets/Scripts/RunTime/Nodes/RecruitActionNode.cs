using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

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
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, 0);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}
