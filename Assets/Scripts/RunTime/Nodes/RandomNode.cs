using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Random Split", "Random Nodes/Random Split", color: "#E40066")]
    public class RandomNode : CodeGraphNode
    {
        [ExposedProperty()]
        [Range(0, 100)]
        [Tooltip("Probabilidad de la primera opción (0-100%)")]
        public int FirstOptionProbability = 50;

        public RandomNode()
        {
            outputs.Clear();
            outputs.Add("First Option");
            outputs.Add("Second Option");
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            int rand = Random.Range(0, 100);
            int port = rand < FirstOptionProbability ? 0 : 1;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }

}
