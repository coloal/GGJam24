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
        PlayerDeckManager playerDeckManager = GameManager.Instance.ProvideDeckManager();
        EnemyDeckManager enemyDeckManager = CombatSceneManager.Instance.ProvideEnemyDeckManager();

        // TODO: Fill the deck with whats saved inside the PlayerInventoryManager...
        // ...and, if there's an inventory manager, it seems like the PlayerDeckManager does not necessary has to live
        // inside the GameManager, and instead inside the CombatSceneManager ⚠️

        //playerDeckManager.StartCombat();
        //enemyDeckManager.StartCombat(combatContext.enemyTemplate);

        PostProcess(combatContext);
    }
}
