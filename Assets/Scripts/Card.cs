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

    //Information of the card
    private string NameOfCard;

    //Incrementos/Decrementos
    public int InfluenceStat;
    public int MoneyStat;
    public int ViolenceStat;

    public List<HitManTypes> ListHitmanTypes;
    public HitmanInfo Contable;
    public HitmanInfo Maton;
    public HitmanInfo Comisario;

    bool CardIsActive = true;

    public void ShowText(bool IsLeft)
    {
        if (!CardIsActive) return;
        if (IsLeft)
        {
            LeftTextBoxContainer.SetActive(true);
        }
        else 
        {
            RightTextBoxContainer.SetActive(true);
        } 
    }

    public void HideText(bool IsLeft)
    {
        if (IsLeft)
        {
            LeftTextBoxContainer.SetActive(false);
        }
        else
        {
            RightTextBoxContainer.SetActive(false);
        }
    }

    public void SetDataCard(CardTemplate DataCard) 
    {
        //Set informaci�n del DataCard
        NameOfCard = DataCard.NameOfCard;

        InfluenceStat = DataCard.InfluenceStat;
        MoneyStat = DataCard.MoneyStat;
        ViolenceStat = DataCard.ViolenceStat;

        //Mostrar texto en pantalla
        BoxNameOfCard.text = NameOfCard;

        RightText.text = DataCard.RightText;
        LeftText.text = DataCard.LeftText;

        RightTextBoxContainer.SetActive(false);
        LeftTextBoxContainer.SetActive(false);
        
        DescriptionText.text = DataCard.Background;
        DescriptionText.GetComponent<MeshRenderer>().sortingLayerID = CardSprite.sortingLayerID;

        CardSprite.sprite = DataCard.CardSprite;

        ListHitmanTypes = DataCard.ListHitmanTypes;

        Contable = DataCard.Contable;
        Maton = DataCard.Maton;
        Comisario = DataCard.Comisario;
        
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
