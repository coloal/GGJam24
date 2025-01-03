using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PickEnemyCardState : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PickPlayerCardState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickPlayerCard
        );
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public async override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        await PickAnEnemyCard(combatContext);
    }

    async protected Task PickAnEnemyCard(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();
        
        CombatCard enemyCombatCard = enemyDeckManager.DrawCardFromDeckToHand();
        enemyDeckManager.PutCardFromHandToCombatZone(enemyCombatCard);

        // Play draw a card from enemy deck
        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayEnemyDrawCardFromDeck(
                enemyDeck: combatContext.enemyDeck
            );
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        
        combatContext.enemyOnCombatCard = enemyCombatCard.gameObject;
        
        enemyCombatCard.gameObject.SetActive(true);
        enemyCombatCard.transform.SetParent(
            combatContext.combatContainer.transform.parent,
            worldPositionStays: false
        );
        enemyCombatCard.transform.position = combatContext.enemyOnCombatCardOriginalPosition.position;

        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayPlaceEnemyCardOnCombat(
                cardToPlaceOnCombat: enemyCombatCard,
                onCombatTransform: combatContext.enemyOnCombatCardFinalPosition
            );
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }

        PostProcess(combatContext);
    }
}
