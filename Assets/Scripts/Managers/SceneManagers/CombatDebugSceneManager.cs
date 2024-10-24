using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CombatDebugSceneManager : MonoBehaviour
{
    [Header("Enemy debug information")]
    [SerializeField] private TextMeshProUGUI violenceCardsText;
    [SerializeField] private TextMeshProUGUI influenceCardsText;
    [SerializeField] private TextMeshProUGUI moneyCardsText;

    private CombatSceneManager combatSceneManager;

    void OnEnable()
    {
        GameObject combatManagerGameObject = GameObject.Find("/CombatSceneManager");
        if (combatManagerGameObject != null)
        {
            combatSceneManager = combatManagerGameObject.GetComponent<CombatSceneManager>();
            if (combatSceneManager != null)
            {
                combatSceneManager.ProvideCombatManager().onCombatStateProcess += OnCombatStateChange;
            }
        }
    }

    void OnDisable()
    {
        if (combatSceneManager != null)
        {
            combatSceneManager.ProvideCombatManager().onCombatStateProcess -= OnCombatStateChange;
        }
    }

    void OnCombatStateChange(CombatState newCombatState)
    {
        void UpdateEnemyDeckInformation()
        {
            if (combatSceneManager != null)
            {
                EnemyDeckManager enemyDeckManager = combatSceneManager.ProvideEnemyDeckManager();
                int violenceCards = enemyDeckManager.GetCardsInDeck().Count((card) => card.GetCombatType() == CombatTypes.Violence);
                int influenceCards = enemyDeckManager.GetCardsInDeck().Count((card) => card.GetCombatType() == CombatTypes.Influence);
                int moneyCards = enemyDeckManager.GetCardsInDeck().Count((card) => card.GetCombatType() == CombatTypes.Money);

                violenceCardsText.text = violenceCards.ToString();
                influenceCardsText.text = influenceCards.ToString();
                moneyCardsText.text = moneyCards.ToString();
            }
        }

        UpdateEnemyDeckInformation();
    }
}
