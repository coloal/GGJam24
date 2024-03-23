using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Return to Parent Story", "Story Nodes/Return to Parent Story", color: "#01295F")]
    public class ReturnToParentStoryNode : CodeGraphNode
    {
        
        public ReturnToParentStoryNode() {
            outputs.Clear();
        }

        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = GameManager.Instance.ProvideStoryManager().ReturntoParentStory().ExecuteGraphStep();
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            return string.Empty;
        }
    }

}

