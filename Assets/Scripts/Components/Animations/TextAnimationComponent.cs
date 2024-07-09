using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextAnimationComponent : MonoBehaviour
{
    [Header("Animation settings")]
    [SerializeField] private float perCharTimeDelay = 0.05f;

    protected bool skippedText = false;
    private Dictionary<int, IEnumerator> activeAnimations;

    void Awake()
    {
        activeAnimations = new Dictionary<int, IEnumerator>();
    }


    void OnClick()
    {
        skippedText = true;
    }

    public void PlayTypewriterAnimation(TextMeshProUGUI textMesh, string text, Action onAnimationEnded = null)
    {
        GameManager.Instance.ProvideInputManager().onClickEvent += OnClick;
        IEnumerator activeAnimationCoroutine;
        activeAnimations.TryGetValue(textMesh.GetInstanceID(), out activeAnimationCoroutine);
        if (activeAnimationCoroutine != null)
        {
            StopCoroutine(activeAnimationCoroutine);
            activeAnimations.Remove(textMesh.GetInstanceID());
        }

        IEnumerator typewriterAnimationCoroutine = 
            TypewriterAnimation(textMesh, text, perCharTimeDelay,
                onAnimationEnded: () => {
                    activeAnimations.Remove(textMesh.GetInstanceID());
                    GameManager.Instance.ProvideInputManager().onClickEvent -= OnClick;
                    if (onAnimationEnded != null)
                    {
                        onAnimationEnded();
                    }
                });
        activeAnimations.Add(textMesh.GetInstanceID(), typewriterAnimationCoroutine);
        StartCoroutine(typewriterAnimationCoroutine);
    }

    private IEnumerator TypewriterAnimation(TextMeshProUGUI textMesh, 
        string text, float delaySeconds, Action onAnimationEnded)
    {
        skippedText = false;
        textMesh.text = "";
        bool slower = false;
        bool angry = false;
        float slowerTime = 0f;
        foreach (char letter in text)
        {
            if (letter == '*')
            {
                slower = !slower;
                continue;
            }
            if(letter == '%')
            {
                angry = !angry;
                continue;
            }
            if (!skippedText)
            {
                
                yield return new WaitForSeconds(delaySeconds + slowerTime);
                if (!slower && !angry) slowerTime = 0f;
                else if (slower) slowerTime = 0.1f;
                else if (angry) slowerTime = 0.03f;
            }
            if(!skippedText && letter != ' ')
            {
                GameManager.Instance.ProvideSoundManager().PlayDialogSFX(slower ? TutorialManager.SceneTutorial.SlowerTextSound : angry ? TutorialManager.SceneTutorial.AngryTextSound : TutorialManager.SceneTutorial.TextSound);
            }
            
            textMesh.text += letter;
        }
        onAnimationEnded();
    }
}
