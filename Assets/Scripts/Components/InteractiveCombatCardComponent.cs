using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCombatCardComponent : MonoBehaviour
{
    private ClickableCardComponent clickableCardComponent;

    void Awake()
    {
        clickableCardComponent = GetComponent<ClickableCardComponent>();
    }

    void DoOnValidInteractiveComponents(Action lambda)
    {
        if (clickableCardComponent != null)
        {
            lambda();
        }
    }

    public void SetOnClickAction(Action onSwipeUpAction)
    {
        DoOnValidInteractiveComponents(() => {
            //TODO: Change this patched behaviour: Maybe it is better to have only one action than several of them
            // and if we want more than one, we do it on the passed action itself
            clickableCardComponent.OnClickActions.Clear();
            clickableCardComponent.OnClickActions.Add(() => {
                if (clickableCardComponent.enabled)
                {
                    onSwipeUpAction();
                }
            });
        });
    }

    public void EnableInteractiveComponent()
    {
        DoOnValidInteractiveComponents(() => {
            clickableCardComponent.enabled = true;
        });
    }

    public void DisableInteractiveComponent()
    {
        DoOnValidInteractiveComponents(() => {
            clickableCardComponent.enabled = false;
        });
    }
}
