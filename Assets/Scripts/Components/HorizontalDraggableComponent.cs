using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalDraggableComponent : MonoBehaviour
{   
    [SerializeField] private float Acceleration = 0.3f;
    [SerializeField] private float MaxVelocity = 3;

    [SerializeField] float BrakeDistance = 10;
    [SerializeField] float EscapeDistance = 5;
    [SerializeField] float RotateDistance = 5;
    [SerializeField] float MaxArcRotation = 15;
    [SerializeField] float MaxFinalRotation = 30;
    [SerializeField] float FinalRotationVelocity = 10;
    [SerializeField] float EscapeAcceleration = 100;
    bool IsActive = true;
    float Velocity = 0;
    Vector2 MousePosition = Vector2.zero;
    Vector2 ClickedPosition = Vector2.zero;
    bool IsMouseClickPressed = false;
    bool IsInLimit = false;
    DraggableStates CurrentState = DraggableStates.PLAY_STATE;
    List<Action> mLeftSwipeActions;
    List<Action> mRigthtSwipeActions;
    
    public List<Action> RightSwipeActions => mRigthtSwipeActions;
    public List<Action> LeftSwipeActions => mLeftSwipeActions;

    Vector2 InitialPosition;

    Vector2 GameObjectMidPoint;

    void Awake()
    {
        SpriteRenderer SpriteRendererComponent = GetComponent<SpriteRenderer>();
        if (SpriteRendererComponent)
        {
            GameObjectMidPoint = new Vector2(
                transform.position.x + SpriteRendererComponent.bounds.size.x / 2,
                transform.position.y + SpriteRendererComponent.bounds.size.y / 2
            );
        }
    }

    void Start()
    {
        InitialPosition = transform.position;
        mLeftSwipeActions = new List<Action>();
        mRigthtSwipeActions = new List<Action>();
    }

    void Update()
    {
        HorizontalTick();
    }

    void HorizontalTick()
    {
        Vector2 TargetPosition = CalculateHorizontalTargetPosition();
        float Distance = MathF.Abs(transform.position.x - TargetPosition.x);
        float Velocity = CalculateActualVelocity(TargetPosition, Distance);

        // It's swipping right
        if (transform.position.x >= GameObjectMidPoint.x + EscapeDistance)
        {
            if (TargetPosition.x > transform.position.x)
            {
                float TargetRotation = -Mathf.Lerp(MaxArcRotation, MaxFinalRotation, Distance / RotateDistance);
                if (-FinalRotationVelocity * Time.deltaTime + transform.eulerAngles.z <= TargetRotation + 360)
                {
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, TargetRotation);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, -FinalRotationVelocity) * Time.deltaTime;
                }

                Velocity = Mathf.Min(0, Velocity);
                this.Velocity = Mathf.Min(0, this.Velocity);
            }
        }
        // It's swipping left
        else if (transform.position.x < GameObjectMidPoint.x - EscapeDistance)
        {
            if (TargetPosition.x < transform.position.x)
            {
                float TargetRotation = Mathf.Lerp(MaxArcRotation, MaxFinalRotation, Distance / RotateDistance);
                if (FinalRotationVelocity * Time.deltaTime + transform.eulerAngles.z >= TargetRotation)
                {
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, TargetRotation);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0.0f, 0.0f, FinalRotationVelocity) * Time.deltaTime;
                }
            }

            Velocity = Mathf.Max(0, Velocity);
            this.Velocity = Mathf.Max(0, this.Velocity);
        }
        else
        {
            float TotalDistance = GameObjectMidPoint.x + EscapeDistance - InitialPosition.x;
            float AlreadyTraveledDistance = transform.position.x - InitialPosition.x;

            if (AlreadyTraveledDistance > 0.0f)
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, -Mathf.Lerp(0.0f, MaxArcRotation, AlreadyTraveledDistance / TotalDistance));
            }
            else
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Lerp(0.0f, MaxArcRotation, AlreadyTraveledDistance / -TotalDistance));
            }
        }

        transform.Translate(new Vector2(Velocity * Time.deltaTime, 0.0f));
        if (transform.position.y > InitialPosition.y)
        {
            transform.position = new Vector2(transform.position.x, InitialPosition.y);
        }
    }

    float CalculateActualVelocity(Vector2 TargetPosition, float Distance)
    {
        float MovementDirection = Mathf.Sign(TargetPosition.x - transform.position.x);
        float ActualMaxVelocity = Distance > BrakeDistance ? MaxVelocity : Mathf.Lerp(0, MaxVelocity, Distance / BrakeDistance);

        MovementDirection *= Acceleration;
        Velocity += MovementDirection * Time.deltaTime;
        Velocity = Mathf.Abs(Velocity) > ActualMaxVelocity ? Mathf.Sign(Velocity) * ActualMaxVelocity : Velocity;

        return Velocity;
    }

    Vector2 CalculateHorizontalTargetPosition()
    {
        Vector2 TargetPosition = IsMouseClickPressed ? MousePosition - ClickedPosition : InitialPosition;
        return TargetPosition + InitialPosition;
    }

    void OnLeftClick()
    {
        BoxCollider2D BoxCollider2DComponent = GetComponent<BoxCollider2D>();
        if (BoxCollider2DComponent && BoxCollider2DComponent.bounds.Contains(MousePosition))
        {
            IsMouseClickPressed = true;
            ClickedPosition = MousePosition;
        }
    }

    void OnLeftRelease()
    {
        IsMouseClickPressed = false;
        if (Mathf.Sign(Velocity) == Mathf.Sign(transform.position.x - InitialPosition.x)|| Mathf.Abs(Velocity) < 0.5)
        {
            // It's swipping right
            if (Mathf.Sign(transform.position.x - InitialPosition.x) > 0)
            {
                foreach (Action RightSwipeAction in mRigthtSwipeActions)
                {
                    RightSwipeAction();
                }
            }
            // It's swipping left
            else
            {
                foreach (Action LeftSwipeAction in mRigthtSwipeActions)
                {
                    LeftSwipeAction();
                }
            }
        }
    }

    void OnMouseMove(InputValue Value)
    {
        MousePosition = Camera.main.ScreenToWorldPoint(Value.Get<Vector2>());
    }
}
