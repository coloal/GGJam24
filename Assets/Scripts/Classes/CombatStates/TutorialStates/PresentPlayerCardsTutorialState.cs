using System;
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
            CombatSceneManager.Instance.ProvideCombatManager().EnableNotebookButton();
            CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayShowNotebookButton();
            
            TutorialManager.SceneTutorial.StartNoteBookExplanation(() =>
            {
                CombatSceneManager.Instance.NotebookComponent.ToggleNotebook();
                GameManager.Instance.ProvideInputManager().onClickEvent += NoteBookHell; 
            });
            
        });
    }

    public void NoteBookHell()
    {
        GameManager.Instance.ProvideInputManager().onClickEvent -= NoteBookHell;
        CombatSceneManager.Instance.NotebookComponent.ToggleNotebook();
        GameUtils.CreateTemporizer(() =>
        {
            CombatUtils.ProcessNextStateAfterSeconds(
                nextState: new PresentEnemyCardsTutorialState(),
                seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickEnemyCard
            );
        }, 1, CombatSceneManager.Instance);
    }
}
