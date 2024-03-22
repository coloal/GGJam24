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

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            GameManager.Instance.ProvidePartyManager().AddMemberToParty(RecruitedCard);
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, 0);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}
