using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    [SerializeField] InputManager inputManager;
    [SerializeField] BrainManager brainManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] StoryManager storyManager;
    [SerializeField] PartyManager partyManager;
    [SerializeField] InventoryManager inventoryManager;
    [SerializeField] Transform canvas;

    [Header("Scene transitions")]
    [SerializeField] private float secondsForSceneTransition = 0.5f;
    [SerializeField] private float secondsForCombatTransition = 1.0f;

    [HideInInspector]
    public bool IsMainGame;
    private bool hasAStoryStarted = false;

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

    private EnemyTemplate actualEnemy;
    public EnemyTemplate ActualEnemy => actualEnemy;

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
        
    }

    public void StartCombat(EnemyTemplate enemy, bool isBossFigth, MusicTracks music)
    {
        actualEnemy = enemy;
        EnterBattleScene(isBossFigth, music);
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

            case TurnResult.COMBAT_GAME_OVER:
                action = () =>
                {
                    if (currentSceneManager is MainGameSceneManager mainGameSceneManager)
                    {
                        //ProvideSoundManager().StartGameOver();
                        mainGameSceneManager.ProvideTurnManager().LoseCombat();
                    }
                };
                break;

            default:
                action = () => { };
                Debug.LogError("Combat returned invalid result");
                break;
        }
        ExitBattleScene(action);
    }

    // Maybe we need several FinishGame functions for every final that the game has
    public void FinishGame() 
    {
        Debug.Log("The game has finished! Congratulations ...or maybe not?");

    }

    public InputManager ProvideInputManager()
    {
        return inputManager;
    }

    public BrainManager ProvideBrainManager()
    {
        return brainManager;
    }

    public SoundManager ProvideSoundManager()
    {
        return soundManager;
    }

    public StoryManager ProvideStoryManager()
    {
        return storyManager;
    }

    public PartyManager ProvidePartyManager()
    {
        return partyManager;
    }

    public InventoryManager ProvideInventoryManager()
    {
        return inventoryManager;
    }

    public void OnSceneChanged()
    {
        foreach(Action action in disposableOnSceneChangeActions)
        {
            action();
        }
        disposableOnSceneChangeActions.Clear();
    }

    public void ChangeSceneWithAnimation(Animator transition, string SceneName)
    {
        ProvideInputManager().ClearEvents();
        Animator instantedAnimator = Instantiate(transition.gameObject).GetComponent<Animator>();
        if (instantedAnimator != null)
        {
            instantedAnimator.SetTrigger("ExitAnimation");
            GameUtils.CreateTemporizer(() => {
                SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
            }, secondsForSceneTransition, this);
            disposableOnSceneChangeActions.Add(() =>
            {
                Animator transition_1 = brainManager.ZoneInfo.CombatTransition;
                Animator instantedAnimator_1 = Instantiate(transition.gameObject).GetComponent<Animator>();
                if (instantedAnimator_1 != null)
                {
                    instantedAnimator_1.SetTrigger("EnterAnimation");
                    GameUtils.CreateTemporizer(() =>
                    {
                        Destroy(instantedAnimator_1.gameObject);
                    }, 1.0f, this);
                }
            });
        }
    }

    public void EnterBattleScene(bool isBoss, MusicTracks music)
    {
        List<CombatCardTemplate> members = ProvidePartyManager().GetPartyMembers();
        ProvideSoundManager().StartCombat(music);
        ProvideBrainManager().bIsBossFight = isBoss;

        Animator transition = brainManager.ZoneInfo.CombatTransition;
        Animator instantedAnimator = Instantiate(transition.gameObject).GetComponent<Animator>();
        if (instantedAnimator != null)
        {
            instantedAnimator.SetTrigger("ExitAnimation");
            GameUtils.CreateTemporizer(() => {
                SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
            }, secondsForCombatTransition, this);
            disposableOnSceneChangeActions.Add(() =>
            {
                Animator transition_1 = brainManager.ZoneInfo.CombatTransition;
                Animator instantedAnimator_1 = Instantiate(transition.gameObject).GetComponent<Animator>();
                if (instantedAnimator_1 != null)
                {
                    instantedAnimator_1.SetTrigger("EnterAnimation");
                    GameUtils.CreateTemporizer(() =>
                    {
                        Destroy(instantedAnimator_1.gameObject);
                    }, 1.0f, this);
                }
            });
        }
    }

    public void ExitBattleScene(Action nextAction)
    {
        //ProvideSoundManager().EndCombat();
        ProvideSoundManager().RestartMusicFromCombat();

        Animator transition = brainManager.ZoneInfo.CombatTransition;
        Animator instantedAnimator = Instantiate(transition.gameObject).GetComponent<Animator>();
        if (instantedAnimator != null)
        {
            string nextScene;
            if (IsMainGame)
            {
                nextScene = ScenesNames.MainGameScene;
            }
            else
            {
                nextScene = ScenesNames.MainCombatModeScene;
            }

            instantedAnimator.SetTrigger("ExitAnimation");
            GameUtils.CreateTemporizer(() => {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }, secondsForCombatTransition, this);
            disposableOnSceneChangeActions.Add(() =>
            {
                Animator transition_1 = brainManager.ZoneInfo.CombatTransition;
                Animator instantedAnimator_1 = Instantiate(transition.gameObject).GetComponent<Animator>();
                if (instantedAnimator_1 != null)
                {
                    instantedAnimator_1.SetTrigger("EnterAnimation");
                    GameUtils.CreateTemporizer(() =>
                    {
                        nextAction();
                        Destroy(instantedAnimator_1.gameObject);
                    }, 1.0f, this);
                }
            });
        }
    }

    public void ResetGame()
    {
        storyManager.ResetStory();
        brainManager.ResetMemories();
        soundManager.ChangeZone(brainManager.ZoneInfo.StoryMusicZone);
        soundManager.ResetNess();
        partyManager.ClearParty();
        inventoryManager.RestInventory();
        SetHasAStoryStarted(false);
    }

    public bool HasAStoryStarted()
    {
        return hasAStoryStarted;
    }

    public void SetHasAStoryStarted(bool hasAStoryStarted)
    {
        this.hasAStoryStarted = hasAStoryStarted;
    }

}
