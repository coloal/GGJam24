using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCardPrefab;

    private EnemyTemplate template;
    private List<CombatCard> hand;
    private List<CombatCard> graveyard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public CombatCard SelectRandomCard()
    {
        int idx = Random.Range(0, hand.Count);
        return hand[idx];
    }

    public void KillCard(CombatCard card) 
    {
        hand.Remove(card);
    }

    public void StartCombat(EnemyTemplate template)
    {
        this.template = template;
        template.CombatCards.ForEach(handTemplate =>
        {
            CombatCard card = Instantiate(combatCardPrefab)?.GetComponent<CombatCard>();
            card.gameObject.SetActive(false);
            card.SetDataCard(handTemplate);
            hand.Add(card);
            graveyard.Add(card);
        });
    }

    public CombatCardTemplate SelectCardToSave(int cardIndex)
    {
        return template.CombatCards[cardIndex];
    }

}
