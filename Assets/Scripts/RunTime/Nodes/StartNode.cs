using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Start", "Process/Start", false, true)]
    public class StartNode : CodeGraphNode
    {
        public override string OnProcess(CodeGraphAsset graphAsset)
        {
            Debug.Log("Process");
            return base.OnProcess(graphAsset);
        }
    }
}
