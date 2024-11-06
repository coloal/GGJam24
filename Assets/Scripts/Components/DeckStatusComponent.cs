using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckStatusComponent : MonoBehaviour
{
    [Header("Deck information")]
    [SerializeField] private DeckCardCounterComponent moneyCardsCounter;
    [SerializeField] private DeckCardCounterComponent influenceCardsCounter;
    [SerializeField] private DeckCardCounterComponent violenceCardsCounter;

    private ClickableCardComponent clickableCardComponent;

    private bool areCardsCurrentlyVisible;
    private bool areInteractionsActive;

    void Awake()
    {
        clickableCardComponent = GetComponent<ClickableCardComponent>();
    }

    void Start()
    {
        CombatSceneManager.Instance.ProvidePlayerDeckManager().onDeckStateUpdate += OnPlayerDeckUpdate;

        areCardsCurrentlyVisible = false;
        areInteractionsActive = false;
        SetOnClickAction();
    }

    void OnDisable()
    {
        CombatSceneManager.Instance.ProvidePlayerDeckManager().onDeckStateUpdate -= OnPlayerDeckUpdate;
    }

    void OnPlayerDeckUpdate(List<CombatCard> updatedPlayerDeck)
    {
        int moneyCards = 0;
        int influenceCards = 0;
        int violenceCards = 0;

        foreach (CombatCard combatCard in updatedPlayerDeck)
        {    
            switch (combatCard.GetCombatType())
            {
                case CombatTypes.Money:
                    moneyCards++;
                    break;
                case CombatTypes.Influence:
                    influenceCards++;
                    break;
                case CombatTypes.Violence:
                    violenceCards++;
                    break;
                default:
                    break;
            }
        }

        moneyCardsCounter.SetNumberOfCardsLeft(moneyCards);
        influenceCardsCounter.SetNumberOfCardsLeft(influenceCards);
        violenceCardsCounter.SetNumberOfCardsLeft(violenceCards);
    }

    void SetOnClickAction()
    {
        clickableCardComponent.OnClickActions.Clear();
        clickableCardComponent.OnClickActions.Add(() => {
            if (clickableCardComponent.enabled)
            {
                ToggleCardsLeftVisibility();
            }
        });
    }

    void ToggleCardsLeftVisibility()
    {
        if (!areCardsCurrentlyVisible)
        {
            ShowCardsLeft();   
        } 
        else
        {
            HideCardsLeft();
        }

        areCardsCurrentlyVisible = !areCardsCurrentlyVisible;
    }

    void ShowCardsLeft()
    {
        moneyCardsCounter.ShowCardsLeft();
        influenceCardsCounter.ShowCardsLeft();
        violenceCardsCounter.ShowCardsLeft();
    }

    void HideCardsLeft()
    {
        violenceCardsCounter.HideCardsLeft();
        influenceCardsCounter.HideCardsLeft();
        moneyCardsCounter.HideCardsLeft();
    }

    public void EnableInteractions()
    {
        if (clickableCardComponent != null)
        {
            clickableCardComponent.enabled = true;
            areInteractionsActive = true;
        }
    }

    public void DisableInteractions()
    {
        if (clickableCardComponent != null)
        {
            clickableCardComponent.enabled = false;
            areInteractionsActive = false;
            HideCardsLeft();
        }
    }

    public void ToggleInteractions()
    {
        if (!areInteractionsActive)
        {
            EnableInteractions();
        }
        else
        {
            DisableInteractions();
        }
    }
}
