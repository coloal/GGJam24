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
            nextCombatState = ProcessCombatResult(ResolveCombat(playerOnCombatCard, enemyOnCombatCard));
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

    CombatState ProcessCombatResult(CombatResult combatResult)
    {
        int enemyCardsLeftOnDeck = CombatSceneManager.Instance.ProvideEnemyDeckManager().GetNumberOfCardsInDeck();
        int playerCardsLeftOnDeck = GameManager.Instance.ProvideDeckManager().GetNumberOfCardsInDeck();

        switch (combatResult)
        {
            case CombatResult.PlayerWon:
                return ProcessPlayerWonState(enemyCardsLeftOnDeck);
            case CombatResult.EnemyWon:
                return ProcessEnemyWonState(playerCardsLeftOnDeck);
            case CombatResult.Draw:
                return new ResultDrawState();
            default:
                break;
        }

        return null;
    }

    CombatState ProcessPlayerWonState(int enemyCardsLeftOnDeck)
    {
        if (enemyCardsLeftOnDeck > 0)
        {
            return new PickEnemyCardState();
        }
        else
        {
            return new ResultWinState();
        }
    }

    CombatState ProcessEnemyWonState(int playerCardsLeftOnDeck)
    {
        if (playerCardsLeftOnDeck > 0)
        {
            return new PickEnemyCardState();
        }
        else
        {
            return new ResultLoseState();
        }
    }
}
