using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatSceneManager : BaseSceneManager
{
    public static CombatSceneManager Instance;

    [Header("Managers on scene")]
    [SerializeField] CombatManager combatManager;
    [SerializeField] PlayerDeckManager playerDeckManager;
    [SerializeField] EnemyDeckManager enemyDeckManager;
    [SerializeField] CombatFeedbacksManager combatFeedbacksManager;
    [SerializeField] DialogManager dialogManager;
    TutorialManager tutorialManager;
    [SerializeField] Canvas canvas;
    [SerializeField] public NotebookComponent NotebookComponent;
    [Header("Debug configurations")]
    [Header("Scene debug configurations")]
    [SerializeField] private bool isDebbugingScene = false;
    [Header("Enemy debug comfigurations")]
    [SerializeField] private bool isDebuggingEnemy = false;
    [SerializeField] private EnemyTemplate debugEnemyTemplate;
    
    
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
    }

    void Start()
    { 
        Init();
        if(GameManager.Instance.ProvideBrainManager().IsTutorial)
        {
            SceneManager.sceneLoaded += onTutorialLoaded;
            SceneManager.LoadScene("CombatSceneTutorialLogic", LoadSceneMode.Additive);
        }
    }

    override protected void Init()
    {
        base.Init();
        if (isDebbugingScene)
        {
            SceneManager.LoadScene(ScenesNames.CombatDebugScene, LoadSceneMode.Additive);
        }
    }

    void onTutorialLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        SceneManager.sceneLoaded -= onTutorialLoaded;
        GameObject tutorialManagerObject = GameObject.Find("/TutorialManager");
        if (tutorialManagerObject)
        {
            tutorialManager = tutorialManagerObject.GetComponent<TutorialManager>();
            GameObject tutorialCanvas = GameObject.Find("/TutorialCanvas");
            if (tutorialCanvas != null)
            {
                Transform tutorialElements = tutorialCanvas.transform.GetChild(0);
                if (tutorialElements != null)
                {
                    tutorialElements.SetParent(canvas.transform);
                    dialogManager.ConversationController.gameObject.transform.SetAsLastSibling();
                }
                Destroy(tutorialCanvas);
            }
            combatManager.StartTutorial();
        }
    }

    public CombatManager ProvideCombatManager()
    {
        return combatManager;
    }

    public TutorialManager ProvideTutorialManager()
    {
        return tutorialManager;
    }
    public DialogManager ProvideDialogManager()
    {
        return dialogManager;
    }

    public PlayerDeckManager ProvidePlayerDeckManager()
    {
        return playerDeckManager;
    }

    public EnemyDeckManager ProvideEnemyDeckManager()
    {
        return enemyDeckManager;
    }

    public CombatFeedbacksManager ProvideCombatFeedbacksManager()
    {
        return combatFeedbacksManager;
    }

    public EnemyTemplate ProvideEnemyData()
    {
        if (isDebuggingEnemy)
        {
            return debugEnemyTemplate;
        }
        else
        {
            return GameManager.Instance.ActualEnemy;
        }
    }
}
