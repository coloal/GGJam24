using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static CombatManager;

public class ResultWinTutorialState : ResultWinState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        TutorialManager.SceneTutorial.StartEndExplanation(() =>
        {
            combatContext.CleanEnemyCardsContainer();
            combatContext.DeactivateEnemyCardsContainer();

            CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);
            GameManager.Instance.ProvideBrainManager().IsTutorial = false;
            //TODO: return to history
            GameManager.Instance.EndCombat(TurnResult.COMBAT_WON_CAPTURE);
        });
    }


    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        await ReturnPlayerCardsFromHandToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }

        bool exited = false;
        TutorialManager.SceneTutorial.StartBattleResultExplanation(() =>
        {
            exited = true;
        });

        while (!exited)
        {
            await Task.Yield();
        }

        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }

        TutorialManager.SceneTutorial.StartPickWinCardExplanation(async () =>
        {
            SetEnemyCardsToChooseFrom(combatContext);
            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayKillEnemyDeck();
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }

            GameManager.Instance.ProvideSoundManager().EndCombat();
            await ShowEnemyCardsToChooseFrom();
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            TutorialManager.SceneTutorial.PickWinCard();
        });
    }

    void SetEnemyCardsToChooseFrom(CombatManager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = GetEnemyDeck();

        List<CombatCardTemplate> enemyCombatCards = CombatSceneManager.Instance.ProvideEnemyData().CombatCards;
        List<GameObject> combatCards = new List<GameObject>();

        //Instantiate all cards of the enemyDeck
        foreach (CombatCardTemplate card in enemyCombatCards)
        {
            GameObject combatCard =
                CombatSceneManager.Instance.ProvideCombatManager().InstantiateCombatCardGameObject();
            CombatCard combatCardComponent = combatCard.GetComponent<CombatCard>();
            if (combatCardComponent != null)
            {
                combatCardComponent.SetDataCard(card);
                combatCards.Add(combatCard);
            }
        }

        int maxAllowedEnemyCards = CombatSceneManager.Instance.ProvideCombatManager().GetMaxAllowedEnemyCards();
        int maxCardsPerRow = maxAllowedEnemyCards / CombatUtils.NUMBER_OF_ENEMY_CARDS_TYPE_HINTS_ROWS;

        int cardsLeftToFill = 0;
        if (combatCards.Count <= maxCardsPerRow)
        {
            cardsLeftToFill = maxCardsPerRow - combatCards.Count;
        }
        else if (combatCards.Count > maxCardsPerRow && combatCards.Count <= maxAllowedEnemyCards)
        {
            cardsLeftToFill = maxAllowedEnemyCards - combatCards.Count;
        }

        //ASIGNAR Click Action

        foreach (GameObject EnemyCard in combatCards)
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                EnemyCard.GetComponent<InteractiveCombatCardComponent>();
            CombatCard enemyCombatCard = EnemyCard.GetComponent<CombatCard>();

            if (interactiveCombatCardComponent != null && enemyCombatCard != null)
            {
                interactiveCombatCardComponent.SetOnClickAction(() => {
                    DisableAllCardsInteractions(combatCards);
                    TutorialManager.SceneTutorial.UnBlockScreen(() => {
                        PickAEnemyCard(combatContext, enemyCombatCard);
                    });
                });
                interactiveCombatCardComponent.EnableInteractiveComponent();
                enemyCombatCard.FlipCardUp();
            }
        }

        // Fills the list of card type hints with dummies to fill the empty gaps in the row
        // (a workaround for the default HorizontalLayout component behaviour)
        for (int i = 0; i < cardsLeftToFill; i++)
        {
            GameObject emptyCardDummy = CombatSceneManager.Instance.ProvideCombatManager().InstantiateEmptyCardDummyGameObject();
            combatCards.Add(emptyCardDummy);
        }

        // Case when there's only 1 row of card type hints to fill
        if (combatCards.Count <= maxCardsPerRow)
        {
            combatContext.enemyCardsRow0.SetActive(true);
            combatContext.enemyCardsRow1.SetActive(false);
            foreach (GameObject enemyCard in combatCards)
            {
                enemyCard.transform.SetParent(
                    combatContext.enemyCardsRow0.transform,
                    worldPositionStays: false
                );
            }
        }
        // Case when there's more than 1 row of card type hints to fill
        else if (combatCards.Count > maxCardsPerRow && combatCards.Count <= maxAllowedEnemyCards)
        {
            combatContext.enemyCardsRow0.SetActive(true);
            combatContext.enemyCardsRow1.SetActive(true);
            int cardIndex;
            // Fills the row 0 for the enemy card type hints
            for (cardIndex = 0; cardIndex < maxCardsPerRow; cardIndex++)
            {
                combatCards[cardIndex].transform.SetParent(
                    combatContext.enemyCardsRow0.transform,
                    worldPositionStays: false
                );
            }
            // Fills the row 1 for the enemy card type hints
            for (int i = cardIndex; i < maxAllowedEnemyCards; i++)
            {
                combatCards[i].transform.SetParent(
                    combatContext.enemyCardsRow1.transform,
                    worldPositionStays: false
                );
            }
        }
    }

    void DisableAllCardsInteractions(List<GameObject> cardsToDisableInteractions)
    {
        foreach (GameObject cardToDisableInteraction in cardsToDisableInteractions)
        {
            InteractiveCombatCardComponent interactiveCombatCardComponent =
                cardToDisableInteraction.GetComponent<InteractiveCombatCardComponent>();
            if (interactiveCombatCardComponent != null)
            {
                interactiveCombatCardComponent.DisableInteractiveComponent();
            }
        }
    }

    protected override async Task ShowEnemyCardsToChooseFrom()
    {
        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayMoveEnemyCardsTypesHints(
                origin: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintOriginPosition(),
                destination: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintDestinationPosition()
            );
    }

    protected async override Task HideEnemyCardsToChooseFrom()
    {
        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayMoveEnemyCardsTypesHints(
                origin: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintDestinationPosition(),
                destination: TutorialManager.SceneTutorial.GetEnemyCardsTypesHintOriginPosition()
            );
    }

    protected override async void PickAEnemyCard(CombatManager.CombatContext combatContext, CombatCard pickedCombatCard)
    {

        foreach (Transform enemyCard in combatContext.GetEnemyCards())
        {
            if (enemyCard.gameObject == pickedCombatCard.gameObject)
            {
                GameObject emptyCardDummy = CombatSceneManager.Instance.ProvideCombatManager().InstantiateEmptyCardDummyGameObject();
                emptyCardDummy.transform.SetParent(enemyCard.transform.parent);
                emptyCardDummy.transform.SetSiblingIndex(
                    pickedCombatCard.transform.GetSiblingIndex()
                );
            }
        }
        CombatSceneManager.Instance.ProvidePlayerDeckManager()
            .AddCardToDeck(pickedCombatCard);
        pickedCombatCard.transform.SetParent(
            combatContext.playerDeck.transform.parent,
            worldPositionStays: true
        );

        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayReturnCardToDeck(
                cardToReturn: pickedCombatCard,
                deckTransform: combatContext.playerDeck.transform
            );
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        pickedCombatCard.gameObject.SetActive(false);

        // Hide all other cards
        if (combatContext.enemyCardsRow0.transform.childCount > 0)
        {
            await HideEnemyCardsToChooseFrom();
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
        }

        PostProcess(combatContext);
    }
}
