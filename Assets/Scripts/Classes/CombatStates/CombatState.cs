using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatState
{
    abstract public void Preprocess(CombatManager.CombatContext combatContext);

    abstract public void ProcessImplementation(CombatManager.CombatContext combatContext);

    // This method should return the next state to process, not a void
    abstract public void PostProcess(CombatManager.CombatContext combatContext);

    // This method should return the next state to process, not a void
    public void Process(CombatManager.CombatContext combatContext)
    {
        Debug.Log($"Started: {this.GetType().Name}");

        ProcessImplementation(combatContext);
    }

    virtual protected EnemyDeckManager GetEnemyDeck()
    {
        return CombatSceneManager.Instance.ProvideEnemyDeckManager();
    }
}
