using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ConversationController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] TextAnimationComponent textAnimation;
    bool talkHasFinished = true;
    bool TalkHasFinished => talkHasFinished;
    Action OnConversationEnd = null;
    List<string> currentConversation = new List<string>();
    bool inputIsBlocked = false;
    public bool InputIsBlocked {
        get { return inputIsBlocked; }
        set { inputIsBlocked = value; }
    }
    public void EndConversation()
    {
        InputManager.GameInputManager.onClickEvent -= NextText;
        gameObject.SetActive(false);
        OnConversationEnd();
    }

    public void StartConversation(List<string> conversation, Action OnConversationEnd)
    {
        gameObject.SetActive(true);
        currentConversation = new List<string>(conversation);
        InputManager.GameInputManager.onClickEvent += NextText;
        this.OnConversationEnd = OnConversationEnd;
        NextText();
    }
    
    public void Talk(string message)
    {
        talkHasFinished = false;
        textAnimation.PlayTypewriterAnimation(textMesh, message, () => talkHasFinished = true);
    }
    public void NextText()
    {
        if (!talkHasFinished || inputIsBlocked) return;
        if(!currentConversation.Any())
        {
            EndConversation();
            return;
        }
        string nextText = currentConversation[0];
        currentConversation.RemoveAt(0);
        Talk(nextText);
    }
}
