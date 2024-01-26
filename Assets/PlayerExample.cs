using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExample : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField]
    private float Speed;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 move = playerInput.Player.Movement.ReadValue<Vector2>();
        transform.position += move * Speed * Time.deltaTime;
    }
}
