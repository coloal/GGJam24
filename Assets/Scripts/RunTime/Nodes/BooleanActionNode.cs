using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Bool Action", "Action Nodes/Bool Action", color: "#F57600")] 
    public class BooleanActionNode : CodeGraphNode
    {
        [ExposedProperty()]
        public BrainAction Action;

        

        public BooleanActionNode()
        {
            /*
            outputs.Clear();
            outputs.Add("True");
            
            */
           
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
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

