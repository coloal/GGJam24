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
    [Header("Swipe speed configuration")]
    [SerializeField] private float acceleration = 500.0f;
    [SerializeField] private float maxVelocity = 500.0f;

    [Header("Swipe Movement Escape Zone configuration")]
    [SerializeField] float escapeZoneUpperLimitDistance = 3.0f;
    [SerializeField] float escapeZoneLowerLimitDistance = 3.0f;
    [SerializeField] float brakeDistance = 50.0f;
    [SerializeField] float topEscapeAcceleration = 100.0f;

    List<Action> topSwipeActions;
    List<Action> topSwipeEscapeZoneEnterActions;
    List<Action> topSwipeEscapeZoneExitActions;

    float velocity = 0.0f;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool isMouseClickPressed = false;
    bool hasToTriggerEnterEscapeZone = true;
    bool hasToTriggerExitEscapeZone = false;

    Vector2 initialPosition;

    public List<Action> TopSwipeActions => topSwipeActions;
    public List<Action> TopSwipeEscapeZoneEnterActions => topSwipeEscapeZoneEnterActions;
    public List<Action> TopSwipeEscapeZoneExitActions => topSwipeEscapeZoneExitActions;

    void OnEnable()
    {
        initialPosition = transform.position;
    }

    void Awake()
    {
        topSwipeActions = new List<Action>();
        topSwipeEscapeZoneEnterActions = new List<Action>();
        topSwipeEscapeZoneExitActions = new List<Action>();
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


        if (transform.position.y >= initialPosition.y + escapeZoneUpperLimitDistance)
        {
            velocity = Mathf.Min(0, velocity);
            this.velocity = Mathf.Min(0, this.velocity);
        }
        else if (transform.position.y <= initialPosition.y)
        {
            velocity = Mathf.Max(0, velocity);
            this.velocity = Mathf.Max(0, this.velocity);
        }

        CheckForEscapeZone();

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
        position *= topEscapeAcceleration;
        velocity += (position * Time.deltaTime);
    }

    void OnLeftClick()
    {
        BoxCollider2D boxCollider2DComponent = GetComponent<BoxCollider2D>();
        if (boxCollider2DComponent)
        {
            if (boxCollider2DComponent.bounds.Contains(mousePosition))
            {
                isMouseClickPressed = true;
                clickedPosition = mousePosition;
            }
        }
    }

    void OnLeftRelease()
    {
        isMouseClickPressed = false;
        hasToTriggerEnterEscapeZone = true;
        hasToTriggerExitEscapeZone = false;

        bool isInsideEscapeZoneOrAbove = transform.position.y >= initialPosition.y + escapeZoneLowerLimitDistance;
        if (enabled && isInsideEscapeZoneOrAbove &&
            (Mathf.Sign(velocity) == Mathf.Sign(transform.position.x - initialPosition.x)|| Mathf.Abs(velocity) < 0.5))
        {
            foreach (Action topSwipeOnWarningZoneExitAction in topSwipeEscapeZoneExitActions)
            {
                topSwipeOnWarningZoneExitAction();
            }
            foreach (Action topSwipeAction in topSwipeActions)
            {
                topSwipeAction();
            }
        }
    }

    void OnMouseMove(InputValue value)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }

    void CheckForEscapeZone()
    {   
        if (isMouseClickPressed)
        {
            bool isInsideEscapeZoneOrAbove = transform.position.y >= initialPosition.y + escapeZoneLowerLimitDistance;

            if (isInsideEscapeZoneOrAbove && hasToTriggerEnterEscapeZone)
            {
                hasToTriggerEnterEscapeZone = false;
                hasToTriggerExitEscapeZone = true;
                foreach (Action topSwipeOnWarningZoneEnterAction in topSwipeEscapeZoneEnterActions)
                {
                    topSwipeOnWarningZoneEnterAction();
                }
            }
            else if (!isInsideEscapeZoneOrAbove && hasToTriggerExitEscapeZone)
            {
                hasToTriggerExitEscapeZone = false;
                hasToTriggerEnterEscapeZone = true;
                foreach (Action topSwipeOnWarningZoneExitAction in topSwipeEscapeZoneExitActions)
                {
                    topSwipeOnWarningZoneExitAction();
                }
            }
        }
    }
}
