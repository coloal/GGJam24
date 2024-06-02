using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentPlayerCardsState : CombatState
{
    public override CombatState PostProcess()
    {
        return new PresentEnemyCardsState();
    }

    public override void Preprocess()
    {
    }

    public override void ProcessImplementation()
    {
    }
}
