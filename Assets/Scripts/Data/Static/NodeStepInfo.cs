using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepInfo
{
    
}

public class CombatStep:StepInfo
{
    EnemyTemplate enemy;
    bool isBossFigth;
    public EnemyTemplate Enemy => enemy;
    public bool IsBossFigth => isBossFigth;
    public CombatStep(EnemyTemplate enemy)
    {
        this.enemy = enemy;
        isBossFigth = false; 
    }
    public CombatStep(EnemyTemplate enemy, bool isBossFigth)
    {
        this.enemy = enemy;
        this.isBossFigth = isBossFigth;
    }

}

public class WaitStep : StepInfo
{
    float seconds;
    public float Seconds => seconds;
    
    public WaitStep(float seconds)
    {
        this.seconds = seconds;
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

public class MenuStep : StepInfo
{

}
public class RestartStep : StepInfo
{

}