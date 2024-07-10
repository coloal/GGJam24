using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PickEnemyCardTutorialState : PickEnemyCardState
{
    protected override EnemyDeckManager GetEnemyDeck()
    {
        return TutorialManager.SceneTutorial.EnemyDeck;
    }
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
            // Play draw a card from enemy deck
            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayEnemyDrawCardFromDeck(
                    enemyDeck: combatContext.enemyDeck
                );
            if (CombatSceneManager.Instance == null || CombatSceneManager.Instance.ProvideCombatManager().IsTaskCancellationRequested)
            {
                return;
            }
            await PickAnEnemyCard(combatContext);
        });
        
    }
}
