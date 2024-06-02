using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSceneManager : BaseSceneManager
{
    public static CombatSceneManager Instance;

   
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

   

    public CombatVisualManager ProvideCombatVisualManager()
    {
        return combatVisualManager;
    }
}
