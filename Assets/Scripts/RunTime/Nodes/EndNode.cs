using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("End", "Flow Nodes/End", color: "#550C18")]
    public class EndNode : CodeGraphNode
    {
        public EndNode()
        {
            outputs.Clear();      
        }
    
        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new EndStep();
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            return string.Empty;
        }
    }

}
