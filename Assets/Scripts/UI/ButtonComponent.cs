using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonComponent : MonoBehaviour
{
    public UnityEvent OnClick;

    private ClickableCardComponent clickableCardComponent;

    void Awake()
    {
        clickableCardComponent = GetComponent<ClickableCardComponent>();
    }

    void Start()
    {
        if (clickableCardComponent != null)
        {
            clickableCardComponent.OnClickActions.Clear();
            clickableCardComponent.OnClickActions.Add(() =>
            {
                OnClick?.Invoke();
            });
        }
    }
}
