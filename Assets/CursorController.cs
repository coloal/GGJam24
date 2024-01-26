using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private PlayerInput mouseInput;

    [SerializeField] 
    private Camera gameCamera;

    private void Awake()
    {
        mouseInput = new PlayerInput();
        mouseInput.Mouse.Enable();

        mouseInput.Mouse.LeftClick.performed += ctx =>
        {
            RaycastHit hit;
            Vector3 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
            if (Physics.Raycast(gameCamera.ScreenPointToRay(mousePosition), out hit))
            {
                hit.collider.GetComponent<IClickable>()?.Onclick();
            }

        };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
