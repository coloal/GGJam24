using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Restart", "Flow Nodes/Restart", color: "#550C18")]
    public class RestartNode : CodeGraphNode
    {
        public RestartNode()
        {
            outputs.Clear();      
        }
    
        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new RestartStep();
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            return string.Empty;
        }
    }

}
