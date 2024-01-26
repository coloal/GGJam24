using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDecisionComponent : MonoBehaviour
{
    [SerializeField] ScriptableObject CardData; 

    public void SwipeRight() {
        // Broadcast message that a swipe right has been performed
    }

    public void SwipeLeft() {
        // Broadcast message that a swipe left has been performed
    }
}
