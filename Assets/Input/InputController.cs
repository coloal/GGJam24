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
        Ray ray = gameCamera.ScreenPointToRay(MousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        hit.collider?.GetComponent<IClickable>()?.OnClick();
    }

    public void OnMouseOverRay(InputValue value) 
    {

        MousePosition = value.Get<Vector2>();
        Ray ray = gameCamera.ScreenPointToRay(MousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        hit.collider?.GetComponent<IClickable>()?.OnMouseOver();
    }
}
