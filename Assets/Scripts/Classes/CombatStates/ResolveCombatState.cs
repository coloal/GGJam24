using System.Collections;
using System.Collections.Generic;
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

    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        if (nextCombatState != null)
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().OverwriteCombatContext(combatContext);
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(nextCombatState);   
        }
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
        nextCombatState = null;
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        Preprocess(combatContext);

        CombatCard playerOnCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
        CombatCard enemyOnCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();

        if (playerOnCombatCard != null && enemyOnCombatCard != null)
        {
            nextCombatState = ProcessCombatResult(ResolveCombat(playerOnCombatCard, enemyOnCombatCard), ref combatContext);
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

    CombatState ProcessCombatResult(CombatResult combatResult, ref CombatV2Manager.CombatContext combatContext)
    {
        switch (combatResult)
        {
            case CombatResult.PlayerWon:
                return ProcessPlayerWonState(ref combatContext);
            case CombatResult.EnemyWon:
                return ProcessEnemyWonState(ref combatContext);
            case CombatResult.Draw:
                return new ResultDrawState();
            default:
                break;
        }

        return null;
    }

    CombatState ProcessPlayerWonState(ref CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        PlayerDeckManager playerDeckManager = GameManager.Instance.ProvideDeckManager();

        void KillEnemyCard(ref CombatV2Manager.CombatContext combatContext)
        {
            CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
            if (enemyCombatCard != null)
            {
                enemyDeckManager.DestroyCard(enemyCombatCard);
                GameObject.Destroy(combatContext.enemyOnCombatCard.gameObject);
                combatContext.enemyOnCombatCard = null;
            }
        }

        void ReturnPlayerCardToDeck(ref CombatV2Manager.CombatContext combatContext)
        {
            CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
            if (playerCombatCard != null)
            {
                playerDeckManager.ReturnCardFromHandToDeck(playerCombatCard);
                combatContext.playerOnCombatCard.SetActive(false);
                combatContext.playerOnCombatCard = null;
            }
        }

        KillEnemyCard(ref combatContext);
        ReturnPlayerCardToDeck(ref combatContext);

        if (enemyDeckManager.GetNumberOfCardsInDeck() > 0)
        {
            return new PresentPlayerCardsState();
        }
        else
        {
            return new ResultWinState();
        }
    }

    CombatState ProcessEnemyWonState(ref CombatV2Manager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = GameManager.Instance.ProvideDeckManager();
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        void KillPlayerCard(ref CombatV2Manager.CombatContext combatContext)
        {
            CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
            if (playerCombatCard != null)
            {
                playerDeckManager.DestroyCard(playerCombatCard);
                GameObject.Destroy(combatContext.playerOnCombatCard.gameObject);
                combatContext.playerOnCombatCard = null;
            }
        }

        void ReturnEnemyCardToDeck(ref CombatV2Manager.CombatContext combatContext)
        {
            CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
            if (enemyCombatCard != null)
            {
                enemyDeckManager.ReturnCardFromHandToDeck(enemyCombatCard);
                combatContext.enemyOnCombatCard.SetActive(false);
                combatContext.enemyOnCombatCard = null;
            }
        }

        KillPlayerCard(ref combatContext);
        ReturnEnemyCardToDeck(ref combatContext);

        if (playerDeckManager.GetNumberOfCardsInDeck() > 0 || playerDeckManager.GetNumberOfCardsInHand() > 0)
        {
            return new PresentPlayerCardsState();
        }
        else
        {
            return new ResultLoseState();
        }
    }
}
