using System;
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
    private CombatAnimationsComponent combatAnimationsComponent;

    void Awake()
    {
        combatVisualManager = CombatSceneManager.Instance.ProvideCombatVisualManager();
        textAnimationComponent = GetComponent<TextAnimationComponent>();
        combatAnimationsComponent = GetComponent<CombatAnimationsComponent>();
    }

    public void SetTurnNumber(int turn)
    {
        SpritesUtils.SetNumberAsSprites(
            unitNumberContainer: unitNumberContainer,
            tensNumberContainer: tensNumberContainer,
            unitNumberImage: unitNumberImage,
            tensUnitNumberImage: tensUnitNumberImage,
            tensTensNumberImage: tensTensNumberImage,
            number: turn,
            getNumberAsSprite: combatVisualManager.GetTurnNumberAsSprites 
        );
    }

    public void SetTurnNumberAsAnimation(int turn, Action onAnimationEnded)
    {
        if (combatAnimationsComponent)
        {
            combatAnimationsComponent.PlayTurnCounterAnimation(turn,
                onNextTurnNumber: (nextTurnNumber) => {
                    SetTurnNumber(nextTurnNumber);
                },
                onAnimationEnded: onAnimationEnded
            );
        }
    }

    public void ShowDialogText(string text, Action onAnimationEnded = null)
    {
        if (textAnimationComponent)
        {
            GameUtils.CreateTemporizer(() =>
            {
                dialogTextContainer.SetActive(true);
                textAnimationComponent.PlayTypewriterAnimation(dialogText, text, onAnimationEnded);
            }, 1.0f, this);       
        }
    }

    public void HideDialogText()
    {
        dialogTextContainer.SetActive(false);
    }
}
