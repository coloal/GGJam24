using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public override async void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        await SendPlayerCombatCardToTieZone(combatContext);
        await SendEnemyCombatCardToTieZone(combatContext);
        PostProcess(combatContext);
    }

    async Task SendPlayerCombatCardToTieZone(CombatV2Manager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();
        CombatCard playerCombatCard = combatContext.playerOnCombatCard.GetComponent<CombatCard>();

        if (playerCombatCard != null)
        {
            Transform firstAvailablePositionInTieZone = combatContext
                .GetPlayerCardInTieZoneContainers()
                .Find((cardInTieZoneContainer) => cardInTieZoneContainer.childCount == 0);

            playerDeckManager.AddCardToTieZone(playerCombatCard);
            playerCombatCard.gameObject.transform.SetParent(
                firstAvailablePositionInTieZone,
                worldPositionStays: true
            );

            combatContext.playerOnCombatCard = null;
            
            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayMoveCardToTieZone(
                    cardToMove: playerCombatCard,
                    firstAvailablePositionInTieZone
                );
        }
    }

    async Task SendEnemyCombatCardToTieZone(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();
        CombatCard enemyCombatCard = combatContext.enemyOnCombatCard.GetComponent<CombatCard>();


        if (enemyCombatCard != null)
        {
            Transform firstAvailablePositionInTieZone = combatContext
                .GetEnemyCardInTieZoneContainers()
                .Find((cardInTieZoneContainer) => cardInTieZoneContainer.childCount == 0);

            enemyDeckManager.AddCardToTieZone(enemyCombatCard);
            enemyCombatCard.gameObject.transform.SetParent(
                firstAvailablePositionInTieZone,
                worldPositionStays: true
            );

            combatContext.enemyOnCombatCard = null;

            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayMoveCardToTieZone(
                    cardToMove: enemyCombatCard,
                    firstAvailablePositionInTieZone
                );
        }
    }
}
