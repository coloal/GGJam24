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
            nodeColor = new Color(01.0f / 255.0f, 41.0f / 255.0f, 95.0f / 255.0f);
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

