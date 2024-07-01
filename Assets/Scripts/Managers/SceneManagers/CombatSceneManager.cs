using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public static CombatSceneManager Instance;

    [Header("Managers on scene")]
    [SerializeField] CombatManager combatManager;
    [SerializeField] PlayerDeckManager playerDeckManager;
    [SerializeField] EnemyDeckManager enemyDeckManager;
    [SerializeField] CombatFeedbacksManager combatFeedbacksManager;

    [Header("GameObjects")]
    [SerializeField] private GameObject CoinCardGO;

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
    }

    public CombatManager ProvideCombatManager()
    {
        return combatManager;
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

    public GameObject ProvideCoinCardGO() 
    {
        return CoinCardGO;
    }
}
