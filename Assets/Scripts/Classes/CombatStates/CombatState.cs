using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatState
{
    abstract public void Preprocess(CombatV2Manager.CombatContext combatContext);

    abstract public void ProcessImplementation(CombatV2Manager.CombatContext combatContext);

    // This method should return the next state to process, not a void
    abstract public void PostProcess(CombatV2Manager.CombatContext combatContext);

    // This method should return the next state to process, not a void
    public void Process(CombatV2Manager.CombatContext combatContext)
    {
        Debug.Log($"Started: {this.GetType().Name}");

        ProcessImplementation(combatContext);
    }
}
