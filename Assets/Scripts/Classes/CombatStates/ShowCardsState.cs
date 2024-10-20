using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShowCardsState : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new ResolveCombatState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForResolveCombat
        );
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
        CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
        if (enemyCombatCard != null && playerCombatCard != null)
        {
            await RevealCard(enemyCombatCard);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            await RevealCard(playerCombatCard);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
        }

        PostProcess(combatContext);
    }

    async Task RevealCard(CombatCard combatCard)
    {
        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayRevealCard(combatCard);
    }
}
