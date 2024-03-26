using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public static CombatSceneManager Instance;

    [SerializeField] CombatManager combatManager;

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

    public CombatManager ProvideCombatManager()
    {
        return combatManager;
    }
}
