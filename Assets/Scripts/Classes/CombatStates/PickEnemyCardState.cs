using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickEnemyCardState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickPlayerCardState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        CombatCard enemyCard = enemyDeckManager.SelectRandomCard();
        RectTransform enemyCardTransform = enemyCard.GetComponent<RectTransform>();
        if (enemyCardTransform)
        {
            enemyCardTransform.anchoredPosition = new Vector2(0,2000);
        }
        

        PostProcess(combatContext);
    }
}
