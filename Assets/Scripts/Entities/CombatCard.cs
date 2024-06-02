using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatCard : MonoBehaviour
{
    [Header("Visual configurations")]
    [SerializeField] private TextMeshPro nameOfCard;
    [SerializeField] protected SpriteRenderer cardSprite;
    [SerializeField] private GameObject inactiveOverlay;

    [Header("Overlay text configurations")]
    [SerializeField] GameObject overlayTextContainer;
    [SerializeField] TextMeshProUGUI overlayTextMesh;

    protected CombatTypes combatType;
    protected CombatCardVisualComposerComponent visualComposerComponent;
    protected void SetCardSprite(CombatCardTemplate combatCardTemplate)
    {
        cardSprite.sprite = combatCardTemplate.CharacterSprite;
    }

    void Awake()
    {
        visualComposerComponent = GetComponent<CombatCardVisualComposerComponent>();
    }

    public void SetDataCard(CombatCardTemplate dataCard)
    {
        nameOfCard.text = dataCard.NameOfCard;
        combatType = dataCard.CombatType;
        SetCardSprite(dataCard);
    }

    public float GetCardWidth()
    {
        return cardSprite.bounds.size.x;
    }

    public CombatTypes GetCombatType()
    {
        return combatType;
    }

    public void SetInactiveOverlayActivation(bool isActive)
    {
        inactiveOverlay.SetActive(isActive);
    }
}
