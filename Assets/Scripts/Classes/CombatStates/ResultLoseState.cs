using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultLoseState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {

        //TODO: eliminar cartas obtenidas mediante BrainManager ??
        
        GameManager.Instance.EndCombat(TurnResult.COMBAT_GAME_OVER);
        //throw new System.NotImplementedException();
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        PostProcess(combatContext);
    }
}
