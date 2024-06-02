using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public static CombatSceneManager Instance;

   
    [SerializeField] CombatVisualManager combatVisualManager;
    [SerializeField] CombatV2Manager combatV2Manager;

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
}
