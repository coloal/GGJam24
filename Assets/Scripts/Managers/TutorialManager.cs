using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TutorialManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TutorialInfoTemplate tutorialInfo;
    [SerializeField] private ConversationController tutorialConversationController;
    [SerializeField] private RectTransform playerCardsMask;
    [SerializeField] private RectTransform enemyCardsMask;
    [SerializeField] private RectTransform firstCardMask;
    [SerializeField] private RectTransform secondCardMask;
    [SerializeField] private RectTransform thirdCardMask;
    [SerializeField] private RectTransform combatResultMask;

    [SerializeField] private float MaskVelocity = 30;

    public int TutorialCombatTurn = 0;
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
        tutorialConversationController.StartConversation(tutorialInfo.InitialConversation, onFinishConversation);
    }

    public void StartCardExplanation(Action onFinishExplanation)
    {
        if (TutorialCombatTurn == 0) StartConversationWithBlock(playerCardsMask, tutorialInfo.CardExplanation, onFinishExplanation);
        else onFinishExplanation();
    }
    public void StartEnemyCardExplanationPreShow(Action onFinishExplanation)
    {
        tutorialConversationController.StartConversation(tutorialInfo.EnemyCardExplanationPreShow, onFinishExplanation);
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
        StartConversationWithBlock(combatResultMask, text, onFinishExplanation);
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
        tutorialConversationController.StartConversation(text, onFinishConversation);
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
        if (mask != null) tutorialConversationController.StartConversation(text, () => BlockScreen(mask, onFinishConversation));
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
        tutorialConversationController.StartConversation(tutorialInfo.CoinCardExplanation, onFinishExplanation);
    }

    public void StartCoinExplanation(Action onFinishExplanation)
    {
        tutorialConversationController.StartConversation(tutorialInfo.CoinExplanation, onFinishExplanation);
    }
    public void StartBattleResultExplanation(Action onFinishExplanation)
    {
        tutorialConversationController.StartConversation(tutorialInfo.BattleResultExplanation, onFinishExplanation);

    }

    public void StartPickWinCardExplanation(Action onFinishExplanation)
    {
        tutorialConversationController.StartConversation(tutorialInfo.PickWinCardExplanation, onFinishExplanation);
    }

    public void PickWinCard()
    {
        BlockScreen(enemyCardsMask, () => { });
    }

    public void StartEndExplanation(Action onFinishExplanation)
    {
        tutorialConversationController.StartConversation(tutorialInfo.EndExplanation, onFinishExplanation);
    }

    void StartConversationWithBlock(RectTransform mask, List<string> text, Action onFinishConversation)
    {
        BlockScreen(mask, () => tutorialConversationController.StartConversation(text, () => UnBlockScreen(() => onFinishConversation())));

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMasking)
        {
            currentMask.localScale -= new Vector3(Time.deltaTime * MaskVelocity, Time.deltaTime * MaskVelocity, 0);
            if(currentMask.localScale.x <= 1)
            {
                currentMask.localScale = new Vector3(1, 1, 1);
                isMasking = false;
                onMask();
            }
        }
        else if(isDemaskig)
        {
            currentMask.localScale += new Vector3(Time.deltaTime * MaskVelocity, Time.deltaTime * MaskVelocity, 0);
            if(currentMask.localScale.x >= maxMaskScale)
            {
                currentMask.localScale = new Vector3(maxMaskScale, maxMaskScale, 1);
                isDemaskig = false;
                onDemask();
                currentMask.gameObject.SetActive(false);
            }
        }
    }

    public void BlockScreen(RectTransform mask, Action onBlock)
    {
        onMask = onBlock;
        maxMaskScale = mask.localScale.x;
        isMasking = true;
        isDemaskig = false;
        currentMask = mask;
        mask.gameObject.SetActive(true);
    }
    public void UnBlockScreen(Action onUnblock)
    {
        onDemask = onUnblock;
        isMasking = false;
        isDemaskig = true;
    }
}