using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatState
{
    abstract public void Preprocess();

    abstract public void ProcessImplementation();

    // This method should return the next state to process, not a void
    abstract public CombatState PostProcess();

    // This method should return the next state to process, not a void
    public CombatState Process(CombatV2Manager.CombatContext combatContext)
    {
        Debug.Log($"Started: {this.GetType().Name}");

        Preprocess();
        
        ProcessImplementation();
        
        return PostProcess();
    }
}
