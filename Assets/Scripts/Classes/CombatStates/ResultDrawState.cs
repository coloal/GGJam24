using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDrawState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().OverwriteCombatContext(combatContext);

        //Al enemigo o al Player le quedan cartas
        if (CombatSceneManager.Instance.ProvideEnemyDeckManager().GetNumberOfCardsInDeck() > 0
            && CombatSceneManager.Instance.ProvidePlayerDeckManager().GetNumberOfCardsInHand() > 0 )
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickEnemyCardState());
        }
        else
        {
            CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new TossCoinState());
        }
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        SendPlayerCombatCardToTieZone(combatContext);
        SendEnemyCombatCardToTieZone(combatContext);
        PostProcess(combatContext);
    }

    void SendPlayerCombatCardToTieZone(CombatV2Manager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();

        if (playerCombatCard != null)
        {
            playerDeckManager.AddCardToTieZone(playerCombatCard);
            playerCombatCard.gameObject.transform.SetParent(
                combatContext.playerTieZone,
                worldPositionStays: false
            );
        }
    }

    void SendEnemyCombatCardToTieZone(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();


        if (enemyCombatCard != null)
        {
            enemyDeckManager.AddCardToTieZone(enemyCombatCard);
            enemyCombatCard.gameObject.transform.SetParent(
                combatContext.enemyTieZone,
                worldPositionStays: false
            );
        }
    }
}
