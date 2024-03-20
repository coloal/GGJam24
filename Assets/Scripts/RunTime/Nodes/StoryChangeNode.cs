using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Change Story", "Story Nodes/Change Story", color: "#01295F")]
    public class StoryChangeNode : CodeGraphNode
    {
        [ExposedProperty()]
        public CodeGraphAsset newStory;
        [ExposedProperty()]
        public bool restartNewStory = false;

        

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            if(newStory != null)
            {
                CodeGraphObject newStoryInstanced = GameManager.Instance.ProvideStoryManager().ChangeStory(newStory, false);
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
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, 0);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}

