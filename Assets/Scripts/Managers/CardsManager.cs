using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsManager : MonoBehaviour
{
    // Lists of cards to be added to the game
    [SerializeField]
    GameObject CardPrefab;

    [SerializeField]
    List<CardTemplate> DataCardsList;

    [SerializeField]
    private bool isSequential;

    [SerializeField]
    Transform CardSpawnerOrigin;

    [SerializeField]
    List<CodeGraphObject> histories;

    [SerializeField]
    CodeGraphObject currentHistory;

    private int indexNextCard = 0;

    public GameObject SpawnNextCard()
    {
        GameObject newCard = Instantiate(CardPrefab, CardSpawnerOrigin.position, Quaternion.identity);

        CardTemplate nextCard = currentHistory.GetNextCard(true);
        if(nextCard != null)
        {
            newCard.GetComponent<Card>().SetDataCard(nextCard);
        }
        
        
        //indexNextCard++;
        return newCard;
    }

    public bool IsDeckEmpty()
    {
        return indexNextCard == DataCardsList.Count;
    }
}
