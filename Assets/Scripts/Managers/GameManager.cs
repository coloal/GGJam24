using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    [SerializeField] InputManager inputManager;
    [SerializeField] BrainManager brainManager;
    [SerializeField] BrainSoundManager brainSoundManager;
    [SerializeField] StoryManager storyManager;
    [SerializeField] PartyManager partyManager;
    [SerializeField] InventoryManager inventoryManager;

    private bool hasToResetGame = false;

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
        brainSoundManager.StartGame();
        if(currentSceneManager != null && currentSceneManager is MainGameSceneManager mainGameSceneManager)
        {
            mainGameSceneManager.StartGame();
        }
    }

    public void StartCombat(CombatCardTemplate enemyCard)
    {
        actualCombatEnemyCard = enemyCard;
        EnterBattleScene();
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
                        GameManager.Instance.ProvideBrainSoundManager().StartGameOver();
                        mainGameSceneManager.ProvideTurnManager().LoseCombat(true);
                        SceneManager.LoadScene(ScenesNames.GameOverScene);
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

    public BrainSoundManager ProvideBrainSoundManager()
    {
        return brainSoundManager;
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

    public void EnterBattleScene()
    {
        bool IsBoss = GameManager.Instance.ProvideBrainManager().bIsBossFight;
        List<CombatCardTemplate> members = GameManager.Instance.ProvidePartyManager().GetPartyMembers();
        GameManager.Instance.ProvideBrainSoundManager().StartCombat(members, IsBoss);

        Animator transition = brainManager.ZoneInfo.CombatTransition;
        Animator instantedAnimator = Instantiate(transition.gameObject).GetComponent<Animator>();
        if (instantedAnimator != null)
        {
            instantedAnimator.SetTrigger("ExitAnimation");
            GameUtils.CreateTemporizer(() => {
                SceneManager.LoadScene("CombatScene", LoadSceneMode.Single);
            }, 1.0f, this);
            disposableOnSceneChangeActions.Add(() =>
            {
                Animator transition_1 = brainManager.ZoneInfo.CombatTransition;
                Animator instantedAnimator_1 = Instantiate(transition.gameObject).GetComponent<Animator>();
                if (instantedAnimator_1 != null)
                {
                    instantedAnimator_1.SetTrigger("EnterAnimation");
                    GameUtils.CreateTemporizer(() =>
                    {
                        Destroy(instantedAnimator_1);
                    }, 1.0f, this);
                }
            });
        }
    }

    public void ExitBattleScene(Action nextAction)
    {
        Animator transition = brainManager.ZoneInfo.CombatTransition;
        Animator instantedAnimator = Instantiate(transition.gameObject).GetComponent<Animator>();
        if (instantedAnimator != null)
        {
            instantedAnimator.SetTrigger("ExitAnimation");
            GameUtils.CreateTemporizer(() => {
                SceneManager.LoadScene(ScenesNames.MainGameScene, LoadSceneMode.Single);
            }, 1.0f, this);
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
                        Destroy(instantedAnimator_1);
                    }, 1.0f, this);
                }
            });
        }
    }

    public void ResetGame()
    {
        storyManager.ResetStory();
        brainManager.ResetMemories();
        brainSoundManager.ChangeZone(brainManager.ZoneInfo.StoryMusicZone);
        brainSoundManager.ResetNess();
        partyManager.ClearParty();
        SetHasToResetGame(true);
    }

    public bool HasToResetGame()
    {
        return hasToResetGame;
    }

    public void SetHasToResetGame(bool hasToResetGame)
    {
        this.hasToResetGame = hasToResetGame;
    }

}
