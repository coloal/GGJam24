namespace CodeGraph
{
    [NodeInfo("Straight Card", "Card Nodes/Straight Card", color: "#80A4ED")]
    public class StraightCardNode : CodeGraphNode
    {
        [ExposedProperty()]
        public StoryCardTemplate card;

       

        public override bool GetStepInfo(out StepInfo stepInfo)
        {
            stepInfo = new StoryStep(card);
            return true;
        }

        

    }
}


