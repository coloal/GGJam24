using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static CombatManager;

public class ResultWinState : CombatState
{

    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        combatContext.CleanEnemyCardsContainer();
        combatContext.DeactivateEnemyCardsContainer();

        CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);

        //TODO: return to history
        GameManager.Instance.EndCombat(TurnResult.COMBAT_WON_CAPTURE);
    }

    public override void Preprocess(CombatManager.CombatContext combatContext)
    {
        combatContext.ActivateEnemyCardsContainer();
    }

    public override async void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        await ReturnPlayerCardsFromHandToDeck(combatContext);
        if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
        {
            return;
        }
        if(CombatSceneManager.Instance.ProvideEnemyData().OnWinConversation.Any())
        {
            DialogManager.SceneDialog.CreateDialog(CombatSceneManager.Instance.ProvideEnemyData().OnWinConversation, async () =>
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
            });
        }
        else
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
        }
        
    }

    protected async Task ReturnPlayerCardsFromHandToDeck(CombatManager.CombatContext combatContext)
    {
        List<Transform> cardInHandContainers = combatContext.GetPlayerCardInHandContainers();
        for (int i = cardInHandContainers.Count - 1; i >= 0; i--)
        {
            if (cardInHandContainers[i].childCount > 0)
            {
                CombatCard combatCard = cardInHandContainers[i].GetChild(0).GetComponent<CombatCard>();
                if (combatCard != null)
                {
                    combatCard.transform.SetParent(
                        combatContext.playerDeck.transform.parent,
                        worldPositionStays: true
                    );
                    await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                        .PlayReturnCardToDeck(
                            cardToReturn: combatCard,
                            combatContext.playerDeck.transform
                        );
                    if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
                    {
                        return;
                    }

                    combatCard.gameObject.SetActive(false);
                }
            }
        }
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
                    PickAEnemyCard(combatContext, enemyCombatCard);
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

    protected async void PickAEnemyCard(CombatManager.CombatContext combatContext, CombatCard pickedCombatCard) 
    {
        //Add choosen enemy card to the player's deck
        GameManager.Instance.ProvideInventoryManager().AddCombatCardToVault(pickedCombatCard.GetCardData());

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

    virtual protected async Task ShowEnemyCardsToChooseFrom()
    {
        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayShowEnemyCardsToChooseFrom();
    }

    virtual protected async Task HideEnemyCardsToChooseFrom()
    {
        await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
            .PlayHideEnemyCardsToChooseFrom();
    }
}
