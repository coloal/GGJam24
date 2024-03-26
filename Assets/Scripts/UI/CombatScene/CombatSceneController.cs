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

    private TextAnimationComponent textAnimationComponent;

    void Awake()
    {
        textAnimationComponent = GetComponent<TextAnimationComponent>();
    }

    public void SetTurnNumber(int turn)
    {
        combatTurnsText.text = turn.ToString();
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
