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
            //TODO: Change this patched behaviour: Maybe it is better to have only one action than several of them
            // and if we want more than one, we do it on the passed action itself
            verticalDraggableComponent.TopSwipeActions.Clear();
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
            //TODO: Change this patched behaviour: Maybe it is better to have only one action than several of them
            // and if we want more than one, we do it on the passed action itself
            verticalDraggableComponent.TopSwipeEscapeZoneEnterActions.Clear();
            verticalDraggableComponent.TopSwipeEscapeZoneExitActions.Clear();

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
            //TODO: Change this patched behaviour: Maybe it is better to have only one action than several of them
            // and if we want more than one, we do it on the passed action itself
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
            //TODO: Change this patched behaviour: Maybe it is better to have only one action than several of them
            // and if we want more than one, we do it on the passed action itself
            horizontalDraggableComponent.LeftSwipeEscapeZoneEnterActions.Clear();
            horizontalDraggableComponent.LeftSwipeEscapeZoneExitActions.Clear();

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
            //TODO: Change this patched behaviour: Maybe it is better to have only one action than several of them
            // and if we want more than one, we do it on the passed action itself
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
            //TODO: Change this patched behaviour: Maybe it is better to have only one action than several of them
            // and if we want more than one, we do it on the passed action itself
            horizontalDraggableComponent.RightSwipeEscapeZoneEnterActions.Clear();
            horizontalDraggableComponent.RightSwipeEscapeZoneExitActions.Clear();

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
