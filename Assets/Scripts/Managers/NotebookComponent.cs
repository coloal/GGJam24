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
