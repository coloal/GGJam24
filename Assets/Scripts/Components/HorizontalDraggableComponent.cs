using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalDraggableComponent : MonoBehaviour
{   
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float maxVelocity = 3;

    [SerializeField] float brakeDistance = 10;
    [SerializeField] float escapeDistance = 5;
    [SerializeField] float rotateDistance = 5;
    [SerializeField] float maxArcRotation = 15;
    [SerializeField] float maxFinalRotation = 30;
    [SerializeField] float finalRotationVelocity = 10;
    [SerializeField] float escapeAcceleration = 100;
    bool isActive = true;
    float velocity = 0;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool isMouseClickPressed = false;
    List<Action> leftSwipeActions;
    List<Action> rigthtSwipeActions;
    
    public List<Action> RightSwipeActions => rigthtSwipeActions;
    public List<Action> LeftSwipeActions => leftSwipeActions;

    Vector2 initialPosition;

    void OnEnable()
    {
        initialPosition = transform.position;
    }

    void Awake()
    {
        leftSwipeActions = new List<Action>();
        rigthtSwipeActions = new List<Action>();
    }

    void Update()
    {
        HorizontalTick();
    }

    void HorizontalTick()
    {
        Vector2 targetPosition = CalculateHorizontalTargetPosition();
        float distance = MathF.Abs(transform.position.x - targetPosition.x);
        float velocity = CalculateActualVelocity(targetPosition, distance);

        // It's on right swipe limit
        if (transform.position.x >= initialPosition.x + escapeDistance)
        {
            if (targetPosition.x > transform.position.x)
            {
                float TargetRotation = -Mathf.Lerp(maxArcRotation, maxFinalRotation, distance / rotateDistance);
                if (-finalRotationVelocity * Time.deltaTime + transform.eulerAngles.z <= TargetRotation + 360)
                {
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, TargetRotation);
                }
                else
                {
                    transform.eulerAngles += new Vector3(0.0f, 0.0f, -finalRotationVelocity) * Time.deltaTime;
                }
            }

            velocity = Mathf.Min(0, velocity);
            this.velocity = Mathf.Min(0, this.velocity);
        }
        // It's on left swipe limit
        else if (transform.position.x <= initialPosition.x - escapeDistance)
        {
            if (targetPosition.x < transform.position.x)
            {
                float TargetRotation = Mathf.Lerp(maxArcRotation, maxFinalRotation, distance / rotateDistance);
                if (finalRotationVelocity * Time.deltaTime + transform.eulerAngles.z >= TargetRotation)
                {
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, TargetRotation);
                }
                else
                {
                    transform.eulerAngles += new Vector3(0.0f, 0.0f, finalRotationVelocity) * Time.deltaTime;
                }
            }

            velocity = Mathf.Max(0, velocity);
            this.velocity = Mathf.Max(0, this.velocity);
        }
        else
        {
            float totalDistance = escapeDistance;
            float alreadyTraveledDistance = transform.position.x - initialPosition.x;

            if (alreadyTraveledDistance > 0.0f)
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, -Mathf.Lerp(0.0f, maxArcRotation, alreadyTraveledDistance / totalDistance));
            }
            else
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(0.0f, maxArcRotation, alreadyTraveledDistance / -totalDistance));
            }
        }

        transform.Translate(new Vector2(velocity * Time.deltaTime, 0.0f));
        if (transform.position.y > initialPosition.y)
        {
            transform.position = new Vector2(transform.position.x, initialPosition.y);
        }
    }

    float CalculateActualVelocity(Vector2 TargetPosition, float Distance)
    {
        float movementDirection = Mathf.Sign(TargetPosition.x - transform.position.x);
        float actualMaxVelocity = Distance > brakeDistance ? maxVelocity : Mathf.Lerp(0, maxVelocity, Distance / brakeDistance);

        movementDirection *= acceleration;
        velocity += movementDirection * Time.deltaTime;
        velocity = Mathf.Abs(velocity) > actualMaxVelocity ? Mathf.Sign(velocity) * actualMaxVelocity : velocity;

        return velocity;
    }

    Vector2 CalculateHorizontalTargetPosition()
    {
        Vector2 targetPosition = isMouseClickPressed ? mousePosition - clickedPosition : Vector2.zero;
        return targetPosition + initialPosition;
    }

    void OnLeftClick()
    {
        BoxCollider2D boxCollider2DComponent = GetComponent<BoxCollider2D>();
        if (boxCollider2DComponent && boxCollider2DComponent.bounds.Contains(mousePosition))
        {
            isMouseClickPressed = true;
            clickedPosition = mousePosition;
        }
    }

    void OnLeftRelease()
    {
        isMouseClickPressed = false;
        if (enabled && (Mathf.Sign(velocity) == Mathf.Sign(transform.position.x - initialPosition.x) || Mathf.Abs(velocity) < 0.5))
        {
            // It's swipping right
            if (Mathf.Sign(transform.position.x - initialPosition.x) > 0)
            {
                bool HasPassedEscapeDistance = transform.position.x >= initialPosition.x + escapeDistance;
                if (HasPassedEscapeDistance)
                {
                    foreach (Action RightSwipeAction in rigthtSwipeActions)
                    {
                        RightSwipeAction();
                    }   
                }
            }
            // It's swipping left
            else
            {
                bool HasPassedEscapeDistance = transform.position.x < initialPosition.x - escapeDistance;
                if (HasPassedEscapeDistance)
                {
                    foreach (Action LeftSwipeAction in leftSwipeActions)
                    {
                        LeftSwipeAction();
                    }    
                }
            }
        }
    }

    void OnMouseMove(InputValue Value)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Value.Get<Vector2>());
    }
}
