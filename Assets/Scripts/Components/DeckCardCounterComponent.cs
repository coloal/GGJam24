using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeckCardCounterComponent : MonoBehaviour
{
    [Header("Display information")]
    [SerializeField] private TextMeshProUGUI textCardLeft;

    private int cardsLeft;

    public void SetNumberOfCardsLeft(int cardsLeft) 
    {
        this.cardsLeft = cardsLeft;
        textCardLeft.text = this.cardsLeft.ToString();
    }

    public void ShowCardsLeft()
    {
        // TODO: Show cards left using the FeedbacksManager
        gameObject.SetActive(true);
    }

    public void HideCardsLeft()
    {
        // TODO: Hide cards left using the FeedbacksManager
        gameObject.SetActive(false);
    }
}
