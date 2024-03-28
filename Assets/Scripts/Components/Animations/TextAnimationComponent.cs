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
        float animationDelay = perCharTimeDelay;
        textMesh.text = "";
        foreach (char letter in text)
        {
            GameUtils.CreateTemporizer(() =>
            {
                textMesh.text += letter;
            }, animationDelay, this);
            animationDelay += perCharTimeDelay;
        }
    }
}
