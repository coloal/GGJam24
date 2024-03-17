using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Return to Parent Story", "Story Nodes/Return to Parent Story")]
    public class ReturnToParentStoryNode : CodeGraphNode
    {
        
        public ReturnToParentStoryNode() {
            outputs.Clear();
        }

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = GameManager.Instance.ProvideStoryManager().ReturntoParentStory().GetNextCard();
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            return string.Empty;
        }
    }

}

