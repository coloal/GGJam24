using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatSceneController : MonoBehaviour
{
    [Header("On screen text configurations")]
    [SerializeField] private TextMeshProUGUI combatTurnsText;
    [SerializeField] private GameObject dialogTextContainer;
    [SerializeField] private TextMeshProUGUI dialogText;

    [Header("Turns counter configuration")]
    [Header("Unit numbers")]
    [SerializeField] private GameObject unitNumberContainer;
    [SerializeField] private Image unitNumberImage;
    [Header("Tens numbers")]
    [SerializeField] private GameObject tensNumberContainer;
    [SerializeField] private Image tensUnitNumberImage;
    [SerializeField] private Image tensTensNumberImage;

    private CombatVisualManager combatVisualManager;
    private TextAnimationComponent textAnimationComponent;

    void Awake()
    {
        combatVisualManager = CombatSceneManager.Instance.ProvideCombatVisualManager();
        textAnimationComponent = GetComponent<TextAnimationComponent>();
    }

    public void SetTurnNumber(int turn)
    {
        string turnAsString = turn.ToString();
        // Turn number is an unit number
        if (turnAsString.Length == 1)
        {
            unitNumberContainer.SetActive(true);
            tensNumberContainer.SetActive(false);

            (Sprite _, Sprite unitNumberSprite) = combatVisualManager.GetTurnNumberAsSprites(turnAsString);
            unitNumberImage.sprite = unitNumberSprite;
        }
        // Turns number is a tens number
        else if (turnAsString.Length > 1)
        {
            tensNumberContainer.SetActive(true);
            unitNumberContainer.SetActive(false);

            (Sprite unitNumberSprite, Sprite tensNumberSprite) = combatVisualManager.GetTurnNumberAsSprites(turnAsString);
            tensUnitNumberImage.sprite = unitNumberSprite;
            tensTensNumberImage.sprite = tensNumberSprite;
        }        
    }

    public void ShowDialogText(string text)
    {
        if (textAnimationComponent)
        {
            GameUtils.CreateTemporizer(() =>
            {
                dialogTextContainer.SetActive(true);
                textAnimationComponent.PlayTypewriterAnimation(dialogText, text);
            }, 1.0f, this);       
        }
    }

    public void HideDialogText()
    {
        dialogTextContainer.SetActive(false);
    }
}
