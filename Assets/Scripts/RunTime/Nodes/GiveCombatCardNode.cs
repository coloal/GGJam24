using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeInfo("Give Combat Card", "Action Nodes/Give Combat Card", color: "#F57600")]
public class GiveCombatCardNode : CodeGraphNode
{
    [ExposedProperty()]
    public CombatCardTemplate combatCard;

    public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
    {
        GameManager.Instance.ProvideInventoryManager().AddCombatCardToVault(combatCard);
        return base.OnNextNode(graphAsset, turnResult);
    }
}
