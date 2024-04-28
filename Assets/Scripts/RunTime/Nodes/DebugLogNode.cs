using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Debug Log", "Debug Nodes/Debug Log Console", color: "#FABC2A")]
    public class DebugLogNode : CodeGraphNode
    {

        [ExposedProperty()]
        public string debugLog;
        

 

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            Debug.Log(debugLog);
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, 0);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
        
    }
}
