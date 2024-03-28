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
    BrainManager BrainManager;
    [SerializeField]
    BrainSoundManager BrainSoundManager;
    [SerializeField]
    StoryManager StoryManager;
    [SerializeField]
    PartyManager PartyManager;

    private List<Action> disposableOnSceneChangeActions = new List<Action>();

    private BaseSceneManager currentSceneManager ;
    public BaseSceneManager CurrentSceneManager {
        get
        {
            return currentSceneManager;
        }
        set
        {
            currentSceneManager = value;
            OnSceneChanged();
        }
    }

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
        if(currentSceneManager != null && currentSceneManager is MainGameSceneManager mainGameSceneManager)
        {
            mainGameSceneManager.StartGame();
        }
    }

    public void StartCombat(CombatCardTemplate enemyCard)
    {
        actualCombatEnemyCard = enemyCard;
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
        
    }

    public void EndCombat(TurnResult combatResult)
    {
        Action action;
        switch (combatResult)
        {
            case TurnResult.COMBAT_WON_CAPTURE:
                action = () =>
                {
                    if (currentSceneManager is MainGameSceneManager mainGameSceneManager)
                    {
                        mainGameSceneManager.ProvideTurnManager().WinCombat(true);
                    }
                };
                break;

            case TurnResult.COMBAT_WON_NO_CAPTURE:
                action = () =>
                {
                    if (currentSceneManager is MainGameSceneManager mainGameSceneManager)
                    {
                        mainGameSceneManager.ProvideTurnManager().WinCombat(false);
                    }
                };
                break;

            case TurnResult.COMBAT_LOST:
                action = () =>
                {
                    if (currentSceneManager is MainGameSceneManager mainGameSceneManager)
                    {
                        mainGameSceneManager.ProvideTurnManager().LoseCombat(false);
                    }
                };
                break;

            case TurnResult.COMBAT_GAME_OVER:
                action = () =>
                {
                    if (currentSceneManager is MainGameSceneManager mainGameSceneManager)
                    {
                        mainGameSceneManager.ProvideTurnManager().LoseCombat(true);
                    }
                };
                break;
            default:
                action = () => { };
                Debug.LogError("Combat returned invalid result");
                break;
        }
        disposableOnSceneChangeActions.Add(action);
        SceneManager.LoadScene(ScenesNames.MainGameScene, LoadSceneMode.Single);
    }


    // Maybe we need several FinishGame functions for every final that the game has
    public void FinishGame() 
    {
        Debug.Log("The game has finished! Congratulations ...or maybe not?");

    }

    
   
    public AudioManager ProvideAudioManager()
    {
        return AudioManager;
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

    public void OnSceneChanged()
    {
        foreach(Action action in disposableOnSceneChangeActions)
        {
            action();
        }
        disposableOnSceneChangeActions.Clear();
    }
    
}
