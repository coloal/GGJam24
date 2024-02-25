using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;


public enum DraggableStates
{
    PRE_TUTORIAL,
    TUTORIAL_MOVE_RIGHT,
    TUTORIAL_STOP_RIGHT,
    TUTORIAL_MOVE_LEFT,
    TUTORIAL_STOP_LEFT,
    TUTORIAL_RETURN,
    SWIPE_RIGHT,
    SWIPE_LEFT,
    PLAY_STATE
}

public class Draggable : MonoBehaviour
{
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float maxVelocity = 3;

    [SerializeField] float brakeDistance = 10;
    [SerializeField] float EscapeDistance = 5;
    [SerializeField] float RotateDistance = 5;
    [SerializeField] float MaxArcRotation = 15;
    [SerializeField] float MaxFinalRotation = 30;
    [SerializeField] float FinalRotationVelocity = 10;
    [SerializeField] float EscapeAcceleration = 100;
     bool IsActive = true;
    float velocity = 0;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool pressed = false;
    bool isInLimit = false;
    bool releasedInLimitLeft = false;
    bool releasedInLimitRight = false;
    int TutorialFase = 0;
    DraggableStates CurrentState = DraggableStates.TUTORIAL_MOVE_RIGHT;

    Vector2 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        

        if (CurrentState == DraggableStates.SWIPE_LEFT)
        {
            SwipeLeftMovement();
            return;
        }
        else if (CurrentState == DraggableStates.SWIPE_RIGHT)
        {
            SwipeRightMovement();
            return;
        }


        Vector2 targetPosition = CalculateTargetPosition();

        float distance = Mathf.Abs(transform.position.x - targetPosition.x);

        float velocity = CalculateActualVelocity(targetPosition, distance);

        if(CurrentState == DraggableStates.TUTORIAL_MOVE_LEFT || CurrentState == DraggableStates.TUTORIAL_MOVE_RIGHT || CurrentState == DraggableStates.TUTORIAL_RETURN)
        {
            velocity /= 10;
        }

        if (transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - EscapeDistance)
        {
            if (CurrentState == DraggableStates.TUTORIAL_MOVE_RIGHT)
            {
                CurrentState = DraggableStates.TUTORIAL_MOVE_LEFT;
            }
            else
            {
                if(targetPosition.x > transform.position.x)
                {
                    float targetRotation = -Mathf.Lerp(MaxArcRotation, MaxFinalRotation, distance / RotateDistance);
                    if (-FinalRotationVelocity * Time.deltaTime + transform.eulerAngles.z <= targetRotation + 360)
                    {
                        transform.eulerAngles = new Vector3(0, 0, targetRotation);
                    }
                    else
                    {
                        transform.eulerAngles += new Vector3(0, 0, -FinalRotationVelocity) * Time.deltaTime;
                    }
                }
                
                velocity = Mathf.Min(0, velocity);
            }
        }
        else if (transform.position.x <= Camera.main.ScreenToWorldPoint(Vector2.zero).x + EscapeDistance)
        {

            if (CurrentState == DraggableStates.TUTORIAL_MOVE_LEFT)
            {
                CurrentState = DraggableStates.PLAY_STATE;
            }
            else
            {
                if (targetPosition.x < transform.position.x)
                {
                    float targetRotation = Mathf.Lerp(MaxArcRotation, MaxFinalRotation, distance / RotateDistance);
                    if (FinalRotationVelocity * Time.deltaTime + transform.eulerAngles.z >= targetRotation)
                    {
                        transform.eulerAngles = new Vector3(0, 0, targetRotation);
                    }
                    else
                    {
                        transform.eulerAngles += new Vector3(0, 0, FinalRotationVelocity) * Time.deltaTime;
                    }
                    velocity = Mathf.Max(0, velocity);
                }
                
            }
        }
        else
        {
            float totalDistance = (Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - EscapeDistance - initialPosition.x);
            float alreadyTraveled = transform.position.x - initialPosition.x;
            if (alreadyTraveled > 0) transform.eulerAngles = new Vector3(0, 0, -Mathf.Lerp(0, MaxArcRotation, alreadyTraveled / totalDistance));
            else transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(0, MaxArcRotation, alreadyTraveled / -totalDistance));
        }


        transform.Translate(new Vector2(velocity * Time.deltaTime, 0));
        if(transform.position.y > initialPosition.y) transform.position = new Vector2(transform.position.x,initialPosition.y);  

    }

    Vector2 CalculateTargetPosition()
    {
        bool IsInCorrectState = GameManager.Instance.ProvideTurnManager().GetCurrentGameState() == GameStates.MAKE_DECISION;
        Vector2 targetPosition = pressed && IsInCorrectState ? mousePosition - clickedPosition : initialPosition;
        if(CurrentState == DraggableStates.TUTORIAL_MOVE_RIGHT)
        {
            targetPosition = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - (EscapeDistance - 0.1f), 0);
        }
        else if(CurrentState == DraggableStates.TUTORIAL_MOVE_LEFT)
        {
            targetPosition = new Vector2(Camera.main.ScreenToWorldPoint(Vector2.zero).x + (EscapeDistance - 0.1f), 0);
        }

        return targetPosition;
    }

    float CalculateActualVelocity(Vector2 targetPosition, float distance)
    {
        float direction = Mathf.Sign(targetPosition.x - transform.position.x);
       
        float actualMaxVelocity = distance > brakeDistance ? maxVelocity : Mathf.Lerp(0, maxVelocity, distance / brakeDistance);
        direction *= acceleration;
        velocity += (direction * Time.deltaTime);
        velocity = Mathf.Abs(velocity) > actualMaxVelocity ? Mathf.Sign(velocity) * actualMaxVelocity : velocity;
        return velocity;
    }

    void SwipeLeftMovement()
    {
        float position = -1;
        position *= EscapeAcceleration;
        velocity += (position * Time.deltaTime);
        transform.Translate(new Vector2(velocity * Time.deltaTime, 0));
    }

    void SwipeRightMovement()
    {
        float position = 1;
        position *= EscapeAcceleration;
        velocity += (position * Time.deltaTime);
        transform.Translate(new Vector2(velocity * Time.deltaTime, 0));
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
        if (isInLimit && (Mathf.Sign(velocity) == Mathf.Sign(transform.position.x - initialPosition.x)|| Mathf.Abs(velocity) < 0.5))
        {
            if(Mathf.Sign(transform.position.x - initialPosition.x) == 1)
            {
                if (!IsActive) return;
                IsActive = false;
                GameManager.Instance.ProvideTurnManager().SwipeRight();   
            }
            else
            {
                CurrentState = DraggableStates.SWIPE_LEFT;
                GameManager.Instance.ProvideTurnManager().SwipeLeft();
                
            }
        }
    }

    void OnMouseMove(InputValue value)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        //Debug.Log(value.Get<Vector2>());
    }

    public void SetInLimit(bool NewIsInLimit)
    {
        isInLimit = NewIsInLimit;
    }

    
    public void FinalSwipeRight()
    {
        CurrentState = DraggableStates.SWIPE_RIGHT;
    }

}
