using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private Vector2 MousePosition;

    [SerializeField]
    private Camera gameCamera;
    public void OnLeftClickRay() 
    {

        RaycastHit hit;
        if (Physics.Raycast(gameCamera.ScreenPointToRay(MousePosition), out hit)) {
            hit.collider.GetComponent<IClickable>()?.OnClick();
        }
    }

    public void OnMouseOverRay(InputValue value) 
    {
        MousePosition = value.Get<Vector2>();
        Debug.Log(""+MousePosition);
        RaycastHit hit;
        if (Physics.Raycast(gameCamera.ScreenPointToRay(MousePosition), out hit)) {
            hit.collider.GetComponent<IClickable>()?.OnMouseOver();
        }
    }
}
