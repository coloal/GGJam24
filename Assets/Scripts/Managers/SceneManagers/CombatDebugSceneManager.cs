using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatDebugSceneManager : MonoBehaviour
{
    [Header("General debug configurations")]
    [SerializeField] private Color violenceCardsColor = Color.red;
    [SerializeField] private Color influenceCardsColor = Color.blue;
    [SerializeField] private Color moneyCardsColor = Color.green;

    [Header("Enemy debug information")]
    [SerializeField] private TextMeshProUGUI violenceCardsText;
    [SerializeField] private TextMeshProUGUI influenceCardsText;
    [SerializeField] private TextMeshProUGUI moneyCardsText;
    [SerializeField] private TextMeshProUGUI deckCardsText;
    [SerializeField] private TextMeshProUGUI tieZoneCardsText;
    [SerializeField] private Image pickedCardImage;

    private CombatSceneManager combatSceneManager;

    void OnEnable()
    {
        GameObject combatManagerGameObject = GameObject.Find("/CombatSceneManager");
        if (combatManagerGameObject != null)
        {
            combatSceneManager = combatManagerGameObject.GetComponent<CombatSceneManager>();
            if (combatSceneManager != null)
            {
                combatSceneManager.ProvideEnemyDeckManager().onDeckStateUpdate += OnEnemyDeckUpdate;
                combatSceneManager.ProvideEnemyDeckManager().onHandStateUpdate += OnEnemyCardPicked;
                combatSceneManager.ProvideEnemyDeckManager().onTieZoneStateUpdate += OnEnemyTieZoneUpdate;
            }
        }
    }

    void OnDisable()
    {
        if (combatSceneManager != null)
        {
            combatSceneManager.ProvideEnemyDeckManager().onDeckStateUpdate -= OnEnemyDeckUpdate;
            combatSceneManager.ProvideEnemyDeckManager().onHandStateUpdate -= OnEnemyCardPicked;
            combatSceneManager.ProvideEnemyDeckManager().onTieZoneStateUpdate -= OnEnemyTieZoneUpdate;
        }
    }

    void OnEnemyDeckUpdate(List<CombatCard> updatedEnemyDeck)
    {
        int violenceCards = updatedEnemyDeck.Count((card) => card.GetCombatType() == CombatTypes.Violence);
        int influenceCards = updatedEnemyDeck.Count((card) => card.GetCombatType() == CombatTypes.Influence);
        int moneyCards = updatedEnemyDeck.Count((card) => card.GetCombatType() == CombatTypes.Money);

        violenceCardsText.text = violenceCards.ToString();
        influenceCardsText.text = influenceCards.ToString();
        moneyCardsText.text = moneyCards.ToString();
        deckCardsText.text = updatedEnemyDeck.Count.ToString();
    }

    void OnEnemyCardPicked(List<CombatCard> pickedCards)
    {
        if (pickedCards.Count > 0)
        {
            CombatTypes pickedCardCombatType = pickedCards[0].GetCombatType();
            switch (pickedCardCombatType)
            {
                case CombatTypes.Violence:
                    pickedCardImage.color = violenceCardsColor;
                    break;
                case CombatTypes.Influence:
                    pickedCardImage.color = influenceCardsColor;
                    break;
                case CombatTypes.Money:
                    pickedCardImage.color = moneyCardsColor;
                    break;
                default:
                    pickedCardImage.gameObject.SetActive(false);
                    break;
            }
        }
    }

    void OnEnemyTieZoneUpdate(List<CombatCard> updatedTieZone)
    {
        tieZoneCardsText.text = updatedTieZone.Count.ToString();
    }
}
