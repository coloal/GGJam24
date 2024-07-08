using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class PresentOnlyPlayerCardsTutorialState : PresentPlayerCardsState
{
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        TutorialManager.SceneTutorial.StartCardExplanation(() =>
        {
            CombatUtils.ProcessNextStateAfterSeconds(
                nextState: new PickEnemyCardTutorialState(),
                seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickEnemyCard
            );
        });
    }
}