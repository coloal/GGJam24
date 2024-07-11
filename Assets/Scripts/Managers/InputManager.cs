using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI debugText;
    public static InputManager Instance;
    public static InputManager GameInputManager => GameManager.Instance.ProvideInputManager();
    public delegate void OnClickEvent();
    public event OnClickEvent onClickEvent;

    public delegate void OnMoveEvent(Vector2 cursorPosition);
    public event OnMoveEvent onMoveEvent;

    public delegate void OnReleaseEvent();
    public event OnReleaseEvent onReleaseEvent;

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

    void OnClick()
    {
        debugText.gameObject.SetActive(true);
        debugText.text += "Clicked";
        clickedPosition = mousePosition;
        onClickEvent?.Invoke();
    }

    void OnMove(InputValue inputValue)
    {
        debugText.gameObject.SetActive(true);
        debugText.text += "Moved";
        mousePosition = inputValue.Get<Vector2>();
        onMoveEvent?.Invoke(inputValue.Get<Vector2>());
    }
    
    void OnRelease()
    {
        debugText.gameObject.SetActive(true);
        debugText.text += "Released";
        onReleaseEvent?.Invoke();
    }

    public void ClearEvents()
    {
        onClickEvent = null;
        onReleaseEvent = null;
        onMoveEvent = null;
    }



}
