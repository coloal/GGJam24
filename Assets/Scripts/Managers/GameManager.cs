using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    [SerializeField]
    AudioManager AudioManager;
    [SerializeField]
    TurnManager TurnManager;
    [SerializeField]
    CardsManager CardsManager;
    [SerializeField]
    EndManager EndManager;
    [SerializeField]
    BrainManager BrainManager;
    [SerializeField]
    BrainSoundManager BrainSoundManager;
    [SerializeField]
    StoryManager StoryManager;
    [SerializeField]
    PartyManager PartyManager;

    private CombatCardTemplate actualCombatEnemyCard;
    public CombatCardTemplate ActualCombatEnemyCard => actualCombatEnemyCard;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        StartGame();
    }

    void StartGame()
    {
        BrainSoundManager.StartGame();
        TurnManager.StartTurn();   
    }

    public void StartCombat(CombatCardTemplate enemyCard)
    {
        actualCombatEnemyCard = enemyCard;
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
        
    }

    public void EndCombat(TurnResult combatResult)
    {
        switch (combatResult)
        {
            case TurnResult.COMBAT_WON:
                TurnManager.WinCombat();
                break;
            case TurnResult.COMBAT_LOST:
                TurnManager.LoseCombat();
                break;
            default:
                Debug.LogError("Combat returned invalid result");
                break;
        }
    }


    // Maybe we need several FinishGame functions for every final that the game has
    public void FinishGame() 
    {
        Debug.Log("The game has finished! Congratulations ...or maybe not?");

    }


    public CardsManager ProvideCardsManager()
    {
        return CardsManager;
    }
    public AudioManager ProvideAudioManager()
    {
        return AudioManager;
    }

    public TurnManager ProvideTurnManager()
    {
        return TurnManager;
    }
    public EndManager ProvideEndManager() {
        return EndManager;
    }

    public BrainManager ProvideBrainManager()
    {
        return BrainManager;
    }

    public BrainSoundManager ProvideBrainSoundManager()
    {
        return BrainSoundManager;
    }

    public StoryManager ProvideStoryManager()
    {
        return StoryManager;
    }

    public PartyManager ProvidePartyManager()
    {
        return PartyManager;
    }

    
}
