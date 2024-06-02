using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCardsState : CombatState
{
    public override CombatState PostProcess()
    {
        return new ResolveCombatState();
    }

    public override void Preprocess()
    {
    }

    public override void ProcessImplementation()
    {
    }
}
