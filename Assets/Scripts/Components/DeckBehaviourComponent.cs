using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckBehaviourComponent : MonoBehaviour
{
    [Header("Deck configurations")]
    [SerializeField] private TextMeshProUGUI textCardsLeft;

    int numberOfCardsLeft;

    void Start()
    {
        InitDeck();
    }

    public void InitDeck()
    {
        numberOfCardsLeft = GameManager.Instance.ProvideInventoryManager().GetNumberOfCardsInDeck();
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
