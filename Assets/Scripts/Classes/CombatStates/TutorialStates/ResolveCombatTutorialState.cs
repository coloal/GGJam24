using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ResolveCombatTutorialState : ResolveCombatState
{
    
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        if (nextCombatState != null)
        {
            CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

            float secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
            if (nextCombatState is ResultDrawTutorialState)
            {
                secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForDrawRound;
            }
            else if (nextCombatState is ResultWinTutorialState || nextCombatState is ResultLoseTutorialState)
            {
                secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForCombatResultsRound;
            }

            CombatUtils.ProcessNextStateAfterSeconds(
                nextState: nextCombatState,
                seconds: secondsForNextProcessState
            );
        }
    }

    protected override async Task<CombatState> ProcessCombatResult(CombatResult combatResult, CombatManager.CombatContext combatContext)
    {
        switch (combatResult)
        {
            case CombatResult.PlayerWon:
                CombatTypes PlayerType = combatContext.playerOnCombatCard.GetComponent<CombatCard>().GetCombatType();
                GameManager.Instance.ProvideBrainManager().SetTypeLasWinnerCard(PlayerType);

                return await ProcessPlayerWonState(combatContext);

            case CombatResult.EnemyWon:
                CombatTypes EnemyType = combatContext.enemyOnCombatCard.GetComponent<CombatCard>().GetCombatType();
                GameManager.Instance.ProvideBrainManager().SetTypeLasWinnerCard(EnemyType);

                return await ProcessEnemyWonState(combatContext);

            case CombatResult.Draw:
                CombatTypes DrawType = combatContext.enemyOnCombatCard.GetComponent<CombatCard>().GetCombatType();
                GameManager.Instance.ProvideBrainManager().SetTypeLasWinnerCard(DrawType);

                return new ResultDrawTutorialState();
            default:
                break;
        }

        return null;
    }

    protected override async Task<CombatState> ProcessPlayerWonState(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        async Task KillEnemyCard(CombatManager.CombatContext combatContext)
        {
            CombatCard playerOnCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
            CombatCard enemyOnCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();

            if (playerOnCombatCard != null && enemyOnCombatCard != null)
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayAttackCard(playerOnCombatCard);
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACard(
                        cardToKill: enemyOnCombatCard,
                        attackerCard: playerOnCombatCard,
                        hasToFlipExplosions: false
                    );
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
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
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
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
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
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
                    if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                    {
                        return;
                    }
                    playerDeckManager.ReturnCardFromTieZoneToDeck(playerCombatCard);
                    playerCombatCard.gameObject.SetActive(false);
                }
            });
        }

        await KillEnemyCard(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        await KillEnemyCardsInTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }

        bool exited = false;
        Debug.LogError("Llegado al punto critico");
        TutorialManager.SceneTutorial.StartWinExplanation(() =>
        {
            
            exited = true;
            Debug.LogError("La cond es " + exited);
        });

        while (!exited)
        {
            Debug.LogError("Esperando, cond: " + exited);
            await Task.Yield();
        }
        

        Debug.LogError("Salido del punto critico");
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        { 
            return null;
        }
        await ReturnPlayerCardsInTieZoneToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        await ReturnPlayerCardToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }

        if (enemyDeckManager.GetNumberOfCardsInDeck() > 0)
        {
            return new PresentOnlyPlayerCardsTutorialState();
        }
        else
        {
            return new ResultWinTutorialState();
        }
    }

    protected override async Task<CombatState> ProcessEnemyWonState(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();

        async Task KillPlayerCard(CombatManager.CombatContext combatContext)
        {
            CombatCard enemyOnCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
            CombatCard playerOnCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();

            if (enemyOnCombatCard != null && playerOnCombatCard != null)
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayAttackCard(enemyOnCombatCard);
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACard(
                        cardToKill: playerOnCombatCard,
                        attackerCard: enemyOnCombatCard,
                        hasToFlipExplosions: true
                    );
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
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
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
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
                        deckTransform: combatContext.enemyDeck.transform
                    );
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
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
                    if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                    {
                        return;
                    }
                    enemyDeckManager.ReturnCardFromTieZoneToDeck(enemyCombatCard);
                    enemyCombatCard.gameObject.SetActive(false);
                }
            });
        }

        await KillPlayerCard(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        await KillPlayerCardsInTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        Task WaitForConversation = new Task(() => { });
        TutorialManager.SceneTutorial.StartLoseExplanation(() =>
        {
            WaitForConversation.Start();
        });
        await WaitForConversation;
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        await ReturnEnemyCardsInTieZoneToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        await ReturnEnemyCardToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }

        if (playerDeckManager.GetNumberOfCardsInDeck() > 0 || playerDeckManager.GetNumberOfCardsInHand() > 0)
        {
            return new PresentOnlyPlayerCardsTutorialState();
        }
        else
        {
            return new ResultLoseTutorialState();
        }
    }
}
