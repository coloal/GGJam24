using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class VerticalDraggableComponent : MonoBehaviour
{
    [SerializeField] private float acceleration = 500;
    [SerializeField] private float maxVelocity = 500;

    [SerializeField] float brakeDistance = 50;
    [SerializeField] float escapeAcceleration = 100;
    [SerializeField] float escapeDistance = 0.5f;

    List<Action> topSwipeActions;
    float velocity = 0;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool isMouseClickPressed = false;

    Vector2 initialPosition;

    public List<Action> TopSwipeActions => topSwipeActions;

    void OnEnable()
    {
        initialPosition = transform.position;
    }

    void Awake()
    {
        topSwipeActions = new List<Action>();
    }

    // Update is called once per frame
    void Update()
    {
         VerticalTick();   
    }

    private void VerticalTick()
    {
        Vector2 targetPosition = CalculateVerticalTargetPosition();

        float distance = Mathf.Abs(transform.position.y - targetPosition.y);

        float velocity = CalculateVerticalActualVelocity(targetPosition, distance);


        if (transform.position.y >= initialPosition.y + escapeDistance)
        {
            velocity = Mathf.Min(0, velocity);
            this.velocity = Mathf.Min(0, this.velocity);
        }
        else if (transform.position.y <= initialPosition.y)
        {
            velocity = Mathf.Max(0, velocity);
            this.velocity = Mathf.Max(0, this.velocity);
        }

        transform.Translate(new Vector2(0, velocity * Time.deltaTime));

    }

    Vector2 CalculateVerticalTargetPosition()
    {
        Vector2 targetPosition = isMouseClickPressed ? mousePosition - clickedPosition : Vector2.zero;
        return targetPosition + initialPosition;
    }

    float CalculateVerticalActualVelocity(Vector2 targetPosition, float distance)
    {
        float direction = Mathf.Sign(targetPosition.y - transform.position.y);

        float actualMaxVelocity = distance > brakeDistance ? maxVelocity : Mathf.Lerp(0, maxVelocity, distance / brakeDistance);
        direction *= acceleration;
        velocity += direction * Time.deltaTime;
        velocity = Mathf.Abs(velocity) > actualMaxVelocity ? Mathf.Sign(velocity) * actualMaxVelocity : velocity;
        return velocity;
    }

    void SwipeUpMovement()
    {
        float position = -1;
        position *= escapeAcceleration;
        velocity += (position * Time.deltaTime);
    }

    void OnLeftClick()
    {
        if (this.GetComponent<BoxCollider2D>().bounds.Contains(mousePosition))
        {
            isMouseClickPressed = true;
            clickedPosition = mousePosition;
        }
    }

    void OnLeftRelease()
    {
        isMouseClickPressed = false;

        bool isAboveTopLimit = transform.position.y >= initialPosition.y + escapeDistance;

        if (enabled && isAboveTopLimit && 
            (Mathf.Sign(velocity) == Mathf.Sign(transform.position.x - initialPosition.x)|| Mathf.Abs(velocity) < 0.5))
        {
            foreach (Action TopSwipeAction in topSwipeActions)
            {
                TopSwipeAction();
            }
        }
    }

    void OnMouseMove(InputValue value)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }
}
