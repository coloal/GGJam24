using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TossCoinState : CombatState
{

    private GameObject CoinCard;
    private int PlayerCoinChoice;
    private CombatV2Manager.CombatContext CombatContext;
    
    public override async void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        /* Cara = 0 / Cruz = 1 */
        int CoinResult = UnityEngine.Random.Range(0, 2);
        Debug.Log("Ha salido: "+CoinResult);

        CoinCard.GetComponent<CoinCard>().ShowCoinResult(CoinResult);

        
        GameUtils.CreateTemporizer(() => {
            CoinCard.GetComponent<CoinCard>().EnableCard(false);
        }, 3, CombatSceneManager.Instance);/**/


        int playerTotalCards = CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInDeck()
            + CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInHand();
        CombatState nextCombatState = null;
        float secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatV2Manager().timeForNextCombatRound;
        
        //Game Over
        if (CoinResult != PlayerCoinChoice && playerTotalCards <= 0)
        {
            Debug.Log("El player ha perdido");

            await ProcessEnemyWonState(combatContext);

            nextCombatState = new ResultLoseState();
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatV2Manager().timeForCombatResultsRound;
        }

        //Victory
        else if (CoinResult == PlayerCoinChoice && CombatSceneManager.Instance.ProvideEnemyDeckManager().GetNumberOfCardsInDeck() <= 0)
        {
            Debug.Log("El player ha ganado");

            await ProcessPlayerWonState(combatContext);

            nextCombatState = new ResultWinState();
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatV2Manager().timeForCombatResultsRound;
        }

        //Next Round
        else
        {
            //El player gana
            if (CoinResult == PlayerCoinChoice)
            {
                await ProcessPlayerWonState(combatContext);
            }
            else
            {
                await ProcessEnemyWonState(combatContext);
            }
            Debug.Log("Next round");
            nextCombatState = new PresentPlayerCardsState();
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatV2Manager().timeForNextCombatRound;
        }

        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: nextCombatState,
            seconds: secondsForNextProcessState
        );
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        //Spawnea Moneda
        CoinCard = CombatSceneManager.Instance.ProvideCoinCardGO();

        CoinCard.GetComponent<CoinCard>().SetActions(this);
        CoinCard.GetComponent<CoinCard>().EnableCard(true);

        CombatContext = combatContext;

    }

    //Elige Cara 0
    public void OnSwipeLeft() 
    {
        Debug.Log("Se ha elegido Cara");

        PlayerCoinChoice = 0;
        PostProcess(CombatContext);
    }

    //Elige Cruz 1
    public void OnSwipeRight()
    {
        Debug.Log("Se ha elegido Cruz");

        PlayerCoinChoice = 1;
        PostProcess(CombatContext);
    }

    async Task<CombatState>  ProcessPlayerWonState(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        async Task KillEnemyCardsInTieZone(CombatV2Manager.CombatContext combatContext)
        {
            await ForEachCardInTieZone(combatContext.GetEnemyCardInTieZoneContainers(), async (cardInTieZone) =>
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACardInTieZone(cardInTieZone);

                enemyDeckManager.DestroyCard(cardInTieZone);
                GameObject.Destroy(cardInTieZone.gameObject);
            });
        }

        async Task ReturnPlayerCardsInTieZoneToDeck(CombatV2Manager.CombatContext combatContext)
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

    async Task<CombatState> ProcessEnemyWonState(CombatV2Manager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        async Task KillPlayerCardsInTieZone(CombatV2Manager.CombatContext combatContext)
        {
            await ForEachCardInTieZone(combatContext.GetPlayerCardInTieZoneContainers(), async (cardInTieZone) =>
            {
                await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                    .PlayKillACardInTieZone(cardInTieZone);

                playerDeckManager.DestroyCard(cardInTieZone);
                GameObject.Destroy(cardInTieZone.gameObject);
            });
        }

        async Task ReturnEnemyCardsInTieZoneToDeck(CombatV2Manager.CombatContext combatContext)
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
