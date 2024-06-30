using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SoundEventsData", menuName = "SoundEventsData")]
public class SoundEventsTemplate : ScriptableObject
{
    //public List<string> EventsNames = new List<string>();

    public List<EventIdentifier> EventsNames = new List<EventIdentifier>();
}

[System.Serializable]
public class EventIdentifier 
{
    public EventFolders FoldersName;
    public string EventName;


}