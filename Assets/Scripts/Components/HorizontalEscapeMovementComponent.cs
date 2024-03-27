using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEscapeMovementComponent : MonoBehaviour
{
    [Header("Movement configurations")]
    [SerializeField] private float escapeAcceleration = 100;

    private float speed;
    private float movementDirection = 1.0f;
    private bool shouldStartMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (shouldStartMoving)
        {
            PerformEscapeMovement();
        }
    }

    void PerformEscapeMovement()
    {
        float position = movementDirection;
        position *= escapeAcceleration;
        speed += position * Time.deltaTime;
        transform.Translate(new Vector2(speed * Time.deltaTime, 0));
    }

    public void StartLeftEscapeMovement(float initialSpeed)
    {
        speed = initialSpeed;
        movementDirection = -1.0f;
        shouldStartMoving = true;
    }

    public void StartRightEscapeMovement(float initialSpeed)
    {
        speed = initialSpeed;
        movementDirection = 1.0f;
        shouldStartMoving = true;
    }
}
