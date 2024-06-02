using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossCoindState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        throw new System.NotImplementedException();
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        PostProcess(combatContext);
    }
}
