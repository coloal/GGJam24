using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDrawState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().OverwriteCombatContext(combatContext);

        //Al enemigo o al Player le quedan cartas
        if (CombatSceneManager.Instance.ProvideEnemyDeckManager().GetNumberOfCardsInDeck() > 0
            && GameManager.Instance.ProvideDeckManager().GetNumberOfCardsInHand() > 0 )
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickEnemyCardState());
        }
        else
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new TossCoindState());
        }
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
    }
}
