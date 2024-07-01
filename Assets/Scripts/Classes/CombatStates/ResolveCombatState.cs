using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResolveCombatState : CombatState
{
    private enum CombatResult
    {
        PlayerWon,
        EnemyWon,
        Draw,
    }

    CombatState nextCombatState;

    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        if (nextCombatState != null)
        {
            CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

            float secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
            if (nextCombatState is ResultDrawState)
            {
                secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForDrawRound;
            }
            else if (nextCombatState is ResultWinState || nextCombatState is ResultLoseState)
            {
                secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForCombatResultsRound;
            }

            CombatUtils.ProcessNextStateAfterSeconds(
                nextState: nextCombatState,
                seconds: secondsForNextProcessState
            );
        }
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
        nextCombatState = null;
    }

    public async override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        Preprocess(combatContext);

        CombatCard playerOnCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
        CombatCard enemyOnCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();

        if (playerOnCombatCard != null && enemyOnCombatCard != null)
        {
            nextCombatState = await ProcessCombatResult(ResolveCombat(playerOnCombatCard, enemyOnCombatCard), combatContext);
            PostProcess(combatContext);
        }
    }

    CombatResult ResolveCombat(CombatCard playerCombatCard, CombatCard enemyCombatCard)
    {
        CombatTypes playerCombatCardType = playerCombatCard.GetCombatType();
        CombatTypes enemyCombatCardType = enemyCombatCard.GetCombatType();

        switch (playerCombatCardType)
        {
            case CombatTypes.Money:
                switch (enemyCombatCardType)
                {
                    case CombatTypes.Money:
                        return CombatResult.Draw;
                    case CombatTypes.Influence:
                        return CombatResult.PlayerWon;
                    case CombatTypes.Violence:
                        return CombatResult.EnemyWon;
                }
                break;
            case CombatTypes.Influence:
                switch (enemyCombatCardType)
                {
                    case CombatTypes.Money:
                        return CombatResult.EnemyWon;
                    case CombatTypes.Influence:
                        return CombatResult.Draw;
                    case CombatTypes.Violence:
                        return CombatResult.PlayerWon;
                }
                break;
            case CombatTypes.Violence:
                switch (enemyCombatCardType)
                {
                    case CombatTypes.Money:
                        return CombatResult.PlayerWon;
                    case CombatTypes.Influence:
                        return CombatResult.EnemyWon;
                    case CombatTypes.Violence:
                        return CombatResult.Draw;
                }
                break;
            default:
                break;
        }

        return CombatResult.Draw;
    }

    async Task<CombatState> ProcessCombatResult(CombatResult combatResult, CombatManager.CombatContext combatContext)
    {
        switch (combatResult)
        {
            case CombatResult.PlayerWon:
                return await ProcessPlayerWonState(combatContext);
            case CombatResult.EnemyWon:
                return await ProcessEnemyWonState(combatContext);
            case CombatResult.Draw:
                return new ResultDrawState();
            default:
                break;
        }

        return null;
    }

    async Task<CombatState> ProcessPlayerWonState(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        async Task KillEnemyCard(CombatManager.CombatContext combatContext)
        {
            CombatCard playerOnCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
            CombatCard enemyOnCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();

            if (playerOnCombatCard != null && enemyOnCombatCard != null)
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayAttackCard(playerOnCombatCard);   

                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACard(
                        cardToKill: enemyOnCombatCard,
                        attackerCard: playerOnCombatCard,
                        hasToFlipExplosions: false
                    );

                enemyDeckManager.DestroyCard(enemyOnCombatCard);
                GameObject.Destroy(combatContext.enemyOnCombatCard.gameObject);
                combatContext.enemyOnCombatCard = null;
            }
        }

        async Task KillEnemyCardsInTieZone(CombatManager.CombatContext combatContext)
        {
            await ForEachCardInTieZone(combatContext.GetEnemyCardInTieZoneContainers(), async (cardInTieZone) =>
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACardInTieZone(cardInTieZone);

                enemyDeckManager.DestroyCard(cardInTieZone);
                GameObject.Destroy(cardInTieZone.gameObject);
            });
        }

        async Task ReturnPlayerCardToDeck(CombatManager.CombatContext combatContext)
        {
            CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
            if (playerCombatCard != null)
            {
                playerDeckManager.AddCardToDeck(playerCombatCard);

                combatContext.playerOnCombatCard.transform.SetParent(
                    combatContext.playerDeck.transform.parent,
                    worldPositionStays: true
                );
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayReturnCardToDeck(
                        cardToReturn: playerCombatCard,
                        deckTransform: combatContext.playerDeck.transform
                    );
                combatContext.playerOnCombatCard.SetActive(false);
                combatContext.playerOnCombatCard = null;
            }
        }

        async Task ReturnPlayerCardsInTieZoneToDeck(CombatManager.CombatContext combatContext)
        {
            await ForEachCardInTieZone(combatContext.GetPlayerCardInTieZoneContainers(), async (cardInTieZone) =>
            {
                CombatCard playerCombatCard = cardInTieZone.GetComponent<CombatCard>();
                if (playerCombatCard != null)
                {
                    await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                        .PlayReturnCardToDeck(playerCombatCard, combatContext.playerDeck.transform);

                    playerDeckManager.ReturnCardFromTieZoneToDeck(playerCombatCard);
                    playerCombatCard.gameObject.SetActive(false);
                }
            });
        }

        await KillEnemyCard(combatContext);
        await KillEnemyCardsInTieZone(combatContext);
        await ReturnPlayerCardsInTieZoneToDeck(combatContext);
        await ReturnPlayerCardToDeck(combatContext);

        if (enemyDeckManager.GetNumberOfCardsInDeck() > 0)
        {
            return new PresentPlayerCardsState();
        }
        else
        {
            return new ResultWinState();
        }
    }

    async Task<CombatState> ProcessEnemyWonState(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        async Task KillPlayerCard(CombatManager.CombatContext combatContext)
        {
            CombatCard enemyOnCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
            CombatCard playerOnCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();

            if (enemyOnCombatCard != null && playerOnCombatCard != null)
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayAttackCard(enemyOnCombatCard);
            
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACard(
                        cardToKill: playerOnCombatCard,
                        attackerCard: enemyOnCombatCard,
                        hasToFlipExplosions: true
                    );

                playerDeckManager.DestroyCard(playerOnCombatCard);
                GameObject.Destroy(combatContext.playerOnCombatCard.gameObject);
                combatContext.playerOnCombatCard = null;
            }
        }

        async Task KillPlayerCardsInTieZone(CombatManager.CombatContext combatContext)
        {
            await ForEachCardInTieZone(combatContext.GetPlayerCardInTieZoneContainers(), async (cardInTieZone) =>
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACardInTieZone(cardInTieZone);
                
                playerDeckManager.DestroyCard(cardInTieZone);
                GameObject.Destroy(cardInTieZone.gameObject);
            });
        }

        async Task ReturnEnemyCardToDeck(CombatManager.CombatContext combatContext)
        {
            CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
            if (enemyCombatCard != null)
            {
                enemyDeckManager.AddCardToDeck(enemyCombatCard);

                combatContext.enemyOnCombatCard.transform.SetParent(
                    combatContext.enemyOnCombatCardOriginalPosition.transform.parent,
                    worldPositionStays: true
                );
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayReturnCardToDeck(
                        cardToReturn: enemyCombatCard,
                        deckTransform: combatContext.enemyOnCombatCardOriginalPosition
                    );
                combatContext.enemyOnCombatCard.SetActive(false);
                combatContext.enemyOnCombatCard = null;
            }
        }

        async Task ReturnEnemyCardsInTieZoneToDeck(CombatManager.CombatContext combatContext)
        {
            await ForEachCardInTieZone(combatContext.GetEnemyCardInTieZoneContainers(), async (cardInTieZone) =>
            {
                CombatCard enemyCombatCard = cardInTieZone.GetComponent<CombatCard>();
                if (enemyCombatCard != null)
                {
                    await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                        .PlayReturnCardToDeck(enemyCombatCard, combatContext.enemyOnCombatCardOriginalPosition);

                    enemyDeckManager.ReturnCardFromTieZoneToDeck(enemyCombatCard);
                    enemyCombatCard.gameObject.SetActive(false);
                }
            });
        }

        await KillPlayerCard(combatContext);
        await KillPlayerCardsInTieZone(combatContext);
        await ReturnEnemyCardsInTieZoneToDeck(combatContext);
        await ReturnEnemyCardToDeck(combatContext);

        if (playerDeckManager.GetNumberOfCardsInDeck() > 0 || playerDeckManager.GetNumberOfCardsInHand() > 0)
        {
            return new PresentPlayerCardsState();
        }
        else
        {
            return new ResultLoseState();
        }
    }

    async Task ForEachCardInTieZone(List<Transform> cardInTieZoneContainers, Func<CombatCard, Task> withCardInTieZone)
    {
        // Reverses the cards in tie zone containers to start killing from the latest added to the oldest one
        List<Transform> reversedCardInTieZoneContainers = new List<Transform>(cardInTieZoneContainers);
        reversedCardInTieZoneContainers.Reverse();

        await CombatUtils.ForEachCardInCardsContainerTask(reversedCardInTieZoneContainers, async (cardInTieZone) =>
        {
            CombatCard combatCard = cardInTieZone.GetComponent<CombatCard>();
            await withCardInTieZone(combatCard);
        });
    }
}
