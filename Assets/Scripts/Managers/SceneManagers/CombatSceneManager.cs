using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Canvas canvas;
    TutorialManager tutorialManager;

    [Header("Debug configurations")]
    [SerializeField] private bool isDebugging = false;
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
        // if(GameManager.Instance.ProvideBrainManager().IsTutorial)
        // {
        //     SceneManager.sceneLoaded += onTutorialLoaded;
        //     SceneManager.LoadScene("CombatSceneTutorialLogic", LoadSceneMode.Additive);
        // }
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
        if (isDebugging)
        {
            return debugEnemyTemplate;
        }
        else
        {
            return GameManager.Instance.ActualEnemy;
        }
    }
}
