using CodeGraph;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PrefabCodeGraph;

    [SerializeField]
    private CodeGraphAsset FirstStory;

    private CodeGraphObject currentStory = null;

    private List<CodeGraphObject> storyStack;

    private List<CodeGraphObject> existingStoryList;

    private bool bLastSwipeWasLeft = false;
    private bool bFinishedGame = false;

    private void Awake()
    {
        existingStoryList = new List<CodeGraphObject>();
        storyStack = new List<CodeGraphObject>();
        InitStory();
    }


    public void InitStory()
    {
        GameObject go = Instantiate(PrefabCodeGraph);
        go.GetComponent<CodeGraphObject>().ExecuteAsset(FirstStory);
        currentStory = go.GetComponent<CodeGraphObject>();
    }

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
        if(currentStory == null)
        {
            InitStory();
        }
        nextCard = currentStory.GetNextCard(bLastSwipeWasLeft);
        return bFinishedGame;
    }

    public CodeGraphObject ChangeStory(CodeGraphAsset newHistory, bool storyHasEnded)
    {
        //Esto es para subarboles
        if (!storyHasEnded)
        {
            existingStoryList.Add(currentStory);
            storyStack.Add(currentStory);
        }
        else
        {
            FinishStory();
        }

        CodeGraphObject story = SearchStory(newHistory);
        if (story != null)
        {
            currentStory = story;
        }
        else
        {
            GameObject gameObject = Instantiate(PrefabCodeGraph);
            gameObject.GetComponent<CodeGraphObject>().ExecuteAsset(newHistory);
            currentStory = gameObject.GetComponent<CodeGraphObject>();
        }
        return currentStory;
    }

    public CodeGraphObject ReturntoParentStory()
    {
        FinishStory();
        currentStory = storyStack[storyStack.Count - 1];
        storyStack.RemoveAt(storyStack.Count-1);
        return currentStory;
    }

    public void FinishStory()
    {
        
        existingStoryList.Remove(currentStory);
        storyStack.RemoveAll(other => currentStory.Equals(other));
        Destroy(currentStory.gameObject);

    }

    public void FinishGame()
    {
        bFinishedGame = true;
    }


    public CodeGraphObject SearchStory(CodeGraphAsset story)
    {
        return existingStoryList.Find((other) => {
            return other.GetGraphAsset().Equals(story);
        });
    }
    

}
