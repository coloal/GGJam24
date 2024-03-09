using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

    [SerializeField]
    List<CodeGraphObject> histories;

    [SerializeField]
    CodeGraphObject currentHistory;

    private bool bLastSwipeWasLeft = false; 

    public void SwipeRight()
    {
        bLastSwipeWasLeft = false;
    }

    public void SwipeLeft()
    {
        bLastSwipeWasLeft = true;
    }

    public CardTemplate GetNextCardInGraph()
    {
        return currentHistory.GetNextCard(bLastSwipeWasLeft);
    }
}
