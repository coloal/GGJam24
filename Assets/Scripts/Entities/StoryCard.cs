using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryCard : MonoBehaviour
{
    [Header("Visual configurations")]
    [SerializeField] private TextMeshPro boxNameOfCard;
    [SerializeField] private TextMeshPro descriptionText;
    [SerializeField] private SpriteRenderer cardSprite;
    [SerializeField] protected SpriteRenderer backgroundSprite;

    [Header("Overlay text configurations")]
    [SerializeField] private GameObject overlayTextContainer;
    [SerializeField] private TextMeshProUGUI overlayTextMesh;

    [HideInInspector] public List<BrainAction> leftActions;
    [HideInInspector] public List<BrainAction> rightActions;

    //Information of the card
    private string nameOfCard;
    bool cardIsActive = true;
    private string rightOverlayText;
    private string leftOverlayText;

    public void ShowText(bool isLeftText)
    {
        if (!cardIsActive) return;
        
        overlayTextContainer.SetActive(true);
        overlayTextMesh.text = isLeftText ? leftOverlayText : rightOverlayText;
    }

    public void HideText()
    {
        overlayTextContainer.SetActive(false);
    }

    public void SetDataCard(StoryCardTemplate DataCard) 
    {
        //Set informaci�n del DataCard
        nameOfCard = DataCard.NameOfCard;

        //Mostrar texto en pantalla
        boxNameOfCard.text = nameOfCard;
        rightOverlayText = DataCard.RightText;
        leftOverlayText = DataCard.LeftText;

        if (overlayTextMesh != null)
        {
            overlayTextContainer.SetActive(false);
        }
        
        if (descriptionText != null)
        {
            descriptionText.text = DataCard.Background;
            descriptionText.GetComponent<MeshRenderer>().sortingLayerID = cardSprite.sortingLayerID;
        }

        cardSprite.sprite = DataCard.CardSprite;
        if (backgroundSprite != null && DataCard.BackgroundSprite != null)
        {
            backgroundSprite.sprite = DataCard.BackgroundSprite;
        }

        leftActions = DataCard.LeftActions;
        rightActions = DataCard.RightActions;
    }


    public void GoToBackGroundAndDeactivate()
    {
        boxNameOfCard.sortingOrder = -2;
        cardSprite.sortingOrder = -3;
        descriptionText.sortingOrder = -2;
        overlayTextContainer.SetActive(false);
        cardIsActive = false;        
    }
}
