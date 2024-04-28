using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitAnimationComponent : MonoBehaviour
{
    [Header("Animation configuration")]
    [SerializeField] private float animationDuration = 1.0f;
    [SerializeField] private float animationUpdateRateSeconds = 0.05f;
    [Header("Hit power configuration")]
    [SerializeField] private float superEffectiveAttackRadius = 1.0f;
    [SerializeField] private float neutralAttackRadius = 0.5f;
    [SerializeField] private float notVeryEffectiveAttackRadius = 0.1f;

    private bool isPlayingAnimation = false;
    private GameObject gameObjectToHit;
    private Vector2 gameObjectToHitOriginalPosition = Vector2.zero;
    private float shakeRadius = 1.0f;

    public void PlayHitAnimation(GameObject gameObjectToHit,
        AttackEffectiveness attackEffectiveness, Action onAnimationEnded)
    {
        this.gameObjectToHit = gameObjectToHit;
        gameObjectToHitOriginalPosition = gameObjectToHit.transform.position;
        isPlayingAnimation = true;

        switch (attackEffectiveness)
        {
            case AttackEffectiveness.NOT_VERY_EFFECTIVE:
                shakeRadius = notVeryEffectiveAttackRadius;
                break;
            case AttackEffectiveness.NEUTRAL:
                shakeRadius = neutralAttackRadius;
                break;
            case AttackEffectiveness.SUPER_EFFECTIVE:
                shakeRadius = superEffectiveAttackRadius;
                break;
        }
        
        StartCoroutine(PlayHitAnimation());

        GameUtils.CreateTemporizer(() =>
        {
            isPlayingAnimation = false;
            StopCoroutine(PlayHitAnimation());
            gameObjectToHit.transform.position = gameObjectToHitOriginalPosition;

            onAnimationEnded();
        }, animationDuration, this);
    }

    private IEnumerator PlayHitAnimation()
    {
        while (isPlayingAnimation)
        {
            gameObjectToHit.transform.position = 
                gameObjectToHitOriginalPosition + UnityEngine.Random.insideUnitCircle * shakeRadius;
            
            yield return new WaitForSeconds(animationUpdateRateSeconds);
        }
    }
}
