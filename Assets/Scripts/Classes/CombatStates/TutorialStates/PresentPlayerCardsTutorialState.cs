using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class PresentPlayerCardsTutorialState : PresentPlayerCardsState
{
    bool hasPressedNoteBook = false;
    bool hasPressedDeck = false;
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
                CombatSceneManager.Instance.NotebookComponent.ToggleNotebookTutorial();
                //CombatSceneManager.Instance.NotebookComponent
                GameManager.Instance.ProvideInputManager().onClickEvent += NoteBookHell; 
            });
            
        });
    }

    public void NoteBookHell()
    {
        if (hasPressedNoteBook) return;
        hasPressedNoteBook = true;
        GameManager.Instance.ProvideInputManager().onClickEvent -= NoteBookHell;
        CombatSceneManager.Instance.NotebookComponent.ToggleNotebookTutorial();
        CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayHideNotebookButton();
        
        // Starts Player's Deck explanation
        GameUtils.CreateTemporizer(() =>
        {
            // Shows the cards left help
            CombatSceneManager.Instance.ProvideCombatManager().TogglePlayerCardLeftInDeckVisibility();

            TutorialManager.SceneTutorial.StartDeckExplanation(() =>
            {
                GameManager.Instance.ProvideInputManager().onClickEvent += DeckCardsHell;
            });
        }, 1, CombatSceneManager.Instance);
    }

    public void DeckCardsHell()
    {
        if (hasPressedDeck) return;
        hasPressedDeck = true;
        GameManager.Instance.ProvideInputManager().onClickEvent -= DeckCardsHell;

        // Hides the cards left help
        CombatSceneManager.Instance.ProvideCombatManager().TogglePlayerCardLeftInDeckVisibility();
        TutorialManager.SceneTutorial.UnBlockScreen(() =>
        {
            CombatUtils.ProcessNextStateAfterSeconds(
                nextState: new PresentEnemyCardsTutorialState(),
                seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickEnemyCard
            );
        });
    }

}
