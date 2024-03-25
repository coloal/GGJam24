using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Rendering;

public class BrainSoundManager : MonoBehaviour
{
    //TODO: nuevos nodos de sonido / pasar a lista ordenada los intrumentos y que no esten tan hardcode
    //TODO: probar / multiplicador de numero de integrantes


    /***** PARAMETERS *****/
    [SerializeField] private string StoryEventPath = "event:/MaquetaAudioLeve";
    [SerializeField] private string CampfireEventPath = "event:/Settlement";
    [SerializeField] private string CombatEventPath;
    [SerializeField] private MusicZonesTemplate MusicZoneData;
    [SerializeField] private float Speed = 0.2f;

    private MusicZones ActualZone;
    private SoundEvent ActualEvent;

    /***** DATA *****/

    private Dictionary<BrainSoundTag, float> SoundsMap;
    private Dictionary<MusicZones, ZoneSoundValues> ZoneSoundsMap;

    private List<SoundAction> PendingActions;

    //List<> zones

    private FMOD.Studio.EventInstance StoryInstance;
    private FMOD.Studio.EventInstance CampfireInstance;
    private FMOD.Studio.EventInstance CombatInstance;

    /***** INITIALIZE *****/
    void Awake()
    {
        InitializeData();

        StoryInstance = FMODUnity.RuntimeManager.CreateInstance(StoryEventPath);
        CampfireInstance = FMODUnity.RuntimeManager.CreateInstance(CampfireEventPath);
    }

    void InitializeData()
    {
        if (SoundsMap == null)
        {
            SoundsMap = new Dictionary<BrainSoundTag, float>();
            foreach (BrainSoundTag tag in Enum.GetValues(typeof(BrainSoundTag)))
            {
                float DefaultValue = 0.0f;
                StoryInstance.getParameterByName(tag.ToString(), out DefaultValue);
                SoundsMap.Add(tag, DefaultValue);
            }
        }

        if (ZoneSoundsMap == null)
        {
            ZoneSoundsMap = new Dictionary<MusicZones, ZoneSoundValues>();
            foreach (ZoneSoundValues ZoneData in MusicZoneData.Zones)
            {
                ZoneSoundsMap.Add(ZoneData.zone, ZoneData);
            }
        }

        PendingActions = new List<SoundAction>();

    }

    void Update()
    {
        if (PendingActions.Count > 0)
        {
            for (int i = 0; i < PendingActions.Count; i++)
            {
                float newValue = Mathf.Lerp(SoundsMap[PendingActions[i].SoundTag], PendingActions[i].NewValue, Speed * Time.deltaTime);
                SetStorySound(PendingActions[i].SoundTag, newValue);
                if (SoundsMap[PendingActions[i].SoundTag] >= PendingActions[i].NewValue - 0.02f)
                {
                    PendingActions.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    /***** ACTIONS *****/
    public void StartGame(MusicZones zone = MusicZones.AdmiredTown)
    {
        ActualZone = zone;
        ActualEvent = ZoneSoundsMap[zone].FMODEvent;

        SetZoneParameters(ActualEvent, ActualZone);
        GetActualEventInstance().start();
    }

    public void ExecuteSoundAction(SoundAction Action)
    {
        AddSoundAction(Action);
    }

    public void ChangeZone(MusicZones zone)
    {
        if (ActualZone != zone)
        {
            if (ActualEvent != ZoneSoundsMap[zone].FMODEvent)
            {
                GetActualEventInstance().stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                ActualZone = zone;
                ActualEvent = ZoneSoundsMap[zone].FMODEvent;

                SetZoneParameters(ActualEvent, ActualZone);
                GetActualEventInstance().start();

            }
            else
            {
                //GetActualEventInstance().stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //*
                 if (zone == MusicZones.Campfire)
                {
                    TurnOnCampfire();

                }
                else if (ActualZone == MusicZones.Campfire)
                {
                    TurnOffCampfire();
                }/**/

                ActualZone = zone;
                SetZoneParameters(ActualEvent, ActualZone);
                
                //GetActualEventInstance().start();
            }

        }
    }

    /***** QUERIES *****/
    public void AddSoundAction(SoundAction Action)
    {
        Debug.Log("Entra " + Action.SoundTag.ToString());
        SoundsMap[Action.SoundTag] = 0f;
        PendingActions.Add(Action);
    }

    public void SetStorySound(BrainSoundTag tag, float NewValue)
    {
        SoundsMap[tag] = NewValue;
        StoryInstance.setParameterByName(tag.ToString(), NewValue);
    }

    public float GetSound(BrainSoundTag tag)
    {
        return SoundsMap[tag];
    }

    private void SetZoneParameters(SoundEvent Event, MusicZones zone)
    {
        //Example line:
        //GetActualEventInstance().setParameterByName("", ZoneSoundsMap[zone].);

        //Se podria optimizar con los valores en una lista en vez de en float diferentes ?? 

        for (int i = 0; i < ZoneSoundsMap[zone].soundActions.Count; i++)
        {
            GetActualEventInstance().setParameterByName(ZoneSoundsMap[zone].soundActions[i].SoundTag.ToString(),
                ZoneSoundsMap[zone].soundActions[i].NewValue);
        }

        /*
        GetActualEventInstance().setParameterByName("Acordeonista", ZoneSoundsMap[zone].Acordeonista);
        GetActualEventInstance().setParameterByName("Bajista", ZoneSoundsMap[zone].Bajista);
        GetActualEventInstance().setParameterByName("Cavaquinhista", ZoneSoundsMap[zone].Cavaquinhista);
        GetActualEventInstance().setParameterByName("Epic", ZoneSoundsMap[zone].Epic);
        GetActualEventInstance().setParameterByName("Fight", ZoneSoundsMap[zone].Fight);
        GetActualEventInstance().setParameterByName("Flautista", ZoneSoundsMap[zone].Flautista);
        GetActualEventInstance().setParameterByName("Percusionista", ZoneSoundsMap[zone].Percusionista);
        GetActualEventInstance().setParameterByName("Zapato", ZoneSoundsMap[zone].Zapato);
        */
    }


    private FMOD.Studio.EventInstance GetActualEventInstance()
    {
        switch (ActualEvent)
        {
            case SoundEvent.StoryEvent1:
                return StoryInstance;
                break;
            case SoundEvent.CombatEvent:
                return CombatInstance;
                break;
        }

        Debug.LogError("El evento actual de FMOD no es valido");
        return new EventInstance();
    }

    private FMOD.Studio.EventInstance GetEventInstance(SoundEvent Event)
    {
        switch (Event)
        {
            case SoundEvent.StoryEvent1:
                return StoryInstance;
                break;
            case SoundEvent.CombatEvent:
                return CombatInstance;
                break;
        }

        Debug.LogError("El evento" + Event.ToString() + " de FMOD no esta configurado");
        return new EventInstance();
    }

    private void TurnOnCampfire(float volume = 1)
    {
        CampfireInstance.setParameterByName(BrainSoundTag.VolumenHoguera.ToString(), volume);
        CampfireInstance.start();
    }

    private void TurnOffCampfire()
    {
        CampfireInstance.setParameterByName(BrainSoundTag.ApagarHogera.ToString(), 1);
        //GameUtils.createTemporizer(() => StopCampfire(), 1f, this);

    }

    public void StopCampfire()
    {
        CampfireInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
