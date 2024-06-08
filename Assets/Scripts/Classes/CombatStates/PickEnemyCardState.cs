using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickEnemyCardState : CombatState
{
    private const float ENEMY_CARD_CANVAS_OFFSET = 360.15f;

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

        // CombatCard enemyCard = enemyDeckManager.SelectRandomCard();
        //GameObject enemyCard = enemyDeckManager.DebugGetRamdomCard();
        GameObject enemyCard = GameObject.Instantiate(
            enemyDeckManager.DebugGetRamdomCard(),
            combatContext.combatContainer.transform
        );
        
        RectTransform enemyCardTransform = enemyCard.GetComponent<RectTransform>();
        if (enemyCardTransform)
        {
            enemyCardTransform.anchoredPosition = new Vector2(0, ENEMY_CARD_CANVAS_OFFSET);
        }
        

        PostProcess(combatContext);
    }
}
