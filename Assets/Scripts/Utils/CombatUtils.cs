using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatUtils
{
    private static float GetDamageMultiplier(CombatTypes AttackerCombatType,
        CombatTypes DefenderCombatType, out AttackEffectiveness AttackFinalEffectiveness)
    {
        float DamageMultiplier = 1.0f;
        AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;

        switch (AttackerCombatType)
        {
            case CombatTypes.Money:
            {
                switch (DefenderCombatType)
                {
                    case CombatTypes.Money:
                    {
                        DamageMultiplier = 1.0f;
                        AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
                        break;    
                    }
                    case CombatTypes.Influence:
                    {
                        DamageMultiplier = 2.0f;
                        AttackFinalEffectiveness = AttackEffectiveness.SUPER_EFFECTIVE;
                        break;    
                    }
                    case CombatTypes.Violence:
                    {
                        DamageMultiplier = 0.5f;
                        AttackFinalEffectiveness = AttackEffectiveness.NOT_VERY_EFFECTIVE;
                        break;    
                    }
                }
                break;
            }
            case CombatTypes.Influence:
            {
                switch (DefenderCombatType)
                {
                    case CombatTypes.Money:
                    {
                        DamageMultiplier = 0.5f;
                        AttackFinalEffectiveness = AttackEffectiveness.NOT_VERY_EFFECTIVE;
                        break;
                    }
                    case CombatTypes.Influence:
                    {
                        DamageMultiplier = 1.0f;
                        AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
                        break;    
                    }
                    case CombatTypes.Violence:
                    {
                        DamageMultiplier = 2.0f;
                        AttackFinalEffectiveness = AttackEffectiveness.SUPER_EFFECTIVE;
                        break;    
                    }
                }
                break;
            }
            case CombatTypes.Violence:
            {
                switch (DefenderCombatType)
                {
                    case CombatTypes.Money:
                    {
                        DamageMultiplier = 2.0f;
                        AttackFinalEffectiveness = AttackEffectiveness.SUPER_EFFECTIVE;
                        break;
                    }
                    case CombatTypes.Influence:
                    {
                        DamageMultiplier = 0.5f;
                        AttackFinalEffectiveness = AttackEffectiveness.NOT_VERY_EFFECTIVE;
                        break;    
                    }
                    case CombatTypes.Violence:
                    {
                        DamageMultiplier = 1.0f;
                        AttackFinalEffectiveness = AttackEffectiveness.NEUTRAL;
                        break;    
                    }
                }
                break;
            }
        }

        return DamageMultiplier;
    }

    private static int GetActualAttackDamage(CombatCard AttackerCombatCard, 
        CombatCard DefenderCombatCard, out AttackEffectiveness AttackFinalEffectiveness)
    {
        float AttackMultiplier = GetDamageMultiplier(
            AttackerCombatCard.GetCombatType(),
            DefenderCombatCard.GetCombatType(),
            out AttackFinalEffectiveness
        );

        float AttackDamage = AttackerCombatCard.GetDamage() * AttackMultiplier;
        if (AttackDamage >= DefenderCombatCard.GetArmor())
        {
            return (int) Mathf.Ceil(AttackDamage - DefenderCombatCard.GetArmor());
        }
        else
        {
            return 0;
        }
    }

    public static void Attack(CombatCard AttackerCombatCard, CombatCard DefenderCombatCard, out AttackEffectiveness AttackFinalEffectiveness)
    {
        int AttackDamage = GetActualAttackDamage(AttackerCombatCard, DefenderCombatCard, out AttackFinalEffectiveness);
        DefenderCombatCard.ReduceHealthPoints(AttackDamage);
    }

    public static void ReduceAttackerEnergy(this CombatCard AttackerCombatCard)
    {
        AttackerCombatCard.ReduceEnergy();
    }

}
