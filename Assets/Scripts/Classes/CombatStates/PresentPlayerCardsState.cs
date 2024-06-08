using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentPlayerCardsState : CombatState
{
    [SerializeField] int numberOfCards = 3;
    public override void PostProcess(CombatV2Manager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatV2Manager().ProcessCombat(new PickEnemyCardState());
    }

    public override void Preprocess(CombatV2Manager.CombatContext combatContext)
    {
    }

    public override void ProcessImplementation(CombatV2Manager.CombatContext combatContext)
    {
        // for(int i = 0; i < numberOfCards; i++)
        // {
        //     CombatCard cardToSpawn = GameManager.Instance.ProvideDeckManager().GiveTopCardToHand();
        //     if(cardToSpawn != null)
        //     {
        //         GameUtils.CreateTemporizer(() =>
        //         {
        //             cardToSpawn.gameObject.SetActive(true);
        //             cardToSpawn.gameObject.transform.parent = combatContext.playerHandContainer.transform;
        //             PostProcess(combatContext);
        //         }, i * 1, GameManager.Instance);
        //     }
           
        // }

        PostProcess(combatContext);
        
    }
}
