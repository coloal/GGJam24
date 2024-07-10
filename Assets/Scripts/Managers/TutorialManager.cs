using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TutorialInfoTemplate tutorialInfo;
    
    [Header("Canvas masks")]
    [SerializeField] private RectTransform playerCardsMask;
    [SerializeField] private RectTransform enemyCardsMask;
    [SerializeField] private RectTransform firstCardMask;
    [SerializeField] private RectTransform secondCardMask;
    [SerializeField] private RectTransform thirdCardMask;
    [SerializeField] private RectTransform combatResultMask;
    [SerializeField] private RectTransform drawResultMask;
    [SerializeField] private RectTransform noteBookMask;
    [SerializeField] public float MaskVelocity = 30;
    

    [SerializeField] private TutorialEnemyDeckManager enemyDeck;
    public TutorialEnemyDeckManager EnemyDeck => enemyDeck;

    [Header("Show enemy cards types hints animation configuration")]
    [SerializeField] private Transform enemyCardsTypesHintsOriginPosition;
    [SerializeField] private Transform enemyCardsTypesHintsDestinationPosition;

    [HideInInspector]
    public int TutorialCombatTurn = 0;
    public int trippingCount = 0;
    private int draws = 0;
    

    private bool isMasking = false;
    private bool isDemaskig = false;
    private RectTransform currentMask;
    private float maxMaskScale;
    private Action onMask = ()=> { };
    private Action onDemask = ()=> { };


    
    public static TutorialManager SceneTutorial => CombatSceneManager.Instance.ProvideTutorialManager();
    public TutorialInfoTemplate TutorialInfo => tutorialInfo;



    public void StartInitialConversation(Action onFinishConversation)
    {
        DialogManager.SceneDialog.CreateDialog(tutorialInfo.InitialConversation, onFinishConversation);
    }

    public void StartCardExplanation(Action onFinishExplanation)
    {
        if (TutorialCombatTurn == 0) StartConversationWithBlock(playerCardsMask, tutorialInfo.CardExplanation, onFinishExplanation);
        else onFinishExplanation();
    }

    public void StartNoteBookExplanation(Action onFinishExplanation)
    {
        StartConversationWithBlock(noteBookMask, tutorialInfo.BookExplanation, onFinishExplanation);
    }

    public void StartEnemyCardExplanationPreShow(Action onFinishExplanation)
    {
        DialogManager.SceneDialog.CreateDialog(tutorialInfo.EnemyCardExplanationPreShow, onFinishExplanation);
    }
    public void StartEnemyCardExplanationWhileShow(Action onFinishExplanation)
    {
        StartConversationWithBlock(enemyCardsMask, tutorialInfo.EnemyCardExplanationWhileShow, onFinishExplanation);
    }

    public void StartWinExplanation(Action onFinishExplanation)
    {
        StartConversationWithBlock(combatResultMask, tutorialInfo.WinExplanation, onFinishExplanation);
        TutorialCombatTurn++;
    }
    public void StartLoseExplanation(Action onFinishExplanation)
    {
        StartConversationWithBlock(combatResultMask, tutorialInfo.LoseExplanation, onFinishExplanation);
        TutorialCombatTurn++;
    }
    public void StartDrawExplanation(Action onFinishExplanation)
    {
        List<string> text = draws == 0 ? tutorialInfo.DrawExplanationFirst : draws == 1 ? tutorialInfo.DrawExplanationSecond : tutorialInfo.DrawExplanationThird;
        StartConversationWithBlock(drawResultMask, text, onFinishExplanation);
        draws++;
        TutorialCombatTurn++;
    }
    public void StartEnemyPickCardConversation(Action onFinishConversation)
    {
        List<string> text = new List<string>();
        switch(TutorialCombatTurn)
        {
            case 0:
                text = tutorialInfo.EnemyPlaysCardExplanationViolenceWin;
                break;
            case 1:
                text = tutorialInfo.EnemyPlaysCardExplanationMoneyLose;
                break;
            case 2:
                text = tutorialInfo.EnemyPlaysCardExplanationMoneyDraw;
                break;
            case 3:
                text = tutorialInfo.EnemyPlaysCardExplanationInfluenceDraw;
                break;
            case 4:
                text = tutorialInfo.EnemyPlaysCardExplanationViolenceDraw;
                break;
        }
        DialogManager.SceneDialog.CreateDialog(text, onFinishConversation);
    }

    public void StartPlayerPickConversation(Action onFinishConversation)
    {
        List<string> text = new List<string>();
        RectTransform mask = null;
        switch (TutorialCombatTurn)
        {
            case 0:
                mask = secondCardMask;
                text = tutorialInfo.PlayerPlaysCardExplanationInfluenceWin;
                break;
            case 1:
                mask = firstCardMask;
                text = tutorialInfo.PlayerPlaysCardExplanationViolenceLose;
                break;
            case 2:
                mask = thirdCardMask;
                text = tutorialInfo.PlayerPlaysCardExplanationMoneyDraw;
                break;
            case 3:
                mask = secondCardMask;
                text = tutorialInfo.PlayerPlaysCardExplanationInfluenceDraw;
                break;
            case 4:
                mask = firstCardMask;
                text = tutorialInfo.PlayerPlaysCardExplanationViolenceDraw;
                break;
        }
        if (mask != null) DialogManager.SceneDialog.CreateDialog(text, () => BlockScreen(mask, onFinishConversation));
    }

    public bool shouldCardBeActive(int cardPosition)
    {
        switch (TutorialCombatTurn)
        {
            case 0:
                return cardPosition == 1;
            case 1:
                return cardPosition == 0;
            case 2:
                return cardPosition == 2;
            case 3:
                return cardPosition == 1;
            case 4:
                return cardPosition == 0;
        }
        return false;
    }
    public void StartCoinCardExplanation(Action onFinishExplanation)
    {
        DialogManager.SceneDialog.CreateDialog(tutorialInfo.CoinCardExplanation, onFinishExplanation);
    }

    public void StartCoinExplanation(Action onFinishExplanation)
    {
        DialogManager.SceneDialog.CreateDialog(tutorialInfo.CoinExplanation, onFinishExplanation);
    }
    public void StartBattleResultExplanation(Action onFinishExplanation)
    {
        DialogManager.SceneDialog.CreateDialog(tutorialInfo.BattleResultExplanation, onFinishExplanation);

    }

    public void StartPickWinCardExplanation(Action onFinishExplanation)
    {
        DialogManager.SceneDialog.CreateDialog(tutorialInfo.PickWinCardExplanation, onFinishExplanation);
    }

    public void PickWinCard()
    {
        BlockScreen(enemyCardsMask, () => { });
    }

    public void PlayerTripping(Action onFinishConversation)
    {
        List<string> text = new List<string>();
        switch (TutorialCombatTurn * 3 + trippingCount)
        {
            case 0:
                text = tutorialInfo.PlayerTrippingInfluence;
                break;
            case 1:
                text = tutorialInfo.PlayerTrippingInfluence_1;
                break;
            case 2:
                text = tutorialInfo.PlayerTrippingInfluence_2;
                break;
            case 3:
                text = tutorialInfo.PlayerTrippingViolence;
                break;
            case 4:
                text = tutorialInfo.PlayerTrippingViolence_1;
                break;
            case 5:
                text = tutorialInfo.PlayerTrippingViolence_2;
                break;
            case 6:
                text = tutorialInfo.PlayerTrippingMoney;
                break;
            case 7:
                text = tutorialInfo.PlayerTrippingMoney_1;
                break;
            case 8:
                text = tutorialInfo.PlayerTrippingMoney_2;
                break;
            case 9:
                text = tutorialInfo.PlayerTrippingInfluence;
                break;
            case 10:
                text = tutorialInfo.PlayerTrippingInfluence_1;
                break;
            case 11:
                text = tutorialInfo.PlayerTrippingInfluence_2;
                break;
            case 12:
                text = tutorialInfo.PlayerTrippingViolence;
                break;
            case 13:
                text = tutorialInfo.PlayerTrippingViolence_1;
                break;
            case 14:
                text = tutorialInfo.PlayerTrippingViolence_2;
                break;
        }
        DialogManager.SceneDialog.CreateDialog(text, onFinishConversation);
        trippingCount = Math.Min(trippingCount + 1, 2);
    }
    public void StartEndExplanation(Action onFinishExplanation)
    {
        DialogManager.SceneDialog.CreateDialog(tutorialInfo.EndExplanation, onFinishExplanation);
    }

    void StartConversationWithBlock(RectTransform mask, List<string> text, Action onFinishConversation)
    {
        BlockScreen(mask, () => DialogManager.SceneDialog.CreateDialog(text, () => UnBlockScreen(() => onFinishConversation())));
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMasking)
        {
            currentMask.localScale -= new Vector3(Time.deltaTime * MaskVelocity * maxMaskScale/10, Time.deltaTime * MaskVelocity * maxMaskScale / 10, 0);
            if(currentMask.localScale.x <= 1)
            {
                currentMask.localScale = new Vector3(1, 1, 1);
                isMasking = false;
                onMask();
            }
        }
        else if(isDemaskig)
        {
            currentMask.localScale += new Vector3(Time.deltaTime * MaskVelocity * maxMaskScale / 10, Time.deltaTime * MaskVelocity * maxMaskScale / 10, 0);
            if(currentMask.localScale.x >= maxMaskScale)
            {
                currentMask.localScale = new Vector3(maxMaskScale, maxMaskScale, 1);
                isDemaskig = false;
                currentMask.gameObject.SetActive(false);
                onDemask();
                
            }
        }
    }

    public void BlockScreen(RectTransform mask, Action onBlock)
    {   
        currentMask = mask;
        onMask = onBlock;
        maxMaskScale = mask.localScale.x;
        isMasking = true;
        isDemaskig = false;
        mask.gameObject.SetActive(true);
    }
    public void UnBlockScreen(Action onUnblock)
    {
        onDemask = onUnblock;
        isMasking = false;
        isDemaskig = true;
    }

    public Transform GetEnemyCardsTypesHintOriginPosition()
    {
        return enemyCardsTypesHintsOriginPosition;
    }

    public Transform GetEnemyCardsTypesHintDestinationPosition()
    {
        return enemyCardsTypesHintsDestinationPosition;
    }
}
