using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentEnemyCardsState : CombatState
{
    public override CombatState PostProcess()
    {
        return new PickEnemyCardState();
    }

    public override void Preprocess()
    {
    }

    public override void ProcessImplementation()
    {
    }
}
