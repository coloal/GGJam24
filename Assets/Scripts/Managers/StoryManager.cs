using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

    
    

    [SerializeField]
    CodeGraphObject currentStory;
    [SerializeField] 
    private List<CodeGraphObject> allStories;

    private Stack<CodeGraphObject> storyStack;

    private bool bLastSwipeWasLeft = false; 
    private bool bFinishedGame = false;

    public void SwipeRight()
    {
        bLastSwipeWasLeft = false;
    }

    public void SwipeLeft()
    {
        bLastSwipeWasLeft = true;
    }

    public bool GetNextCardInGraph(out CardTemplate nextCard)
    {
        nextCard = currentStory.GetNextCard(bLastSwipeWasLeft);
        return bFinishedGame;
    }

    public void ChangeStory(CodeGraphObject newHistory, bool storeActualStory)
    {
        if(storeActualStory)
        {
            storyStack.Push(currentStory);
        }
        currentStory = newHistory;
    }

    public CodeGraphObject ReturntoParentStory()
    {
        currentStory = storyStack.Pop();
        return currentStory;
    }

    public void FinishGame()
    {
        bFinishedGame = true;
    }

    public CodeGraphObject SearchStory(CodeGraphObject Story)
    {
        return allStories.Find((other) => Story.GetType().Equals(other.GetType()));
    }
    

}
