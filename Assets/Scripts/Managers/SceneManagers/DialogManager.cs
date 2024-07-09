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
    public ConversationController ConversationController => conversationController;
    public static DialogManager SceneDialog => CombatSceneManager.Instance.ProvideDialogManager();
  
    public void CreateDialog(List<string> text, Action onDialogFinished)
    {
        conversationController.StartConversation(text, onDialogFinished);
    }
}
