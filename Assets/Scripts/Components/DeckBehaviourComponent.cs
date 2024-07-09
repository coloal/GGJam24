using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckBehaviourComponent : MonoBehaviour
{
    [Header("Deck configurations")]
    [SerializeField] private TextMeshProUGUI textCardsLeft;

    int numberOfCardsLeft;
    
    public void InitDeck(BaseDeckManager deckManager)
    {
        numberOfCardsLeft = deckManager.GetNumberOfCardsInDeck();
        textCardsLeft.text = numberOfCardsLeft.ToString();
    }

    public void AddCardToDeck()
    {
        numberOfCardsLeft++;
        textCardsLeft.text = numberOfCardsLeft.ToString();
    }

    public void DrawCardFromDeck()
    {
        numberOfCardsLeft--;
        textCardsLeft.text = numberOfCardsLeft.ToString();
    }
}
