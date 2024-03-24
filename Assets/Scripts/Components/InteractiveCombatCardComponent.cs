using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCombatCardComponent : MonoBehaviour
{
    private VerticalDraggableComponent VerticalDraggableComponent;
    private DraggableComponent HorizontalDraggableComponent;

    void Awake()
    {
        VerticalDraggableComponent = GetComponent<VerticalDraggableComponent>();
        HorizontalDraggableComponent = GetComponent<DraggableComponent>();
    }

    void DoOnValidDraggableComponents(Action Lambda)
    {
        if (VerticalDraggableComponent && HorizontalDraggableComponent)
        {
            Lambda();
        }
    }

    public void SetOnSwipeUpAction(Action OnSwipeUpAction)
    {
        DoOnValidDraggableComponents(() => {
            VerticalDraggableComponent.TopSwipeActions.Add(() => {
                if (VerticalDraggableComponent.enabled)
                {
                    OnSwipeUpAction();
                }
            });
        });
    }

    public void EnableVerticalDraggableComponent()
    {
        DoOnValidDraggableComponents(() => {
            VerticalDraggableComponent.enabled = true;
            HorizontalDraggableComponent.enabled = false;
        });
    }

    public void EnableHorizontalDraggableComponent()
    {
        DoOnValidDraggableComponents(() => {
            HorizontalDraggableComponent.enabled = true;
            VerticalDraggableComponent.enabled = false;
        });
    }

    public void DisableDraggableComponents()
    {
        DoOnValidDraggableComponents(() => {
            HorizontalDraggableComponent.enabled = false;
            VerticalDraggableComponent.enabled = false;
        });
    }
}
