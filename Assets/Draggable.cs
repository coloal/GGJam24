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

    float brakeDistance = 10;
    float velocity = 0;
    Vector2 mousePosition = Vector2.zero;
    Vector2 clickedPosition = Vector2.zero;
    bool pressed = false;

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
        float actualMaxVelocity = distance > brakeDistance ? maxVelocity :  Mathf.Lerp(0, maxVelocity, distance / brakeDistance);
        float direction = (position.x - transform.position.x) > 0 ? 1 : -1;
        direction *= acceleration;
        velocity += (direction * Time.deltaTime);
        velocity = Mathf.Abs(velocity) > actualMaxVelocity ? Mathf.Sign(velocity)*actualMaxVelocity : velocity;
        transform.Translate(new Vector2(velocity * Time.deltaTime, transform.position.y));
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
    }

}
