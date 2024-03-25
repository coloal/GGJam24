using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCombatCardComponent : MonoBehaviour
{
    private VerticalDraggableComponent verticalDraggableComponent;
    private HorizontalDraggableComponent horizontalDraggableComponent;

    void Awake()
    {
        verticalDraggableComponent = GetComponent<VerticalDraggableComponent>();
        horizontalDraggableComponent = GetComponent<HorizontalDraggableComponent>();
    }

    void DoOnValidDraggableComponents(Action lambda)
    {
        if (verticalDraggableComponent && horizontalDraggableComponent)
        {
            lambda();
        }
    }

    public void SetOnSwipeUpAction(Action onSwipeUpAction)
    {
        DoOnValidDraggableComponents(() => {
            verticalDraggableComponent.TopSwipeActions.Add(() => {
                if (verticalDraggableComponent.enabled)
                {
                    onSwipeUpAction();
                }
            });
        });
    }

    public void SetOnSwipeUpEscapeZoneActions(
        Action onSwipeUpEscapeZoneEnterAction, Action onSwipeUpEscapeZoneExitAction)
    {
        DoOnValidDraggableComponents(() => {
            verticalDraggableComponent.TopSwipeEscapeZoneEnterActions.Add(() => {
                if (verticalDraggableComponent.enabled)
                {
                    onSwipeUpEscapeZoneEnterAction();
                }
            });
            verticalDraggableComponent.TopSwipeEscapeZoneExitActions.Add(() => {
                if (verticalDraggableComponent.enabled)
                {
                    onSwipeUpEscapeZoneExitAction();
                }
            });
        });
    }

    public void SetOnSwipeLeftAction(Action onSwipeLeftAction)
    {
        DoOnValidDraggableComponents(() => {
            horizontalDraggableComponent.LeftSwipeActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeLeftAction();
                }
            });
        });
    }

    public void SetOnSwipeLeftEscapeZoneActions(
        Action onSwipeLeftEscapeZoneEnterAction, Action onSwipeLeftEscapeZoneExitAction)
    {
        DoOnValidDraggableComponents(() => {
            horizontalDraggableComponent.LeftSwipeEscapeZoneEnterActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeLeftEscapeZoneEnterAction();
                }
            });
            horizontalDraggableComponent.LeftSwipeEscapeZoneExitActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeLeftEscapeZoneExitAction();
                }
            });
        });
    }

    public void SetOnSwipeRightAction(Action onSwipeRightAction)
    {
        DoOnValidDraggableComponents(() => {
            horizontalDraggableComponent.RightSwipeActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeRightAction();
                }
            });
        });
    }

    public void SetOnSwipeRightEscapeZoneActions(
        Action onSwipeRightEscapeZoneEnterAction, Action onSwipeRightEscapeZoneExitAction)
    {
        DoOnValidDraggableComponents(() => {
            horizontalDraggableComponent.RightSwipeEscapeZoneEnterActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeRightEscapeZoneEnterAction();
                }
            });
            horizontalDraggableComponent.RightSwipeEscapeZoneExitActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeRightEscapeZoneExitAction();
                }
            });
        });
    }

    public void EnableVerticalDraggableComponent()
    {
        DoOnValidDraggableComponents(() => {
            verticalDraggableComponent.enabled = true;
            horizontalDraggableComponent.enabled = false;
        });
    }

    public void EnableHorizontalDraggableComponent()
    {
        DoOnValidDraggableComponents(() => {
            horizontalDraggableComponent.enabled = true;
            verticalDraggableComponent.enabled = false;
        });
    }

    public void DisableDraggableComponents()
    {
        DoOnValidDraggableComponents(() => {
            horizontalDraggableComponent.enabled = false;
            verticalDraggableComponent.enabled = false;
        });
    }
}
