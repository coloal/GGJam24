using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TossCoinTutorialState : TossCoinState
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
                if (nextCombatState is PresentPlayerCardsTutorialState)
                {
                    secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
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

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
        void onSwipeLeft()
        {
            TutorialManager.SceneTutorial.StartCoinExplanation(async () =>
            {
                playerCoinChoice = CoinFlipResult.Heads;
                nextCombatState = await ProcessTossCoinResult(combatContext);
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
                PostProcess(combatContext);
            });
           
        }
        void onSwipeRight()
        {
            TutorialManager.SceneTutorial.StartCoinExplanation(async () =>
            {
                playerCoinChoice = CoinFlipResult.Tails;
                nextCombatState = await ProcessTossCoinResult(combatContext);
                if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                {
                    return;
                }
                PostProcess(combatContext);
            });
        }
        coinCardGameObject = CombatSceneManager.Instance.ProvideCombatManager()
            .InstantiateCoinCardGameObject();
        CoinCard coinCard = coinCardGameObject.GetComponent<CoinCard>();
        if (coinCard != null)
        {
            coinCard.SetUpCard(
                onSwipeLeft,
                onSwipeLeftEscapeZone: (coinCard) => { coinCard.SetImageAsCoinHeads(); },
                onSwipeRight,
                onSwipeRightEscapeZone: (coinCard) => { coinCard.SetImageAsCoinTails(); }
            );
        }
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        TutorialManager.SceneTutorial.StartCoinCardExplanation(async () =>
        {
            Preprocess(combatContext);
            if (coinCardGameObject != null)
            {
                CoinCard coinCard = coinCardGameObject.GetComponent<CoinCard>();
                if (coinCard != null)
                {
                    await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                        .PlayShowCoinCard(coinCard);
                }
            }
        });
        
    }

    async Task<CombatState> ProcessTossCoinResult(CombatManager.CombatContext combatContext)
    {
        CoinFlipResult coinResult = playerCoinChoice;

        int playerTotalCards = CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInDeck()
            + CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInHand();

        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayTossCoin(
                CombatSceneManager.Instance.ProvideCombatManager().GetCombatCoin(),
                coinResult
            );
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }

        //Game Over
        if (coinResult != playerCoinChoice && playerTotalCards <= 0)
        {
            await ProcessEnemyWonState(combatContext);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return null;
            }
            return new ResultLoseTutorialState();
        }

        //Victory
        else if (coinResult == playerCoinChoice && GetEnemyDeck().GetNumberOfCardsInDeck() <= 0)
        {
            await ProcessPlayerWonState(combatContext);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return null;
            }
            return new ResultWinTutorialState();
        }

        //Next Round
        else
        {
            // The player wins
            if (coinResult == playerCoinChoice)
            {
                await ProcessPlayerWonState(combatContext);
            }
            else
            {
                await ProcessEnemyWonState(combatContext);
            }
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return null;
            }

            return new PresentPlayerCardsState();
        }
    }

    protected override async Task<CombatState>  ProcessPlayerWonState(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

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

        await KillEnemyCardsInTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        await ReturnPlayerCardsInTieZoneToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }

        return new PresentPlayerCardsTutorialState();


    }

    protected override async Task<CombatState> ProcessEnemyWonState(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();

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

        await KillPlayerCardsInTieZone(combatContext);
        await ReturnEnemyCardsInTieZoneToDeck(combatContext);
        
        return new PresentPlayerCardsState();
    }
}
