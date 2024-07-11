using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Menu", "Flow Nodes/Menu", color: "#550C18")]
    public class MenuNode : CodeGraphNode
    {
        public MenuNode()
        {
            outputs.Clear();      
        }
    
        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new MenuStep();
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            return string.Empty;
        }
    }

}
