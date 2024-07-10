using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTutorialState : StartCombatState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PresentPlayerCardsTutorialState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPresentEnemyCards
        );
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        SetUpPlayerDeck(combatContext);
        SetUpEnemyDeck(combatContext);
        TutorialManager.SceneTutorial.StartInitialConversation(()=>PostProcess(combatContext));
    }

    protected override void SetUpPlayerDeck(CombatManager.CombatContext combatContext)
    {
        PlayerDeckManager playerDeckManager = CombatSceneManager.Instance.ProvidePlayerDeckManager();

        List<CombatCardTemplate> playerSavedDeck = TutorialManager.SceneTutorial.TutorialInfo.TutorialDeck;
        playerSavedDeck.ForEach((combatCardData) =>
        {
            CombatCard combatCard = CombatSceneManager.Instance
                .ProvideCombatManager()
                .InstantiateCombatCardGameObject()
                .GetComponent<CombatCard>();
            if (combatCard != null)
            {
                combatCard.gameObject.SetActive(false);
                combatCard.transform.SetParent(
                    combatContext.combatContainer.transform,
                    worldPositionStays: false
                );
                combatCard.SetDataCard(combatCardData);
                playerDeckManager.AddCardToDeck(combatCard);
            }
        });
    }

    protected override void SetUpEnemyDeck(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = TutorialManager.SceneTutorial.EnemyDeck;

        List<CombatCardTemplate> enemyDeck = CombatSceneManager.Instance.ProvideEnemyData().CombatCards;
        enemyDeck.ForEach((combatCardData) =>
        {
            CombatCard combatCard = CombatSceneManager.Instance
                .ProvideCombatManager()
                .InstantiateCombatCardGameObject()
                .GetComponent<CombatCard>();
            if (combatCard != null)
            {
                combatCard.gameObject.SetActive(false);
                combatCard.transform.SetParent(
                    combatContext.combatContainer.transform,
                    worldPositionStays: false
                );
                combatCard.SetDataCard(combatCardData);
                enemyDeckManager.AddCardToDeck(combatCard);
            }
        });
    }

}
