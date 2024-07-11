using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] public float TextSound = 0.4f;
    [SerializeField] public float SlowerTextSound = 1.8f;
    [SerializeField] public float AngryTextSound = 2f;

    [SerializeField] private ConversationController conversationController;
    [SerializeField] private float animationVelocity = 1;

    bool isOpeningConversation = false;
    bool isClosingConversation = false;
    bool isOpenConversation = false;
    Action onOpenConversation;
    Action onCloseConversation;
    public ConversationController ConversationController => conversationController;
    public static DialogManager SceneDialog => CombatSceneManager.Instance.ProvideDialogManager();
  
    public void CreateDialog(List<string> text, Action onDialogFinished)
    {
        OpenConversation(() => conversationController.StartConversation(text, () => CloseConversation(onDialogFinished)));
    }

    private void Update()
    {
        if(isOpeningConversation)
        {
            conversationController.gameObject.transform.localScale += new Vector3 (animationVelocity*Time.deltaTime, animationVelocity * Time.deltaTime, animationVelocity * Time.deltaTime);
            if(conversationController.gameObject.transform.localScale.x >= 1)
            {
                conversationController.gameObject.transform.localScale = Vector3.one;
                isOpeningConversation = false;
                onOpenConversation();
            }
        }
        else if(isClosingConversation)
        {
            conversationController.gameObject.transform.localScale -= new Vector3(animationVelocity * Time.deltaTime, animationVelocity * Time.deltaTime, animationVelocity * Time.deltaTime);
            if (conversationController.gameObject.transform.localScale.x <= 0)
            {
                conversationController.gameObject.transform.localScale = Vector3.zero;
                isClosingConversation = false;
                conversationController.gameObject.SetActive(false);
                onCloseConversation();
            }
        }
    }

    private void OpenConversation(Action onOpenConversation)
    {
        if (isOpenConversation)
        {
            Debug.LogWarning("Trying to open a conversation that is already open");
            onOpenConversation();
            return;
        }
        conversationController.ClearText();
        conversationController.gameObject.SetActive(true);
        isClosingConversation = false;
        isOpenConversation = true;
        isOpeningConversation = true;
        this.onOpenConversation = onOpenConversation;
    }

    private void CloseConversation(Action onCloseConversation)
    {
        if (!isOpenConversation)
        {
            Debug.LogWarning("Trying to close a conversation that is already open");
            onOpenConversation();
            return;
        }
        isOpenConversation = false;
        isOpeningConversation = false;
        isClosingConversation = true;
        this.onCloseConversation = onCloseConversation;
    }

    public void BlockInput()
    {
        conversationController.InputIsBlocked = true;
    }

    public void UnblockInput()
    {
        conversationController.InputIsBlocked = false;
    }

}
