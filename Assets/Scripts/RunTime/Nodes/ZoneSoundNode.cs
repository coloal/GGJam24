using CodeGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace CodeGraph
{
    [NodeInfo("Zone Sound", "Sound Nodes/Zone Sound", color: "#bb9eff")] 
    public class ZoneSoundNode : CodeGraphNode
    {
        [ExposedProperty()]
        public MusicZones NewZone;

        

        public ZoneSoundNode()
        {

           
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            GameManager.Instance.ProvideBrainSoundManager().ChangeZone(NewZone);


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

