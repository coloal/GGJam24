using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public override async void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
        CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
        if (enemyCombatCard != null && playerCombatCard != null)
        {
            await RevealCard(enemyCombatCard);
            await RevealCard(playerCombatCard);
        }

        PostProcess(combatContext);
    }

    async Task RevealCard(CombatCard combatCard)
    {
        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayRevealCard(combatCard);
    }
}
