using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepInfo
{
    
}

public class CombatStep:StepInfo
{
    CombatCardTemplate combatCard;
    public CombatCardTemplate CombatCard => combatCard;
    public CombatStep(CombatCardTemplate combatCard)
    {
        this.combatCard = combatCard;
    }
}

public class StoryStep:StepInfo
{
    StoryCardTemplate storyCard;
    public StoryCardTemplate StoryCard => storyCard;
    public StoryStep(StoryCardTemplate storyCard)
    {
        this.storyCard = storyCard;
    }
}

public class ChangeZoneStep : StepInfo
{
    ZoneTemplate zone;
    public ZoneTemplate Zone => zone;
    public ChangeZoneStep(ZoneTemplate zoneTemplate)
    {
        this.zone = zoneTemplate;
    }
}

public class EndStep : StepInfo
{
 
}