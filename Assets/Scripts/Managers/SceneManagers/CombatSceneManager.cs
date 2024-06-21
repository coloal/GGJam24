using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public static CombatSceneManager Instance;

    [Header("Managers on scene")]
    [SerializeField] CombatVisualManager combatVisualManager;
    [SerializeField] CombatV2Manager combatV2Manager;
    [SerializeField] PlayerDeckManager playerDeckManager;
    [SerializeField] EnemyDeckManager enemyDeckManager;

    [Header("Scene configurations")]
    [SerializeField] EnemyTemplate enemyTemplate;

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

    public CombatVisualManager ProvideCombatVisualManager()
    {
        return combatVisualManager;
    }

    public CombatV2Manager ProvideCombatV2Manager()
    {
        return combatV2Manager;
    }

    public PlayerDeckManager ProvidePlayerDeckManager()
    {
        return playerDeckManager;
    }

    public EnemyDeckManager ProvideEnemyDeckManager()
    {
        return enemyDeckManager;
    }

    public EnemyTemplate ProvideEnemyData()
    {
        return enemyTemplate;
    }
}
