using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BrainSoundManager : MonoBehaviour
{
    //TODO:Metodos start combat y end combat
    
    /***** PARAMETERS *****/
    [SerializeField] private string StoryEventPath = "event:/MaquetaAudioLeve";
    [SerializeField] private string CampfireEventPath = "event:/Settlement";
    [SerializeField] private string CombatEventPath;
    [SerializeField] private string GameOverEventPath = "event:/GameOver";
    
    [SerializeField] private MusicZonesTemplate MusicZoneData;
    [SerializeField] private float SpeedFadeIn = 0.2f;

    private MusicZones ActualZone;
    private SoundEvent ActualEvent;

    /***** DATA *****/

    private Dictionary<string, float> SoundsMap;
    private Dictionary<MusicZones, ZoneSoundValues> ZoneSoundsMap;

    private List<SoundAction> PendingActions;

    //List<> zones

    private FMOD.Studio.EventInstance StoryEventInstance;
    private FMOD.Studio.EventInstance CampfireInstance;
    private FMOD.Studio.EventInstance GameOverInstance;
    private FMOD.Studio.EventInstance CombatInstance;

    /***** INITIALIZE *****/
    void Awake()
    {
        InitializeData();

        StoryEventInstance = FMODUnity.RuntimeManager.CreateInstance(StoryEventPath);
        CampfireInstance = FMODUnity.RuntimeManager.CreateInstance(CampfireEventPath);
        GameOverInstance = FMODUnity.RuntimeManager.CreateInstance(GameOverEventPath);
    }

    void InitializeData()
    {
        if (SoundsMap == null)
        {
            SoundsMap = new Dictionary<string, float>();

            SoundsMap.Add(BrainSoundTag.Acordeonista, 0.0f);
            SoundsMap.Add(BrainSoundTag.Bajista, 0.0f);
            SoundsMap.Add(BrainSoundTag.Cavaquinhista, 0.0f);
            SoundsMap.Add(BrainSoundTag.Flautista, 0.0f);
            SoundsMap.Add(BrainSoundTag.Percusionista, 0.0f);
            SoundsMap.Add(BrainSoundTag.ApagarHoguera, 0.0f);
            SoundsMap.Add(BrainSoundTag.VolumenHoguera, 0.0f);
            SoundsMap.Add(BrainSoundTag.Epic, 0.0f);
            SoundsMap.Add(BrainSoundTag.Fight, 0.0f);
            SoundsMap.Add(BrainSoundTag.Zapato, 0.0f);

            /*foreach (BrainSoundTag tag in Enum.GetValues(typeof(BrainSoundTag)))
            {
                float DefaultValue = 0.0f;
                StoryEventInstance.getParameterByName(tag.ToString(), out DefaultValue);
                SoundsMap.Add(tag, DefaultValue);
            }*/
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
                float newValue = Mathf.Lerp(SoundsMap[PendingActions[i].SoundTag], PendingActions[i].NewValue, SpeedFadeIn * Time.deltaTime);
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
    public void StartGame(MusicZones zone = MusicZones.YourSettlement)
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
                /*
                 GetActualEventInstance().stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                 ActualZone = zone;
                 ActualEvent = ZoneSoundsMap[zone].FMODEvent;

                 SetZoneParameters(ActualEvent, ActualZone);
                 GetActualEventInstance().start();*/

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
                } /**/

                //*
                if (zone == MusicZones.Prueba)
                {
                    StartCombat();
                }
                else if (ActualZone == MusicZones.Prueba)
                {
                    EndCombat();
                } /**/


                ActualZone = zone;
                SetZoneParameters(ActualEvent, ActualZone);

                //GetActualEventInstance().start();
            }

        }
    }

    public void StartCombat()
    {
        StoryEventInstance.setPaused(true);

        /*cogemos el CombatInstance y seteamos los parametros seg�n los party members

        //CombatInstance.setParameterByName("",0.0f);
        //CombatInstance.start();

        */

    }

    public void EndCombat()
    {
        StoryEventInstance.setPaused(false); 
        //CombatInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StartGameOver()
    {
        GameOverInstance.start();

    }

    public void EndGameOver()
    {
        
        GameOverInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }
    /***** QUERIES *****/

    //Mete un instrumento con fade In (se realiza en el update)
    public void AddSoundAction(SoundAction Action, float InitialValue = 0.0f)
    {
        SoundsMap[Action.SoundTag] = InitialValue;
        
        //Contiene el instrumento a meter con su valor objetivo;
        PendingActions.Add(Action);
    }

    public void SetStorySound(string tag, float NewValue)
    {
        SoundsMap[tag] = NewValue;
        StoryEventInstance.setParameterByName(tag, NewValue);
    }

    public float GetSound(string tag)
    {
        return SoundsMap[tag];
    }

    private void SetZoneParameters(SoundEvent Event, MusicZones zone)
    {
        //Importante coger el eventInstance que corresponda a cada parametro
        // Ejemplo: StoryEventInstance.setParameterByName(BrainSoundTag.Acordeonista, ZoneSoundsMap[zone].Acordeonista);

        GetActualEventInstance().setParameterByName(BrainSoundTag.Acordeonista, ZoneSoundsMap[zone].Acordeonista);
        GetActualEventInstance().setParameterByName(BrainSoundTag.Bajista, ZoneSoundsMap[zone].Bajista);
        GetActualEventInstance().setParameterByName(BrainSoundTag.Cavaquinhista, ZoneSoundsMap[zone].Cavaquinhista);
        GetActualEventInstance().setParameterByName(BrainSoundTag.Flautista, ZoneSoundsMap[zone].Flautista);
        GetActualEventInstance().setParameterByName(BrainSoundTag.Percusionista, ZoneSoundsMap[zone].Percusionista);
        GetActualEventInstance().setParameterByName(BrainSoundTag.ApagarHoguera, ZoneSoundsMap[zone].ApagarHoguera);
        GetActualEventInstance().setParameterByName(BrainSoundTag.VolumenHoguera, ZoneSoundsMap[zone].VolumenHoguera);
        GetActualEventInstance().setParameterByName(BrainSoundTag.Epic, ZoneSoundsMap[zone].Epic);
        GetActualEventInstance().setParameterByName(BrainSoundTag.Fight, ZoneSoundsMap[zone].Fight);
        GetActualEventInstance().setParameterByName(BrainSoundTag.Zapato, ZoneSoundsMap[zone].Zapato);

        /*  importante
        Para parametros que sean sound Actions:

        if(SoundsMap[BrainSoundTag.Zapato] != 0)
        {
            GetActualEventInstance().setParameterByName(BrainSoundTag.Zapato, SoundsMap[BrainSoundTag.Zapato]);    
        }

         */

    }

    //Aviso de adri del pasado: De momento coger el evento manualmente y luego ya veremos para automatizarlo
    private FMOD.Studio.EventInstance GetActualEventInstance()
    {
        switch (ActualEvent)
        {
            case SoundEvent.StoryEvent1:
                return StoryEventInstance;
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
                return StoryEventInstance;
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
        CampfireInstance.setParameterByName(BrainSoundTag.ApagarHoguera.ToString(), 1);
        GameUtils.CreateTemporizer(() => StopCampfire(), 1f, this);

    }

    public void StopCampfire()
    {
        CampfireInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
