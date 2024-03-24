using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCombatCardComponent : MonoBehaviour
{
    public void SetOnSwipeUpAction(Action OnSwipeUpAction)
    {
        VerticalDraggableComponent VerticalDraggableComponent = GetComponent<VerticalDraggableComponent>();
        if (VerticalDraggableComponent)
        {
            VerticalDraggableComponent.SwipeActions.Add(() => {
                if (VerticalDraggableComponent.enabled)
                {
                    OnSwipeUpAction();
                }
            });
        }
    }

    public void SetIsActive(bool IsActive)
    {
        VerticalDraggableComponent VerticalDraggableComponent = GetComponent<VerticalDraggableComponent>();
        if (VerticalDraggableComponent)
        {
            VerticalDraggableComponent.enabled = IsActive;
        }
    }
}
