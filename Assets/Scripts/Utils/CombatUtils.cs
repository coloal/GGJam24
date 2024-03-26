using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public static class CombatUtils
{
    private static float GetDamageMultiplier(CombatTypes attackerCombatType,
        CombatTypes defenderCombatType, out AttackEffectiveness attackFinalEffectiveness)
    {
        float damageMultiplier = 1.0f;
        attackFinalEffectiveness = AttackEffectiveness.NEUTRAL;

        switch (attackerCombatType)
        {
            case CombatTypes.Money:
            {
                switch (defenderCombatType)
                {
                    case CombatTypes.Money:
                    {
                        damageMultiplier = 1.0f;
                        attackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
                        break;    
                    }
                    case CombatTypes.Influence:
                    {
                        damageMultiplier = 2.0f;
                        attackFinalEffectiveness = AttackEffectiveness.SUPER_EFFECTIVE;
                        break;    
                    }
                    case CombatTypes.Violence:
                    {
                        damageMultiplier = 0.5f;
                        attackFinalEffectiveness = AttackEffectiveness.NOT_VERY_EFFECTIVE;
                        break;    
                    }
                }
                break;
            }
            case CombatTypes.Influence:
            {
                switch (defenderCombatType)
                {
                    case CombatTypes.Money:
                    {
                        damageMultiplier = 0.5f;
                        attackFinalEffectiveness = AttackEffectiveness.NOT_VERY_EFFECTIVE;
                        break;
                    }
                    case CombatTypes.Influence:
                    {
                        damageMultiplier = 1.0f;
                        attackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
                        break;    
                    }
                    case CombatTypes.Violence:
                    {
                        damageMultiplier = 2.0f;
                        attackFinalEffectiveness = AttackEffectiveness.SUPER_EFFECTIVE;
                        break;    
                    }
                }
                break;
            }
            case CombatTypes.Violence:
            {
                switch (defenderCombatType)
                {
                    case CombatTypes.Money:
                    {
                        damageMultiplier = 2.0f;
                        attackFinalEffectiveness = AttackEffectiveness.SUPER_EFFECTIVE;
                        break;
                    }
                    case CombatTypes.Influence:
                    {
                        damageMultiplier = 0.5f;
                        attackFinalEffectiveness = AttackEffectiveness.NOT_VERY_EFFECTIVE;
                        break;    
                    }
                    case CombatTypes.Violence:
                    {
                        damageMultiplier = 1.0f;
                        attackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
                        break;    
                    }
                }
                break;
            }
        }

        return damageMultiplier;
    }

    private static int GetActualAttackDamage(CombatCard attackerCombatCard, 
        CombatCard defenderCombatCard, out AttackEffectiveness attackFinalEffectiveness)
    {
        float attackMultiplier = GetDamageMultiplier(
            attackerCombatCard.GetCombatType(),
            defenderCombatCard.GetCombatType(),
            out attackFinalEffectiveness
        );

        float attackDamage = attackerCombatCard.GetDamage() * attackMultiplier;
        if (attackDamage >= defenderCombatCard.GetArmor())
        {
            return (int) Mathf.Ceil(attackDamage - defenderCombatCard.GetArmor());
        }
        else
        {
            return 0;
        }
    }

    public static void Attack(CombatCard attackerCombatCard, 
        CombatCard defenderCombatCard, out AttackEffectiveness attackFinalEffectiveness)
    {
        int attackDamage = GetActualAttackDamage(attackerCombatCard, defenderCombatCard, out attackFinalEffectiveness);
        defenderCombatCard.ReduceHealthPoints(attackDamage);
    }

    public static void ReduceAttackerEnergy(this CombatCard attackerCombatCard, int energyToReduce)
    {
        attackerCombatCard.ReduceEnergy(energyToReduce);
    }
    public static int CalculateEnergy(int turns)
    {
        return (int) Mathf.Ceil((float)turns / 3f);
        
        /*
        float NewEnergy = Mathf ((float)Turns / 3f;
        NewEnergy += 0.3f;
        if (NewEnergy < 1f)
        {
            Energy = 1;
        }
        else
        {
            Energy = Mathf.RoundToInt(NewEnergy);
        }*/
    }
}
