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
    [SerializeField] float EscapeAcceleration = 100;
    [SerializeField] float TopSwipeLimitOffset = 0.5f;

    List<Action> swipeActions;
    bool IsActive = true;
    float velocity = 0;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool pressed = false;
    DraggableStates CurrentState = DraggableStates.PLAY_STATE;

    Vector2 initialPosition;

    public List<Action> SwipeActions => swipeActions;

    void Awake()
    {
        swipeActions = new List<Action>();
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
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


        if (transform.position.y >= Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y)
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
        Vector2 targetPosition = pressed ? mousePosition - clickedPosition : initialPosition;
        return targetPosition + initialPosition;
    }

    float CalculateVerticalActualVelocity(Vector2 targetPosition, float distance)
    {
        float direction = Mathf.Sign(targetPosition.y - transform.position.y);

        float actualMaxVelocity = distance > brakeDistance ? maxVelocity : Mathf.Lerp(0, maxVelocity, distance / brakeDistance);
        direction *= acceleration;
        velocity += (direction * Time.deltaTime);
        velocity = Mathf.Abs(velocity) > actualMaxVelocity ? Mathf.Sign(velocity) * actualMaxVelocity : velocity;
        return velocity;
    }

    void SwipeUpMovement()
    {
        float position = -1;
        position *= EscapeAcceleration;
        velocity += (position * Time.deltaTime);
    }

    void OnLeftClick()
    {
        if (CurrentState != DraggableStates.PLAY_STATE) return;

        if (this.GetComponent<BoxCollider2D>().bounds.Contains(mousePosition))
        {
            pressed = true;
            clickedPosition = mousePosition;
        }
    }

    void OnLeftRelease()
    {
        if(CurrentState != DraggableStates.PLAY_STATE) return;
        
        pressed = false;

        bool isAboveTopLimit = transform.position.y >= initialPosition.y + TopSwipeLimitOffset;

        if (isAboveTopLimit && (Mathf.Sign(velocity) == Mathf.Sign(transform.position.x - initialPosition.x)|| Mathf.Abs(velocity) < 0.5))
        {
            if (!IsActive) return;
            IsActive = false;
            foreach (Action action in swipeActions)
            {
                action();
            }
        }
    }

    void OnMouseMove(InputValue value)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }
}
