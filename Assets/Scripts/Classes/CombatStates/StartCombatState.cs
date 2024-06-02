using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombatState : CombatState
{
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PresentEnemyCardsState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        DeckManager deckManager = GameManager.Instance.ProvideDeckManager();
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        deckManager.StartCombat();
        enemyDeckManager.StartCombat(combatContext.enemyTemplate);

        PostProcess(combatContext);
    }
}
