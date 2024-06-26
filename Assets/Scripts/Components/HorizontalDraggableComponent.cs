using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class HorizontalDraggableComponent : MonoBehaviour
{   
    [Header("Swipe speed configuration")]
    [SerializeField] private float acceleration = 100.0f;
    [SerializeField] private float maxVelocity = 200.0f;

    [Header("Swipe Movement Escape Zone configuration")]
    [SerializeField] float escapeZoneUpperLimitDistance = 3.0f;
    [SerializeField] float escapeZoneLowerLimitDistance = 3.0f;
    [SerializeField] float brakeDistance = 10;
    [SerializeField] float rotateDistance = 5;
    [SerializeField] float maxArcRotation = 15;
    [SerializeField] float maxFinalRotation = 30;
    [SerializeField] float finalRotationVelocity = 10;
    
    float velocity = 0;

    Vector2 ClickedPosition => GameManager.Instance.ProvideInputManager().ClickedPosition;
    Vector2 WorldClickedPosition => GameManager.Instance.ProvideInputManager().WorldClickedPosition;
    Vector2 WorldMousePosition => GameManager.Instance.ProvideInputManager().WorldMousePosition;

    bool isMouseClickPressed = false;
    bool hasToTriggerEnterEscapeZone = true;
    bool hasToTriggerExitEscapeZone = true;

    List<Action> leftSwipeActions;
    List<Action> leftSwipeEscapeZoneEnterActions;
    List<Action> leftSwipeEscapeZoneExitActions;

    List<Action> rightSwipeActions;
    List<Action> rightSwipeEscapeZoneEnterActions;
    List<Action> rightSwipeEscapeZoneExitActions;
    RectTransform rectTransformComponent;

    public List<Action> LeftSwipeActions => leftSwipeActions;
    public List<Action> LeftSwipeEscapeZoneEnterActions => leftSwipeEscapeZoneEnterActions;
    public List<Action> LeftSwipeEscapeZoneExitActions => leftSwipeEscapeZoneExitActions;

    public List<Action> RightSwipeActions => rightSwipeActions;
    public List<Action> RightSwipeEscapeZoneEnterActions => rightSwipeEscapeZoneEnterActions;
    public List<Action> RightSwipeEscapeZoneExitActions => rightSwipeEscapeZoneExitActions;

    Vector2 initialPosition;



    void OnEnable()
    {
        initialPosition = transform.position;
    }

    void Awake()
    {
        leftSwipeActions = new List<Action>();
        leftSwipeEscapeZoneEnterActions = new List<Action>();
        leftSwipeEscapeZoneExitActions = new List<Action>();

        rightSwipeActions = new List<Action>();
        rightSwipeEscapeZoneEnterActions = new List<Action>();
        rightSwipeEscapeZoneExitActions = new List<Action>();
        rectTransformComponent = GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        GameManager.Instance.ProvideInputManager().onClickEvent += OnClick;
        GameManager.Instance.ProvideInputManager().onReleaseEvent += OnRelease;
    }

    public void SetInitialPosition()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        HorizontalTick();
    }

    private void OnDestroy()
    {
        GameManager.Instance.ProvideInputManager().onClickEvent -= OnClick;
        GameManager.Instance.ProvideInputManager().onReleaseEvent -= OnRelease;
    }

    void HorizontalTick()
    {
        Vector2 targetPosition = CalculateHorizontalTargetPosition();
        float distance = MathF.Abs(transform.position.x - targetPosition.x);
        float velocity = CalculateActualVelocity(targetPosition, distance);

        // It's on right swipe limit
        if (transform.position.x >= initialPosition.x + escapeZoneUpperLimitDistance)
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
        else if (transform.position.x <= initialPosition.x - escapeZoneUpperLimitDistance)
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
            float totalDistance = escapeZoneUpperLimitDistance;
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

        CheckForEscapeZone();

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
        Vector2 targetPosition = isMouseClickPressed ? WorldMousePosition - WorldClickedPosition : Vector2.zero;
        return targetPosition + initialPosition;
    }

    void OnClick()
    {

        isMouseClickPressed = true;
        if (rectTransformComponent && RectTransformUtility.RectangleContainsScreenPoint(rectTransformComponent, ClickedPosition, Camera.main))
        {
        }
    }

    void OnRelease() 
    {
        isMouseClickPressed = false;
        hasToTriggerEnterEscapeZone = true;
        hasToTriggerExitEscapeZone = false;

        if (enabled && (Mathf.Sign(velocity) == Mathf.Sign(transform.position.x - initialPosition.x) || Mathf.Abs(velocity) < 0.5))
        {
            // It's swipping right
            if (Mathf.Sign(transform.position.x - initialPosition.x) > 0)
            {
                bool isInsideEscapeZoneOrFarther = transform.position.x >= initialPosition.x + escapeZoneLowerLimitDistance;
                if (isInsideEscapeZoneOrFarther)
                {
                    foreach (Action rightSwipeOnWarningZoneExitAction in rightSwipeEscapeZoneExitActions)
                    {
                        rightSwipeOnWarningZoneExitAction();
                    }
                    foreach (Action rightSwipeAction in rightSwipeActions)
                    {
                        rightSwipeAction();
                    }
                }
            }
            // It's swipping left
            else
            {
                bool isInsideEscapeZoneOrFarther = transform.position.x < initialPosition.x - escapeZoneLowerLimitDistance;
                if (isInsideEscapeZoneOrFarther)
                {
                    foreach (Action leftSwipeOnWarningZoneExitAction in leftSwipeEscapeZoneExitActions)
                    {
                        leftSwipeOnWarningZoneExitAction();
                    }
                    foreach (Action leftSwipeAction in leftSwipeActions)
                    {
                        leftSwipeAction();
                    }
                }
            }
        }
    }

    //Position of the finger
   

    void CheckForEscapeZone()
    {
        if (isMouseClickPressed)
        {
            // It's swipping right
            if (Mathf.Sign(transform.position.x - initialPosition.x) > 0)
            {
                bool isInsideEscapeZoneOrFarther = transform.position.x >= initialPosition.x + escapeZoneLowerLimitDistance;
                if (isInsideEscapeZoneOrFarther && hasToTriggerEnterEscapeZone)
                {
                    hasToTriggerEnterEscapeZone = false;
                    hasToTriggerExitEscapeZone = true;
                    foreach (Action rightSwipeOnWarningZoneEnterAction in rightSwipeEscapeZoneEnterActions)
                    {
                        rightSwipeOnWarningZoneEnterAction();
                    }   
                }
                else if (!isInsideEscapeZoneOrFarther && hasToTriggerExitEscapeZone)
                {
                    hasToTriggerExitEscapeZone = false;
                    hasToTriggerEnterEscapeZone = true;
                    foreach (Action rightSwipeOnWarningZoneExitAction in rightSwipeEscapeZoneExitActions)
                    {
                        rightSwipeOnWarningZoneExitAction();
                    }
                }
            }
            // It's swipping left
            else
            {
                bool isInsideEscapeZoneOrFarther = transform.position.x < initialPosition.x - escapeZoneLowerLimitDistance;
                if (isInsideEscapeZoneOrFarther && hasToTriggerEnterEscapeZone)
                {
                    hasToTriggerEnterEscapeZone = false;
                    hasToTriggerExitEscapeZone = true;
                    foreach (Action leftSwipeOnWarningZoneEnterAction in leftSwipeEscapeZoneEnterActions)
                    {
                        leftSwipeOnWarningZoneEnterAction();
                    }   
                }
                else if (!isInsideEscapeZoneOrFarther && hasToTriggerExitEscapeZone)
                {
                    hasToTriggerExitEscapeZone = false;
                    hasToTriggerEnterEscapeZone = true;
                    foreach (Action leftSwipeOnWarningZoneExitAction in leftSwipeEscapeZoneExitActions)
                    {
                        leftSwipeOnWarningZoneExitAction();
                    }
                }
            }
        }
    }

    public float GetCurrentSpeed()
    {
        return velocity;
    }
}
