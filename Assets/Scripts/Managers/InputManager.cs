using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static InputManager GameInputManager => GameManager.Instance.ProvideInputManager();
    public delegate void OnClickEvent();
    public event OnClickEvent onClickEvent;

    public delegate void OnMoveEvent(Vector2 cursorPosition);
    public event OnMoveEvent onMoveEvent;

    public delegate void OnReleaseEvent();
    public event OnReleaseEvent onReleaseEvent;

    public delegate void OnPauseEvent();
    public event OnPauseEvent onPauseEvent;

    private Vector2 clickedPosition = Vector2.zero;
    public Vector2 ClickedPosition => clickedPosition;

    private Vector2 mousePosition = Vector2.zero;
    public Vector2 MousePosition => mousePosition;

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

    void Start()
    {
#if UNITY_ANDROID
        UnityEngine.InputSystem.EnhancedTouch.EnhancedTouchSupport.Enable();
#endif
    }

    void OnClick()
    {
        clickedPosition = mousePosition;
        onClickEvent?.Invoke();
    }

    void OnMove(InputValue inputValue)
    {
        mousePosition = inputValue.Get<Vector2>();
        onMoveEvent?.Invoke(inputValue.Get<Vector2>());
    }
    
    void OnRelease()
    {
        onReleaseEvent?.Invoke();
    }

    void OnPause()
    {
        onPauseEvent?.Invoke();
    }

    public void ClearEvents()
    {
        onClickEvent = null;
        onReleaseEvent = null;
        onMoveEvent = null;
    }

    public bool IsASingleTouchEvent()
    {
#if UNITY_ANDROID
        return UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 1;
#else
        return true;
#endif
    }

}
