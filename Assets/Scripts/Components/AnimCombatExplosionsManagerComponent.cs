using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCombatExplosionsManagerComponent : MonoBehaviour
{
    [Header("Animations prefabs")]
    [SerializeField] private GameObject animCombatExplosionInfluence;
    [SerializeField] private GameObject animCombatExplosionMoney;
    [SerializeField] private GameObject animCombatExplosionViolence;

    private GameObject animCombatExplosionInflenceInstance;
    private GameObject animCombatExplosionMoneyInstance;
    private GameObject animCombatExplosionViolenceInstance;

    private GameObject currentActiveAnimation;

    void Awake()
    {
        currentActiveAnimation = null;
    }

    void Start()
    {
        animCombatExplosionInflenceInstance = Instantiate(animCombatExplosionInfluence, this.transform);
        animCombatExplosionInflenceInstance.SetActive(false);

        animCombatExplosionMoneyInstance = Instantiate(animCombatExplosionMoney, this.transform);
        animCombatExplosionMoneyInstance.SetActive(false);

        animCombatExplosionViolenceInstance = Instantiate(animCombatExplosionViolence, this.transform);
        animCombatExplosionViolenceInstance.SetActive(false);
    }

    public void SetAnimExplosionToPlay(CombatCard attackerCard, Transform animPositionTransform, bool hasToFlipExplosions)
    {
        switch (attackerCard.GetCombatType())
        {
            case CombatTypes.Influence:
                currentActiveAnimation = animCombatExplosionInflenceInstance;
                break;
            case CombatTypes.Money:
                currentActiveAnimation = animCombatExplosionMoneyInstance;
                break;
            case CombatTypes.Violence:
                currentActiveAnimation = animCombatExplosionViolenceInstance;
                break;
        }

        currentActiveAnimation.transform.position = animPositionTransform.position;
        
        if (hasToFlipExplosions)
        {
            currentActiveAnimation.transform.localScale = new Vector2(
                currentActiveAnimation.transform.localScale.x,
                currentActiveAnimation.transform.localScale.y * -1.0f
            );
        }
    }

    public void PlayExplosionAnimation()
    {
        if (currentActiveAnimation != null)
        {
            transform.SetAsLastSibling();
            currentActiveAnimation.SetActive(true);
        }
    }

    public void StopExplosionAnimation()
    {
        if (currentActiveAnimation != null)
        {
            currentActiveAnimation.SetActive(false);
        }
    }
}
