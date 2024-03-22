using CodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Party Numeric Condition", "Condition Nodes/Party Conditions/Party Numeric Condition", color: "#06B184")]
    public class PartyNumericConditionNode : CodeGraphNode
    {
        
        [ExposedProperty()]
        public NumericConditions ConditionType;

        [ExposedProperty()]
        public int ComparedNumber;



        public PartyNumericConditionNode()
        {
            outputs.Clear();
            outputs.Add("True");
            outputs.Add("False");
        }

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            int value = GameManager.Instance.ProvidePartyManager().GetPartyCount();
            int port = 1;
            switch (ConditionType)
            {
                case NumericConditions.GREATER:
                    if (value > ComparedNumber) { port = 0; }
                    break;
                case NumericConditions.GREATER_EQUAL:
                    if (value >= ComparedNumber) { port = 0; }
                    break;
                case NumericConditions.EQUAL:
                    if (value == ComparedNumber) { port = 0; }
                    break;
                case NumericConditions.LESS:
                    if (value < ComparedNumber) { port = 0; }
                    break;
                case NumericConditions.LESS_EQUAL:
                    if (value <= ComparedNumber) { port = 0; }
                    break;
                default:
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
