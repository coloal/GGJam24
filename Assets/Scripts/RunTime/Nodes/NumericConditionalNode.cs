using CodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Numeric Condition", "Condition Nodes/Numeric Condition")]
    public class NumericConditionalNode : CodeGraphNode
    {
        [ExposedProperty()]
        public NumericTags ComparedTag;

        [ExposedProperty()]
        public NumericConditions ConditionType;

        [ExposedProperty()]
        public int ComparedNumber;



        public NumericConditionalNode()
        {
            outputs.Clear();
            outputs.Add("True");
            outputs.Add("False");
            nodeColor = new Color(73.0f / 255.0f, 220.0f / 255.0f, 177.0f / 255.0f);
        }

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            int value = GameManager.Instance.ProvideBrainManager().GetNumericTag(ComparedTag);
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

    [Serializable]
    public enum NumericConditions
    {
        GREATER,
        GREATER_EQUAL,
        EQUAL,
        LESS,
        LESS_EQUAL
    }
}
