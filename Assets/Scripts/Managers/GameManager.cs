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

    public BaseSceneManager CurrentSceneManager;
   

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
    }

    public void StartCombat(CombatCardTemplate enemyCard)
    {
        actualCombatEnemyCard = enemyCard;
        SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
        
    }

    public void EndCombat(TurnResult combatResult)
    {
        SceneManager.LoadScene(ScenesNames.MainGameScene, LoadSceneMode.Single);
            GameUtils.CreateTemporizer(() =>
            {
                if (CurrentSceneManager is MainGameSceneManager mainGameSceneManager)
                {
                    switch (combatResult)
                    {
                        case TurnResult.COMBAT_WON:
                            mainGameSceneManager.ProvideTurnManager().WinCombat();
                            break;
                        case TurnResult.COMBAT_LOST:
                            mainGameSceneManager.ProvideTurnManager().LoseCombat();
                            break;
                        default:
                            Debug.LogError("Combat returned invalid result");
                            break;
                    }
                }
            }, 2, this);
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

    
}
