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
        foreach (char letter in text)
        {
            if(!skippedText)
            {
                yield return new WaitForSeconds(delaySeconds);
            }
            textMesh.text += letter;
        }
        onAnimationEnded();
    }
}
