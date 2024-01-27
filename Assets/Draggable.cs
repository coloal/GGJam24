using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

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
    float velocity = 0;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool pressed = false;
    bool isInLimit = false;
    bool releasedInLimit = false;
    Vector2 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (releasedInLimit)
        {
            float position = (transform.position.x - initialPosition.x) > 0 ? 1 : -1;
            position *= EscapeAcceleration;
            velocity += (position * Time.deltaTime);
            transform.Translate(new Vector2(velocity * Time.deltaTime, 0));
            return;
        }

       
        bool IsInCorrectState = !(GameManager.Instance.ProvideTurnManager().GetCurrentGameState() == GameStates.MAKE_DECISION);

        Vector2 targetPosition = pressed && IsInCorrectState ? mousePosition - clickedPosition : initialPosition;
        
        float direction = (targetPosition.x - transform.position.x) > 0 ? 1 : -1;


        float distance = Mathf.Abs(transform.position.x - targetPosition.x);

        float actualMaxVelocity = distance > brakeDistance ? maxVelocity : Mathf.Lerp(0, maxVelocity, distance / brakeDistance);
       



        direction *= acceleration;
        velocity += (direction * Time.deltaTime);
        velocity = Mathf.Abs(velocity) > actualMaxVelocity ? Mathf.Sign(velocity) * actualMaxVelocity : velocity;



        if (transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - EscapeDistance)
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
            velocity = Mathf.Min(0, velocity);
            
        }
        else if (transform.position.x <= Camera.main.ScreenToWorldPoint(Vector2.zero).x + EscapeDistance)
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

    void OnLeftClick()
    {
        pressed = true;
        clickedPosition = mousePosition;
    }

    void OnLeftRelease()
    {
        pressed = false;

        if (isInLimit && (Mathf.Sign(velocity) == Mathf.Sign(transform.position.x - initialPosition.x)|| Mathf.Abs(velocity) < 0.5))
        {
            if(Mathf.Sign(transform.position.x - initialPosition.x) == 1)
            {
                GameManager.Instance.ProvideTurnManager().SwipeRight();
                //Llamar carta hacia la derecha, se bloquea
            }
            else
            {
                releasedInLimit = true;
                GameManager.Instance.ProvideTurnManager().SwipeLeft();
                //Llamar carta hacia la izquierda
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

    
}
