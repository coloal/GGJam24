using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("End and Change Story", "Story Nodes/End Story And Change")]
    public class EndAndChangeStoryNode : CodeGraphNode
    {

        [ExposedProperty()]
        public CodeGraphAsset newStory;
        [ExposedProperty()]
        public bool restartNewStory = false;

        public EndAndChangeStoryNode()
        {
            outputs.Clear();
        }
        
        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            if (newStory != null)
            {
                CodeGraphObject newStoryInstanced = GameManager.Instance.ProvideStoryManager().ChangeStory(newStory, true);
                if (restartNewStory)
                {
                    newStoryInstanced.RestartGraph(); 
                }
                card = newStoryInstanced.GetNextCard();
                return true;
            }
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            return string.Empty;
        }

    }
}

