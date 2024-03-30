using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Zone Change", "Zone Nodes/Zone Change", color: "#bb9eff")] 
    public class ZoneNode : CodeGraphNode
    {
        [ExposedProperty()]
        public ZoneTemplate NewZone;


        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new ChangeZoneStep(NewZone);
            return true;
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
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

