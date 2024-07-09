using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] public float TextSound = 0.4f;
    [SerializeField] public float SlowerTextSound = 1.8f;
    [SerializeField] public float AngryTextSound = 2f;

    [SerializeField] private ConversationController ConversationController;
    public static DialogManager SceneDialog => CombatSceneManager.Instance.ProvideDialogManager();
  
    public void CreateDialog(List<string> text, Action onDialogFinished)
    {
        ConversationController.StartConversation(text, onDialogFinished);
    }
}
