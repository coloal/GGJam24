using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI combatTurnsText;

    public void SetTurnNumber(int turn)
    {
        combatTurnsText.text = turn.ToString();
    }
}
