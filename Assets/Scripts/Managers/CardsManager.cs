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

    private int indexNextCard = 0;

    public GameObject SpawnNextCard(CardTemplate nextCard)
    {
        GameObject newCard = Instantiate(CardPrefab, CardSpawnerOrigin.position, Quaternion.identity);

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
