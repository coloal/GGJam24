using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public class RecruitActionNode : CodeGraphNode
{
    [NodeInfo("Recruit Action", "Action Nodes/Recruit Action", color: "#D17A22")]
    public class RandomNode : CodeGraphNode
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
