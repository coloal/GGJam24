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

    private Dictionary<int, IEnumerator> activeAnimations;

    void Awake()
    {
        activeAnimations = new Dictionary<int, IEnumerator>();
    }

    public void PlayTypewriterAnimation(TextMeshProUGUI textMesh, string text, Action onAnimationEnded = null)
    {
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
        textMesh.text = "";
        foreach (char letter in text)
        {
            yield return new WaitForSeconds(delaySeconds);
            textMesh.text += letter;
        }
        onAnimationEnded();
    }
}
