using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCardsState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new ResolveCombatState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        // Show enemy card first
        CardAnimationsComponent enemyCardAnimationsComponent = combatContext.enemyOnCombatCard.GetComponent<CardAnimationsComponent>();
        CardAnimationsComponent playerCardAnimationsComponent = combatContext.playerOnCombatCard.GetComponent<CardAnimationsComponent>();
        if (enemyCardAnimationsComponent != null && playerCardAnimationsComponent != null)
        {
            enemyCardAnimationsComponent.ShowCard();
            GameUtils.CreateTemporizer(() => {
                playerCardAnimationsComponent.ShowCard();
            }, 2.0f, GameManager.Instance);
        }

        PostProcess(combatContext);
    }
}
