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
    [SerializeField] float EscapeDistance = 3;
    [SerializeField] float RotateDistance = 5;
    [SerializeField] float MaxArcRotation = 30;
    [SerializeField] float MaxFinalRotation = 30;
    [SerializeField] float FinalRotationVelocity = 10;
    float velocity = 0;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool pressed = false;
    bool isInLimit = false;
    Vector2 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        Vector2 position = pressed ? mousePosition - clickedPosition : initialPosition;



        float distance = Mathf.Abs(transform.position.x - position.x);

        float actualMaxVelocity = distance > brakeDistance ? maxVelocity : Mathf.Lerp(0, maxVelocity, distance / brakeDistance);
        float direction = (position.x - transform.position.x) > 0 ? 1 : -1;



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
            isInLimit = true;
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
            isInLimit = true;
        }
        else
        {
            isInLimit = false;
            float totalDistance = (Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x - EscapeDistance - initialPosition.x);
            float alreadyTraveled = transform.position.x - initialPosition.x;
            if (alreadyTraveled > 0) transform.eulerAngles = new Vector3(0, 0, -Mathf.Lerp(0, MaxArcRotation, alreadyTraveled / totalDistance));
            else transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(0, MaxArcRotation, alreadyTraveled / -totalDistance));
        }


        transform.Translate(new Vector2(velocity * Time.deltaTime, 0));


    }

    void OnLeftClick()
    {
        pressed = true;
        clickedPosition = mousePosition;
    }

    void OnLeftRelease()
    {
        pressed = false;
    }

    void OnMouseMove(InputValue value)
    {
        mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        //Debug.Log(value.Get<Vector2>());
    }

}
