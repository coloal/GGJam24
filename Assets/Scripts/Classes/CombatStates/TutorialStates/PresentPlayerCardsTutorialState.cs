using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class PresentPlayerCardsTutorialState : PresentPlayerCardsState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        TutorialManager.SceneTutorial.StartCardExplanation(() =>
        {
            CombatUtils.ProcessNextStateAfterSeconds(
                nextState: new PresentEnemyCardsTutorialState(),
                seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickEnemyCard
            );
        });
    }
}
