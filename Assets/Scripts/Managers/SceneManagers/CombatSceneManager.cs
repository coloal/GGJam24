using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public static CombatSceneManager Instance;

    [SerializeField] CombatManager combatManager;
    [SerializeField] CombatVisualManager combatVisualManager;

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

    public CombatVisualManager ProvideCombatVisualManager()
    {
        return combatVisualManager;
    }
}
