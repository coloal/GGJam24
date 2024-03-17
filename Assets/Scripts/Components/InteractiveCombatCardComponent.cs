using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCombatCardComponent : MonoBehaviour, IClickable
{
    private bool IsActive;

    private Action OnClickAction;

    void Awake()
    {
        IsActive = false;
        OnClickAction = null;
    }

    public void OnClick()
    {
        if (IsActive && OnClickAction != null)
        {
            OnClickAction();    
        }
    }

    public void OnMouseOver()
    {
        //TODO: Add visual feedback to let the player know that the mouse is over this card
    }

    public void SetOnClickAction(Action OnClickAction)
    {
        this.OnClickAction = OnClickAction;
    }

    public void SetIsActive(bool IsActive)
    {
        this.IsActive = IsActive;
    }
}
