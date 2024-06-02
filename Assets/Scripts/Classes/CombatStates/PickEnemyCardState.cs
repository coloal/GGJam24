using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickEnemyCardState : CombatState
{
    public override CombatState PostProcess()
    {
        return new PickPlayerCardState();
    }

    public override void Preprocess()
    {
    }

    public override void ProcessImplementation()
    {
    }
}
