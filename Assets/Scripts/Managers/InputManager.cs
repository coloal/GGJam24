using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public delegate void OnClickEvent();
    public event OnClickEvent onClickEvent;

    public delegate void OnMoveEvent(Vector2 cursorPosition);
    public event OnMoveEvent onMoveEvent;

    public delegate void OnReleaseEvent();
    public event OnReleaseEvent onReleaseEvent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void OnClick()
    {
        onClickEvent?.Invoke();
    }

    void OnMove(InputValue inputValue)
    {
        onMoveEvent?.Invoke(inputValue.Get<Vector2>());
    }
    
    void OnRelease()
    {
        onReleaseEvent?.Invoke();
    }
}
