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
        GameUtils.CreateTemporizer(() =>
        {
            TutorialManager.SceneTutorial.StartDeckExplanation(() =>
            {
                //TODO Mostrar animacion del deck
                GameManager.Instance.ProvideInputManager().onClickEvent += DeckCardsHell;
            });
        }, 1, CombatSceneManager.Instance);
    }

    public void DeckCardsHell()
    {
        if (hasPressedDeck) return;
        hasPressedDeck = true;
        GameManager.Instance.ProvideInputManager().onClickEvent -= DeckCardsHell;
        //TODO Llamar animacion para ocultar deck
        GameUtils.CreateTemporizer(() =>
        {
            CombatUtils.ProcessNextStateAfterSeconds(
                nextState: new PresentEnemyCardsTutorialState(),
                seconds: CombatSceneManager.Instance.ProvideCombatManager().timeForPickEnemyCard
            );
        }, 1, CombatSceneManager.Instance);
    }

}
