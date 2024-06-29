using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TossCoinState : CombatState
{

    private GameObject CoinCard;
    private int PlayerCoinChoice;
    private CombatV2Manager.CombatContext CombatContext;
    
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        /* Cara = 0 / Cruz = 1 */
        int CoinResult = Random.Range(0, 2);
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
            nextCombatState = new ResultLoseState();
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatV2Manager().timeForCombatResultsRound;
        }

        //Victory
        else if (CoinResult == PlayerCoinChoice && CombatSceneManager.Instance.ProvideEnemyDeckManager().GetNumberOfCardsInDeck() <= 0)
        {
            Debug.Log("El player ha ganado");
            nextCombatState = new ResultWinState();
            secondsForNextProcessState = CombatSceneManager.Instance.ProvideCombatV2Manager().timeForCombatResultsRound;
        }

        //Next Round
        else
        {
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

        //Debug
        /*
        OnSwipeLeft();
        /*/
        //OnSwipeRight();
        /**/

        //PostProcess(combatContext);
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

}
