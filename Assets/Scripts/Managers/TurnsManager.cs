using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsManager : MonoBehaviour
{
    // Lists of cards to be added to the game
    [SerializeField]
    List<GameObject> SequentialCards;

    [SerializeField]
    List<GameObject> NonSequentialCards;

    [SerializeField]
    private bool isSequential;

    [SerializeField]
    private int indexNextCard = 0;

    public static TurnsManager Instance; // A static reference to the GameManager instance

    public Card LastCard;
    
    void Start()
    {
        SpawnNextCard(indexNextCard);
    }


    public void SpawnNextCard(int iNextCard)
    {
        //spawn next card
        Instantiate(SequentialCards[iNextCard], new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

    }
    /**

    Static LastCard;

    SpwanCard(int indice)
    {
        Cards[i] - devuelve inf al game
        Incrementa el indice de los chats -> if(indiceChat>chats.count) remove Card from list

    opcion 1
    al guardar la carta no hace falta setear nada en comps externos

    opcion 2
    get indice proximas elecciones y se lo setea a las funciones de los collider exteriores que van a llamar
    }

    RecibeDecision(int Decision)
    {
        Aplica incrementos anterior carta

        Coje la siguiente carta segun la anterior y obtiene su indice y llama a la función Spwan Card
    }

     */

}
