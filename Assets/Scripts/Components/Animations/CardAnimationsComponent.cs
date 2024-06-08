using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationsComponent : MonoBehaviour
{
    [Header("Animations settings")]

    [Header("Show card animation")]
    [Tooltip("Shoaw crad animation in seconds")]
    [SerializeField] private float showCardAnimationDuration = 3.0f;

    public void ShowCard() {
        // Placeholder logics for showing a card. Here must be place the final animation logic
        gameObject.SetActive(false);
        GameUtils.CreateTemporizer(() => {
            gameObject.SetActive(true);
        }, showCardAnimationDuration, this);
    }
}
