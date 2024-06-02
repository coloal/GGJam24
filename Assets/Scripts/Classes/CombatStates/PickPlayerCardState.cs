using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickPlayerCardState : CombatState
{
    public override CombatState PostProcess()
    {
        return new ShowCardsState();
    }

    public override void Preprocess()
    {
    }

    public override void ProcessImplementation()
    {
    }
}
