using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResultDrawState : CombatState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

        float secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
        CombatState nextCombatState = null;

        //Al enemigo o al Player le quedan cartas
        if (GetEnemyDeck().GetNumberOfCardsInDeck() > 0
            && CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInHand() > 0 )
        {
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
            nextCombatState = new PickEnemyCardState();
        }
        else
        {
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForTossCoin;
            nextCombatState = new TossCoinState();
        }

        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: nextCombatState,
            seconds: secondsForNextProcessState
        );
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
    }

    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        await AttackCards(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        await SendPlayerCombatCardToTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        await SendEnemyCombatCardToTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        PostProcess(combatContext);
    }

    protected async Task AttackCards(CombatManager.CombatContext combatContext)
    {
        CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
        CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();

        if (playerCombatCard != null && enemyCombatCard != null)
        {
            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayAttackCardsOnTie(playerCombatCard, enemyCombatCard);
        }
    }

    protected async Task SendPlayerCombatCardToTieZone(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();

        if (playerCombatCard != null)
        {
            Transform firstAvailablePositionInTieZone = combatContext
                .GetPlayerCardInTieZoneContainers()
                .Find((cardInTieZoneContainer) => cardInTieZoneContainer.childCount == 0);

            playerDeckManager.AddCardToTieZone(playerCombatCard);
            playerCombatCard.gameObject.transform.SetParent(
                firstAvailablePositionInTieZone,
                worldPositionStays: true
            );

            combatContext.playerOnCombatCard = null;
            
            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayMoveCardToTieZone(
                    cardToMove: playerCombatCard,
                    firstAvailablePositionInTieZone
                );
        }
    }

    protected async Task SendEnemyCombatCardToTieZone(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();
        CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();

        if (enemyCombatCard != null)
        {
            Transform firstAvailablePositionInTieZone = combatContext
                .GetEnemyCardInTieZoneContainers()
                .Find((cardInTieZoneContainer) => cardInTieZoneContainer.childCount == 0);

            enemyDeckManager.AddCardToTieZone(enemyCombatCard);
            enemyCombatCard.gameObject.transform.SetParent(
                firstAvailablePositionInTieZone,
                worldPositionStays: true
            );

            combatContext.enemyOnCombatCard = null;

            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayMoveCardToTieZone(
                    cardToMove: enemyCombatCard,
                    firstAvailablePositionInTieZone
                );
        }
    }
}
