using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Random Split", "Random Nodes/3 Random Split", color: "#E40066")]
    public class MultipleRandomNode : CodeGraphNode
    {
        /*
        [ExposedProperty()]
        [Range(0, 100)]
        [Tooltip("Probabilidad de la primera opción (0-100%)")]
        public int FirstOptionProbability = 50;
        */

        public MultipleRandomNode()
        {
            outputs.Clear();
            outputs.Add("First Option");
            outputs.Add("Second Option");
            outputs.Add("Third Option");
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            int rand = Random.Range(0, 3);

            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, rand);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }

}
