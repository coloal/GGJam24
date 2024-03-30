using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAnimationsComponent : MonoBehaviour
{
    [Header("Animation settings")]
    [Header("Turns counter animation")]
    [SerializeField] private float perTurnTimeDelay = 0.05f;

    public void PlayTurnCounterAnimation(int turn, Action<int> onNextTurnNumber, Action onAnimationEnded)
    {
        StartCoroutine(UpdateTurnWithDelayAnimation(perTurnTimeDelay, turn, onNextTurnNumber, onAnimationEnded));
    }

    private IEnumerator UpdateTurnWithDelayAnimation(float delaySeconds, int turn, 
        Action<int> onNextTurnNumber, Action onAnimationEnded)
    {
        for (int i = 0; i <= turn; i++)
        {
            yield return new WaitForSeconds(delaySeconds);
            onNextTurnNumber(i);
        }
        onAnimationEnded();
    }
}
