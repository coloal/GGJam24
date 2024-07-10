using UnityEngine;

namespace CodeGraph
{
    [NodeInfo("Wait", "Util Nodes/Wait", color: "#6c4675")]
    public class WaitNode : CodeGraphNode
    {
        
        [ExposedProperty()]
        public float Seconds;

        
        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new WaitStep(Seconds);
            return true;
        }
    }
}


    
