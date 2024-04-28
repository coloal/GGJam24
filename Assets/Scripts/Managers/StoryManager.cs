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

    private TurnResult LastActionResult = TurnResult.NO_RESULT;

    public static StoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
        existingStoryList = new List<CodeGraphObject>();
        storyStack = new List<CodeGraphObject>();
        InitStory();
       
    }
    
    public void InitStory()
    {
        GameObject go = Instantiate(PrefabCodeGraph);
        go.transform.parent = gameObject.transform;
        go.GetComponent<CodeGraphObject>().ExecuteAsset(FirstStory);
        currentStory = go.GetComponent<CodeGraphObject>();
    }

    public void SwipeRight()
    {
        LastActionResult = TurnResult.SWIPED_RIGHT;
    }

    public void SwipeLeft()
    {
        LastActionResult = TurnResult.SWIPED_LEFT;
    }
    public void LoseCombat(bool gameOver)
    {
        LastActionResult = gameOver ? TurnResult.COMBAT_GAME_OVER: TurnResult.COMBAT_LOST;
    }
    public void WinCombat(bool captured)
    {
        LastActionResult = captured ? TurnResult.COMBAT_WON_CAPTURE : TurnResult.COMBAT_WON_NO_CAPTURE;
    }



    public StepInfo ContinueStoryExecution()
    {
        return currentStory.ExecuteGraphStep(LastActionResult);
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
            gameObject.transform.parent = this.gameObject.transform;
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

    public CodeGraphObject SearchStory(CodeGraphAsset story)
    {
        return existingStoryList.Find((other) => {
            return other.GetGraphAsset().Equals(story);
        });
    }

    public void ResetStory()
    {
        existingStoryList = new List<CodeGraphObject>();
        storyStack = new List<CodeGraphObject>();
        InitStory();
    }
}
