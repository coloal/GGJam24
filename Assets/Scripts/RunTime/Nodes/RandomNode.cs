using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Random Split", "Random Nodes/Random Split")]
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

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
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
