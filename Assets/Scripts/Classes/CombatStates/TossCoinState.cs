using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TossCoinState : CombatState
{
    
    protected CoinFlipResult playerCoinChoice;
    protected CombatState nextCombatState;
    protected GameObject coinCardGameObject = null;
    
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        if (nextCombatState != null)
        {
            CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

            float secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
            if (nextCombatState is PresentPlayerCardsState)
            {
                secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatManager().timeForNextCombatRound;
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
        async void onSwipeLeft()
        {
            playerCoinChoice = CoinFlipResult.Heads;
            nextCombatState = await ProcessTossCoinResult(combatContext);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            PostProcess(combatContext);
        }

        async void onSwipeRight()
        {
            playerCoinChoice = CoinFlipResult.Tails;
            nextCombatState = await ProcessTossCoinResult(combatContext);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            PostProcess(combatContext);
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

    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
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
    }

    async Task<CombatState> ProcessTossCoinResult(CombatManager.CombatContext combatContext)
    {
        CoinFlipResult[] coinResults = { CoinFlipResult.Heads, CoinFlipResult.Tails };
        CoinFlipResult coinResult = coinResults[UnityEngine.Random.Range(0, coinResults.Length)];

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
            GameManager.Instance.ProvideBrainManager().PlayerWonCoin(false);
            await ProcessEnemyWonState(combatContext);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return null;
            }
            return new ResultLoseState();
        }

        //Victory
        else if (coinResult == playerCoinChoice && GetEnemyDeck().GetNumberOfCardsInDeck() <= 0)
        {
            GameManager.Instance.ProvideBrainManager().PlayerWonCoin(true);
            await ProcessPlayerWonState(combatContext);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return null;
            }
            return new ResultWinState();
        }

        //Next Round
        else
        {
            // The player wins
            if (coinResult == playerCoinChoice)
            {
                GameManager.Instance.ProvideBrainManager().PlayerWonCoin(true);
                await ProcessPlayerWonState(combatContext);
            }
            else
            {
                GameManager.Instance.ProvideBrainManager().PlayerWonCoin(false);
                await ProcessEnemyWonState(combatContext);
            }
            
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return null;
            }

            return new PresentPlayerCardsState();
        }
    }

    virtual protected async Task<CombatState>  ProcessPlayerWonState(CombatManager.CombatContext combatContext)
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
        
        return new PresentPlayerCardsState();
    }

    virtual protected async Task<CombatState> ProcessEnemyWonState(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();

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

        await KillPlayerCardsInTieZone(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        await ReturnEnemyCardsInTieZoneToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return null;
        }
        
        return new PresentPlayerCardsState();
    }


    protected async Task ForEachCardInTieZone(List<Transform> cardInTieZoneContainers, Func<CombatCard, Task> withCardInTieZone)
    {
        // Reverses the cards in tie zone containers to start killing from the latest added to the oldest one
        List<Transform> reversedCardInTieZoneContainers = new List<Transform>(cardInTieZoneContainers);
        reversedCardInTieZoneContainers.Reverse();

        await CombatUtils.ForEachCardInCardsContainerTask(reversedCardInTieZoneContainers, async (cardInTieZone) =>
        {
            CombatCard combatCard = cardInTieZone.GetComponent<CombatCard>();
            await withCardInTieZone(combatCard);
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
        });
    }
}
