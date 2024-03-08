using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Debug Log", "Debug/Debug Log Console")]
    public class DebugLogNode : CodeGraphNode
    {
        [ExposedProperty()]
        public string log;
        public override string OnProcess(CodeGraphAsset graphAsset)
        {
            Debug.Log(log);
            return base.OnProcess(graphAsset);
        }
    }
}
