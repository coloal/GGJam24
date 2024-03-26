using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextAnimationComponent : MonoBehaviour
{
    [Header("Animation settings")]
    [SerializeField] private float perCharTimeDelay = 0.05f;

    public void PlayTypewriterAnimation(TextMeshProUGUI textMesh, string text)
    {
        textMesh.text = "";
        StartCoroutine(GetTypewriterAnimationCoroutine(textMesh, text));
    }

    private IEnumerator GetTypewriterAnimationCoroutine(TextMeshProUGUI textMesh, string text)
    {
        foreach (char letter in text)
        {
            yield return new WaitForSeconds(perCharTimeDelay);
            textMesh.text += letter;
        }
    }
}
