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
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

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

        void KillEnemyCardsInTieZone(CombatV2Manager.CombatContext combatContext)
        {
            foreach (Transform cardInTieZone in combatContext.enemyTieZone)
            {
                CombatCard enemyCombatCard = cardInTieZone.GetComponent<CombatCard>();
                if (enemyCombatCard != null)
                {
                    enemyDeckManager.DestroyCard(enemyCombatCard);
                    GameObject.Destroy(enemyCombatCard.gameObject);
                }
            }
        }

        void ReturnPlayerCardToDeck(ref CombatV2Manager.CombatContext combatContext)
        {
            CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();
            if (playerCombatCard != null)
            {
                playerDeckManager.AddCardToDeck(playerCombatCard);
                combatContext.playerOnCombatCard.SetActive(false);
                combatContext.playerOnCombatCard = null;
            }
        }

        void ReturnPlayerCardsInTieZoneToDeck(CombatV2Manager.CombatContext combatContext)
        {
            foreach (Transform cardInTieZone in combatContext.playerTieZone)
            {
                CombatCard playerCombatCard = cardInTieZone.GetComponent<CombatCard>();
                if (playerCombatCard != null)
                {
                    playerDeckManager.ReturnCardFromTieZoneToDeck(playerCombatCard);
                    playerCombatCard.gameObject.SetActive(false);
                }
            }
        }

        KillEnemyCard(ref combatContext);
        KillEnemyCardsInTieZone(combatContext);
        ReturnPlayerCardToDeck(ref combatContext);
        ReturnPlayerCardsInTieZoneToDeck(combatContext);

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
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
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

        void KillPlayerCardsInTieZone(CombatV2Manager.CombatContext combatContext)
        {
            foreach (Transform cardInTieZone in combatContext.playerTieZone)
            {
                CombatCard playerCombatCard = cardInTieZone.GetComponent<CombatCard>();
                if (playerCombatCard != null)
                {
                    playerDeckManager.DestroyCard(playerCombatCard);
                    GameObject.Destroy(playerCombatCard.gameObject);
                }
            }
        }

        void ReturnEnemyCardToDeck(ref CombatV2Manager.CombatContext combatContext)
        {
            CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();
            if (enemyCombatCard != null)
            {
                enemyDeckManager.AddCardToDeck(enemyCombatCard);
                combatContext.enemyOnCombatCard.SetActive(false);
                combatContext.enemyOnCombatCard = null;
            }
        }

        void ReturnEnemyCardsInTieZoneToDeck(CombatV2Manager.CombatContext combatContext)
        {
            foreach (Transform cardInTieZone in combatContext.enemyTieZone)
            {
                CombatCard enemyCombatCard = cardInTieZone.GetComponent<CombatCard>();
                if (enemyCombatCard != null)
                {
                    enemyDeckManager.ReturnCardFromTieZoneToDeck(enemyCombatCard);
                    enemyCombatCard.gameObject.SetActive(false);
                }
            }
        }

        KillPlayerCard(ref combatContext);
        KillPlayerCardsInTieZone(combatContext);
        ReturnEnemyCardToDeck(ref combatContext);
        ReturnEnemyCardsInTieZoneToDeck(combatContext);

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
