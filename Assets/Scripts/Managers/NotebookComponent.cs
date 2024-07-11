using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookComponent : MonoBehaviour
{
    [Header("Notebook animations configurations")]
    [SerializeField] private GameObject notebook;
    [SerializeField] private Transform notebookOriginPosition;
    [SerializeField] private Transform notebookDestinationPosition;
    [SerializeField] private float notebookActiveBackgroungAlpha = 0.7f;

    private bool isNotebookShown = false;

    void Start()
    {
        notebook.SetActive(false);
    }

    public void ToggleNotebook()
    {
        if(GameManager.Instance.ProvideBrainManager().IsTutorial)
        {
            return;
        }
        if (!isNotebookShown)
        {
            ShowNotebook();
        }
        else
        {
            HideNotebook();
        }
    }

    public async void ShowNotebook()
    {
        if (!isNotebookShown)
        {
            notebook.SetActive(true);
            CombatSceneManager.Instance.ProvideCombatManager().DisablePlayerCardsInHandInteractiveComponent();

            //SFX
            GameManager.Instance.ProvideSoundManager().PlaySFX("Pause");

            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayShowNotebook(
                    origin: notebookOriginPosition,
                    destination: notebookDestinationPosition,
                    backgroundAplha: notebookActiveBackgroungAlpha
                );
            if (this != null && !destroyCancellationToken.IsCancellationRequested)
            {
                isNotebookShown = true;
            }
        }
    }

    public async void HideNotebook()
    {
        if (isNotebookShown)
        {
            //SFX
            GameManager.Instance.ProvideSoundManager().StopSFX("Pause");

            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayHideNotebook(
                    origin: notebookDestinationPosition,
                    destination: notebookOriginPosition,
                    backgroundAplha: notebookActiveBackgroungAlpha
                );
            if (this != null && !destroyCancellationToken.IsCancellationRequested)
            {
                isNotebookShown = false;
                notebook.SetActive(false);
                CombatSceneManager.Instance.ProvideCombatManager().EnablePlayerCardsInHandInteractiveComponent();
            }
        }
    }

    public void ToggleNotebookTutorial()
    {
        if (!isNotebookShown)
        {
            ShowNotebookTutorial();
        }
        else
        {
            HideNotebookTutorial();
        }
    }

    public async void ShowNotebookTutorial()
    {
        if (!isNotebookShown)
        {
            notebook.SetActive(true);
            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayShowNotebook(
                    origin: notebookOriginPosition,
                    destination: notebookDestinationPosition,
                    backgroundAplha: notebookActiveBackgroungAlpha
                );
            if (this != null && !destroyCancellationToken.IsCancellationRequested)
            {
                isNotebookShown = true;
            }
        }
    }

    public async void HideNotebookTutorial()
    {
        if (isNotebookShown)
        {
            await CombatSceneManager.Instance.ProvideCombatFeedbacksManager()
                .PlayHideNotebook(
                    origin: notebookDestinationPosition,
                    destination: notebookOriginPosition,
                    backgroundAplha: notebookActiveBackgroungAlpha
                );
            if (this != null && !destroyCancellationToken.IsCancellationRequested)
            {
                isNotebookShown = false;
                notebook.SetActive(false);
            }
        }
    }

}
