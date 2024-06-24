using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCard : MonoBehaviour
{
    [SerializeField]
    private GameObject Container;

    [SerializeField]
    private GameObject UIText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActions(TossCoinState tossCoinState) 
    {
        HorizontalDraggableComponent HorizontalDragable = gameObject.GetComponent<HorizontalDraggableComponent>();

        if (HorizontalDragable != null)
        {
            //Limpiamos posibles referencias a estados antiguos
            HorizontalDragable.LeftSwipeActions.Clear();
            HorizontalDragable.RightSwipeActions.Clear();

            //Asignamos las acciones izq y drcha al nuevo CoinState
            HorizontalDragable.LeftSwipeActions.Add(() =>
            {
                tossCoinState.OnSwipeLeft();
            });

            HorizontalDragable.RightSwipeActions.Add(() =>
            {
                tossCoinState.OnSwipeRight();
            });
        }
    }

    public void EnableCard(bool result) 
    {
        //Oculta resultado
        UIText.GetComponent<TextMeshProUGUI>().enabled = false;

        Container.SetActive(result);
    }

    public void ShowCoinResult(int result) 
    {
        UIText.GetComponent<TextMeshProUGUI>().enabled = true;
        if (result == 0)
        {
            UIText.GetComponent<TextMeshProUGUI>().text = "HEADS";
        }
        else 
        {
            UIText.GetComponent<TextMeshProUGUI>().text = "TAILS";
        }
    }
}
