using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Bool Action", "Action Nodes/Bool Action", color: "#D17A22")] 
    public class BooleanActionNode : CodeGraphNode
    {
        [ExposedProperty()]
        public Option Action;

        

        public BooleanActionNode()
        {
            /*
            outputs.Clear();
            outputs.Add("True");
            
            */
           
        }

        public override bool GetNodeCard(out StoryCardTemplate card)
        {
            card = null;
            return false;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, bool bSwipedLeft)
        {
            GameManager.Instance.ProvideBrainManager().ExecuteActions(Action);


            //Avanza al siguiente nodo
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, 0);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }

}

