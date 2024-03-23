using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Check for Party Member", "Condition Nodes/Party Conditions/Check for Party Member", color: "#06B184")]
    public class CheckPartyMemberNode : CodeGraphNode
{
        [ExposedProperty()]
        [Tooltip("Carta a revisar")]
        public CombatCardTemplate PartyMember;

        public CheckPartyMemberNode()
        {
            outputs.Clear();
            outputs.Add("True");
            outputs.Add("False");
        }

   
        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            int port = GameManager.Instance.ProvidePartyManager().CheckPartyMember(PartyMember) ? 0: 1;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}
