using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro BoxNameOfCard;

    [SerializeField]
    private GameObject RightTextBoxContainer;
    [SerializeField]
    private TextMeshProUGUI RightText;

    [SerializeField]
    private GameObject LeftTextBoxContainer;
    [SerializeField]
    private TextMeshProUGUI LeftText;

    [SerializeField]
    private TextMeshPro DescriptionText;

    [SerializeField]
    private SpriteRenderer CardSprite;

    [SerializeField]
    protected SpriteRenderer BackgroundSprite;

    [HideInInspector]
    public List<Option> LeftActions;
    [HideInInspector]
    public List<Option> RightActions;

    //Information of the card
    private string NameOfCard;


    bool CardIsActive = true;

    public void ShowText(bool IsLeft)
    {
        if (!CardIsActive) return;
        if (IsLeft)
        {
            if (LeftTextBoxContainer != null)
            {
                LeftTextBoxContainer.SetActive(true);
            }
        }
        else 
        {
            if (RightTextBoxContainer != null)
            {
                RightTextBoxContainer.SetActive(true);
            }
        } 
    }

    public void HideText(bool IsLeft)
    {
        if (IsLeft)
        {
            if (LeftTextBoxContainer != null)
            {
                LeftTextBoxContainer.SetActive(false);
            }
        }
        else
        {
            if (RightTextBoxContainer != null)
            {
                RightTextBoxContainer.SetActive(false);
            }
        }
    }

    public virtual void SetDataCard(CardTemplate DataCard) 
    {
        //Set informaci�n del DataCard
        NameOfCard = DataCard.NameOfCard;

        //Mostrar texto en pantalla
        BoxNameOfCard.text = NameOfCard;

        if (RightText != null)
        {
            RightText.text = DataCard.RightText;
            RightTextBoxContainer.SetActive(false);
        }
        
        if (LeftText != null)
        {
            LeftText.text = DataCard.LeftText;
            LeftTextBoxContainer.SetActive(false);
        }
        
        if (DescriptionText != null)
        {
            DescriptionText.text = DataCard.Background;
            DescriptionText.GetComponent<MeshRenderer>().sortingLayerID = CardSprite.sortingLayerID;
        }

        CardSprite.sprite = DataCard.CardSprite;
        if (BackgroundSprite != null && DataCard.BackgroundSprite != null)
        {
            BackgroundSprite.sprite = DataCard.BackgroundSprite;
        }

        LeftActions = DataCard.LeftActions;
        RightActions = DataCard.RightActions;
    }


    public void GoToBackGroundAndDeactivate()
    {
        BoxNameOfCard.sortingOrder = -2;
        CardSprite.sortingOrder = -3;
        DescriptionText.sortingOrder = -2;
        LeftTextBoxContainer.SetActive(false);
        RightTextBoxContainer.SetActive(false);
        CardIsActive = false;
        
    }
}
