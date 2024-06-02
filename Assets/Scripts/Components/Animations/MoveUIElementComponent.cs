using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUIElementComponent : MonoBehaviour
{
    [Header("Animation configuration")]
    [SerializeField] private float animationSpeed = 50.0f;

    private Vector2 finalPosition;
    private GameObject objectToMove;
    private RectTransform objectToMoveRectTransform;
    private Action onAnimationEnded = () => {};

    void Awake()
    {
        objectToMove = null;
    }

    void Update()
    {
        if (objectToMove && objectToMoveRectTransform)
        {
            float step = animationSpeed * Time.deltaTime;
            objectToMoveRectTransform.anchoredPosition = 
                Vector2.MoveTowards(objectToMoveRectTransform.anchoredPosition, finalPosition, step);

            if (SqrDistance(objectToMoveRectTransform.anchoredPosition, finalPosition) < 0.001f)
            {
                objectToMove = null;
                onAnimationEnded();
            }
        }
    }

    float SqrDistance(Vector2 origin, Vector2 target)
    {
        return (target - origin).sqrMagnitude;
    }

    public void StartMovingTowards(GameObject objectToMove, Vector2 finalPosition, Action onAnimationEnded)
    {
        this.objectToMove = objectToMove;
        this.objectToMoveRectTransform = objectToMove.GetComponent<RectTransform>();
        this.finalPosition = finalPosition;
        this.onAnimationEnded = onAnimationEnded;
    }
}
