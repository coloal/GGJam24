using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderScript : MonoBehaviour
{
    [SerializeField]
    bool IsLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<StoryCard>()?.ShowText(IsLeft);
        collision.gameObject.GetComponent<DraggableComponent>()?.SetInLimit(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<StoryCard>()?.HideText(IsLeft);
        collision.gameObject.GetComponent<DraggableComponent>()?.SetInLimit(false);
    }

}
