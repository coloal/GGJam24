using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryCard : MonoBehaviour
{
    [Header("Visual configurations")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image characterSprite;

    [Header("Overlay text configurations")]
    [SerializeField] private GameObject overlayTextContainer;
    [SerializeField] private TextMeshProUGUI overlayText;

    //Information of the card
    bool cardIsActive = true;
    private string rightOverlayText;
    private string leftOverlayText;

    public void ShowText(bool isLeftText)
    {
        if (!cardIsActive) return;
        
        overlayTextContainer.SetActive(true);
        overlayText.text = isLeftText ? leftOverlayText : rightOverlayText;

        overlayText.alignment = isLeftText ? TextAlignmentOptions.Right : TextAlignmentOptions.Left;
    }

    public void HideText()
    {
        overlayTextContainer.SetActive(false);
    }

    public void SetDataCard(StoryCardTemplate cardData) 
    {
        //Set informacion del DataCard
        nameText.text = cardData.NameOfCard;
        rightOverlayText = cardData.RightText;
        leftOverlayText = cardData.LeftText;

        if (overlayText != null)
        {
            HideText();
        }
        
        if (descriptionText != null)
        {
            descriptionText.text = cardData.Text;
        }

        characterSprite.sprite = cardData.CharacterSprite;
    }

    public void GoToBackGroundAndDeactivate()
    {
        overlayTextContainer.SetActive(false);
        cardIsActive = false;        
    }
}
