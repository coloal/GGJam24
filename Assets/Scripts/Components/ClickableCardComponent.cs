using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableCardComponent : MonoBehaviour
{
    List<Action> onClickActions;
    bool isClicking;
    RectTransform rectTransformComponent;
    Vector2 clickPosition;

    public List<Action> OnClickActions => onClickActions;

    void OnEnable()
    {
        GameManager.Instance.ProvideInputManager().onClickEvent += OnClick;
        GameManager.Instance.ProvideInputManager().onMoveEvent += OnMove;
        GameManager.Instance.ProvideInputManager().onReleaseEvent += OnRelease;
    }

    void OnDisable()
    {
        GameManager.Instance.ProvideInputManager().onClickEvent -= OnClick;
        GameManager.Instance.ProvideInputManager().onMoveEvent -= OnMove;
        GameManager.Instance.ProvideInputManager().onReleaseEvent -= OnRelease;
    }

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

    void OnMove(Vector2 cursorPosition)
    {
        clickPosition = cursorPosition;
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
