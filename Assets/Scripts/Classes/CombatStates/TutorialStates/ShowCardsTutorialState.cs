using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShowCardsTutorialState : ShowCardsState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
    public override void PostProcess(CombatManager.CombatContext combatContext)
    {
        CombatUtils.ProcessNextStateAfterSeconds(
            nextState: new ResolveCombatTutorialState(),
            seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForResolveCombat
        );
    }
}
