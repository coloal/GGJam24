using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickEnemyCardState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().OverwriteCombatContext(combatContext);
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickPlayerCardState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        GameObject enemyCard = enemyDeckManager.SelectRandomCard().gameObject;
        combatContext.enemyOnCombatCard = enemyCard;
        
        enemyCard.SetActive(true);
        enemyCard.gameObject.transform.SetParent(
            combatContext.enemyOnCombatCardFinalPosition.transform.parent,
            worldPositionStays: false
        );
        enemyCard.transform.position = combatContext.enemyOnCombatCardFinalPosition.transform.position;

        PostProcess(combatContext);
    }
}
