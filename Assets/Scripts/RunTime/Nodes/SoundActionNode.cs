namespace CodeGraph
{
    [NodeInfo("Sound Action", "Sound Nodes/Sound Action", color: "#bb9eff")] 
    public class SoundActionNode : CodeGraphNode
    {
        [ExposedProperty()]
        public SoundAction Parameters;

        

        public SoundActionNode()
        {

           
        }

        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            GameManager.Instance.ProvideSoundManager().ExecuteSoundAction(Parameters);


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

