using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombatState : CombatState
{
    public override CombatState PostProcess()
    {
        return new PresentPlayerCardsState();
    }

    public override void Preprocess()
    {
    }

    public override void ProcessImplementation()
    {
    }
}
