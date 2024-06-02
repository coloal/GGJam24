using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepInfo
{
    
}

public class CombatStep:StepInfo
{
    EnemyTemplate enemy;
    public EnemyTemplate Enemy => enemy;
    public CombatStep(EnemyTemplate enemy)
    {
        this.enemy = enemy;
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