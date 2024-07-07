using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TossCoinTutorialState : TossCoinState
{
    private enum CoinResult
    {
        Heads,
        Tails
    }
    
    CoinResult playerCoinChoice;
    CombatState nextCombatState;
    GameObject coinCardGameObject = null;
    
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
                playerCoinChoice = CoinResult.Heads;
                nextCombatState = await ProcessTossCoinResult(combatContext);
                PostProcess(combatContext);
            });
           
        }
        void onSwipeRight()
        {
            TutorialManager.SceneTutorial.StartCoinExplanation(async () =>
            {
                playerCoinChoice = CoinResult.Tails;
                nextCombatState = await ProcessTossCoinResult(combatContext);
                PostProcess(combatContext);
            });
        }
        coinCardGameObject = CombatSceneManager.Instance.ProvideCombatManager()
            .InstantiateCoinCardGameObject();
        CoinCard coinCard = coinCardGameObject.GetComponent<CoinCard>();
        if (coinCard != null)
        {
            coinCard.SetUpCard(onSwipeLeft, onSwipeRight);
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
        CoinResult[] coinResults = { CoinResult.Heads, CoinResult.Tails };
        CoinResult coinResult = playerCoinChoice;

        int playerTotalCards = CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInDeck()
            + CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInHand();

        //Game Over
        if (coinResult != playerCoinChoice && playerTotalCards <= 0)
        {
            await ProcessEnemyWonState(combatContext);
            return new ResultLoseTutorialState();
        }

        //Victory
        else if (coinResult == playerCoinChoice && CombatSceneManager.Instance.ProvideEnemyDeckManager().GetNumberOfCardsInDeck() <= 0)
        {
            await ProcessPlayerWonState(combatContext);
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
            return new PresentPlayerCardsState();
        }
    }

    async Task<CombatState>  ProcessPlayerWonState(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

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

        await KillEnemyCardsInTieZone(combatContext);
        await ReturnPlayerCardsInTieZoneToDeck(combatContext);
        
        return new PresentPlayerCardsState();


    }

    async Task<CombatState> ProcessEnemyWonState(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

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
