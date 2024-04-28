using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCardAnimationComponent : MonoBehaviour
{
    [Header("Animation configuration")]
    [SerializeField] private float animationSpeed = 50.0f;

    private Transform cardFinalPosition;
    private GameObject cardToMove;
    private Action onAnimationEnded = () => {};

    void Awake()
    {
        cardToMove = null;
    }

    void Update()
    {
        if (cardToMove)
        {
            float step = animationSpeed * Time.deltaTime;
            cardToMove.transform.position = 
                Vector2.MoveTowards(cardToMove.transform.position, cardFinalPosition.position, step);

            if (SqrDistance(cardToMove.transform.position, cardFinalPosition.position) < 0.001f)
            {
                cardToMove = null;
                onAnimationEnded();
            }    
        }
    }

    float SqrDistance(Vector2 origin, Vector2 target)
    {
        return (target - origin).sqrMagnitude;
    }

    public void StartMovingCardTowards(GameObject cardToMove, Transform cardFinalPosition, Action onAnimationEnded)
    {
        this.cardToMove = cardToMove;
        this.cardFinalPosition = cardFinalPosition;
        this.onAnimationEnded = onAnimationEnded;
    }
}
