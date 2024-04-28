using UnityEngine;

public class VerticalDraggableComponentNoInput : MonoBehaviour
{
    [Header("Swipe speed configuration")]
    [SerializeField] private float acceleration = 500.0f;
    [SerializeField] private float maxVelocity = 500.0f;
    [SerializeField] float brakeDistance = 50.0f;


    float velocity = 0.0f;
    Vector2 relativeTargetPosition = Vector2.zero;
    
  

    Vector2 initialPosition;


    void OnEnable()
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

        transform.Translate(new Vector2(0, velocity * Time.deltaTime));
    }

    Vector2 CalculateVerticalTargetPosition()
    {
        return relativeTargetPosition + initialPosition;
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

    public void SetRelativeTargetPosition(float relativeTargetPosition)
    {
        this.relativeTargetPosition = new Vector2(0, relativeTargetPosition);
    }

    public void RestartMovement()
    {
        velocity = 0;
    }
   
    public float GetVerticalMovedPosition()
    {
        return transform.position.y - initialPosition.y;
    }

    public Vector3 GetMovedPosition()
    {
        return transform.position - new Vector3(initialPosition.x, initialPosition.y, 0);
    }
}
