using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveStoryCardComponent : MonoBehaviour
{
    private HorizontalDraggableComponent horizontalDraggableComponent;

    void Awake()
    {
        horizontalDraggableComponent = GetComponent<HorizontalDraggableComponent>();
    }

    void DoOnValidDraggableComponents(Action lambda)
    {
        if (horizontalDraggableComponent)
        {
            lambda();
        }
    }

    public void SetOnSwipeLeftAction(Action onSwipeLeftAction)
    {
        DoOnValidDraggableComponents(() => {
            horizontalDraggableComponent.LeftSwipeActions.Clear();
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
            horizontalDraggableComponent.LeftSwipeEscapeZoneEnterActions.Clear();
            horizontalDraggableComponent.LeftSwipeEscapeZoneEnterActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeLeftEscapeZoneEnterAction();
                }
            });
            horizontalDraggableComponent.LeftSwipeEscapeZoneExitActions.Clear();
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
            horizontalDraggableComponent.RightSwipeActions.Clear();
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
            horizontalDraggableComponent.RightSwipeEscapeZoneEnterActions.Clear();
            horizontalDraggableComponent.RightSwipeEscapeZoneEnterActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeRightEscapeZoneEnterAction();
                }
            });
            horizontalDraggableComponent.RightSwipeEscapeZoneExitActions.Clear();
            horizontalDraggableComponent.RightSwipeEscapeZoneExitActions.Add(() => {
                if (horizontalDraggableComponent.enabled)
                {
                    onSwipeRightEscapeZoneExitAction();
                }
            });
        });
    }
}
