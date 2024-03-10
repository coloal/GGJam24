using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Change Story", "Story Nodes/Change Story")]
    public class StoryChangeNode : CodeGraphNode
    {
        [ExposedProperty()]
        public CodeGraphObject newStory;
        [ExposedProperty()]
        public bool restartNewStory = false;

        public override bool GetNodeCard(out CardTemplate card)
        {
            if(newStory != null)
            {
                CodeGraphObject newStoryInstanced = GameManager.Instance.ProvideStoryManager().SearchStory(newStory);
                if (restartNewStory) { newStoryInstanced.RestartGraph(); }
                card = newStoryInstanced.GetNextCard();
                GameManager.Instance.ProvideStoryManager().ChangeStory(newStoryInstanced, true);
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

