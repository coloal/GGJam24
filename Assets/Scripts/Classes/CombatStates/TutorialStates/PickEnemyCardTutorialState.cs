using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PickEnemyCardTutorialState : PickEnemyCardState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatSceneManager.Instance.ProvideCombatManager().OverwriteCombatContext(combatContext);
        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new PickPlayerCardTutorialState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickPlayerCard
        );
    }

    public override void ProcessImplementation(CombatManager.CombatContext combatContext)
    {
        TutorialManager.SceneTutorial.StartEnemyPickCardConversation(async () =>
        {
            await PickAnEnemyCard(combatContext);
        });
        
    }
}
