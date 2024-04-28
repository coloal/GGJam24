namespace CodeGraph
{
    [NodeInfo("Zone Change", "Zone Nodes/Zone Change", color: "#C275E6")] 
    public class ChangeZoneNode : CodeGraphNode
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

