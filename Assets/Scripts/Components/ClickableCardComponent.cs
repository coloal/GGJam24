using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickableCardComponent : MonoBehaviour
{
    List<Action> onClickActions;
    bool isClicking;
    RectTransform rectTransformComponent;
    Vector2 clickPosition;

    public List<Action> OnClickActions => onClickActions;

    void Awake()
    {
        isClicking = false;
        onClickActions = new List<Action>();
        rectTransformComponent = GetComponent<RectTransform>();
        clickPosition = Vector2.zero;
    }

    void OnClick()
    {
        if (!isClicking)
        {
            isClicking = true;
        }
    }

    void OnMove(InputValue inputValue)
    {
        clickPosition = inputValue.Get<Vector2>();
    }

    void OnRelease()
    {
        if (isClicking)
        {
            if (rectTransformComponent != null &&
                RectTransformUtility.RectangleContainsScreenPoint(rectTransformComponent, clickPosition))
            {
                foreach (Action onClickAction in onClickActions)
                {
                    onClickAction();
                }
            }
            
            isClicking = false;
        }
    }
}
