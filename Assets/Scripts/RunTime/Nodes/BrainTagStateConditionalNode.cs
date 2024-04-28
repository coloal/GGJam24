namespace CodeGraph
{
    [NodeInfo("State Condition", "Condition Nodes/State Condition", color: "#06B184")]
    public class BrainTagStateConditionalNode : CodeGraphNode
    {
        [ExposedProperty()]
        public BrainTagOptionPicker Picker;
        

        public BrainTagStateConditionalNode()
        {
            outputs.Clear();
            outputs.Add("True");
            outputs.Add("False");
        }

   
        public override string OnNextNode(CodeGraphAsset graphAsset, TurnResult turnResult)
        {
            int port = GameManager.Instance.ProvideBrainManager().IsState(Picker.selectedTag, Picker.selectedTagState) ? 0 : 1;
            CodeGraphNode nextNode = graphAsset.GetNodeConnected(id, port);
            if (nextNode != null)
            {
                return nextNode.id;
            }
            return string.Empty;
        }
    }
}

