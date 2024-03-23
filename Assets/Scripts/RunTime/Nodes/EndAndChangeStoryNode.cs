using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("End and Change Story", "Story Nodes/End Story And Change", color: "#01295F")]
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
        
        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            if (newStory != null)
            {
                CodeGraphObject newStoryInstanced = GameManager.Instance.ProvideStoryManager().ChangeStory(newStory, true);
                if (restartNewStory)
                {
                    newStoryInstanced.RestartGraph(); 
                }
                stepInfo = newStoryInstanced.ExecuteGraphStep();
                return true;
            }
            Debug.LogError("ChangeStoryNode has no story");
            stepInfo = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            return string.Empty;
        }

    }
}

