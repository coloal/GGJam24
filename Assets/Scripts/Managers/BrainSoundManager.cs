using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class SoundManager : MonoBehaviour
{
    /***** CONST *****/

    public const string FMOD_PATH = "event:/";

    /***** PARAMETERS *****/
    [SerializeField] private string StoryEventPath = "event:/Level1";
    [SerializeField] private string CampfireEventPath;
    [SerializeField] private string CombatEventPath;
    [SerializeField] private string GameOverEventPath = "event:/Music/GameOver";
    [SerializeField] private string CombatSoundsEventPath = "event:/SFX/CombatSounds";
    [SerializeField] private string CardSoundsEventPath = "event:/SFX/SwitchCard";
    [SerializeField] private string StepsEventPath = "event:/SFX/Steps";

    [SerializeField] private SoundEventsTemplate SoundEvents;

    [SerializeField] private MusicZonesTemplate MusicZoneData;
    [SerializeField] private float SpeedFadeIn = 0.2f;

    private MusicZones ActualZone;
    private SoundEvent ActualEvent;

    /***** DATA *****/

    private Dictionary<string, float> SoundsMap;
    private Dictionary<MusicZones, ZoneSoundValues> ZoneSoundsMap;

    private List<SoundAction> PendingActions;

    private static Dictionary<string, FMOD.Studio.EventInstance> EventMap;

    //List<> zones

    private FMOD.Studio.EventInstance StoryEventInstance;
    private FMOD.Studio.EventInstance CampfireInstance;
    private FMOD.Studio.EventInstance GameOverInstance;
    private FMOD.Studio.EventInstance CombatInstance;
    private FMOD.Studio.EventInstance CombatSoundInstance;
    private FMOD.Studio.EventInstance CardSoundsInstance;
    private FMOD.Studio.EventInstance StepsInstance;


    public static SoundManager Instance;

    /***** INITIALIZE *****/
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);

        //Initialize();

    }

    public void Initialize() 
    {
        InitializeData();
        //InitializeEventMap();
        EventMap = new Dictionary<string, EventInstance>();

        StoryEventInstance = FMODUnity.RuntimeManager.CreateInstance(StoryEventPath);
        //CampfireInstance = FMODUnity.RuntimeManager.CreateInstance(CampfireEventPath);
        GameOverInstance = FMODUnity.RuntimeManager.CreateInstance(GameOverEventPath);
        CardSoundsInstance = FMODUnity.RuntimeManager.CreateInstance(CardSoundsEventPath);
        StepsInstance = FMODUnity.RuntimeManager.CreateInstance(StepsEventPath);


        CombatSoundInstance = FMODUnity.RuntimeManager.CreateInstance(CombatSoundsEventPath);
    }
    void InitializeData()
    {
        if (SoundsMap == null)
        {
            SoundsMap = new Dictionary<string, float>();
            SoundsMap.Add(BrainSoundTag.Zone, 0.0f);
            SoundsMap.Add(BrainSoundTag.Acordeon, 0.0f);
            SoundsMap.Add(BrainSoundTag.Clavicordio, 0.0f);
            SoundsMap.Add(BrainSoundTag.Guitar, 0.0f);
            SoundsMap.Add(BrainSoundTag.Ness, 0.0f);
            SoundsMap.Add(BrainSoundTag.LetsFight, 0.0f);
            SoundsMap.Add(BrainSoundTag.FinBatalla, 0.0f);
            SoundsMap.Add(BrainSoundTag.Curse, 0.0f);
            SoundsMap.Add(BrainSoundTag.ApagarHoguera, 0.0f);
            SoundsMap.Add(BrainSoundTag.Golpes, 0.0f);




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

    void InitializeEventMap()
    {
        if (SoundEvents != null)
        {
            EventMap = new Dictionary<string, EventInstance>();
            foreach (EventIdentifier FMODevent in SoundEvents.EventsNames)
            {
                string path;
                if (FMODevent.FoldersName != EventFolders.snapshot)
                {
                    path = FMOD_PATH + Enum.GetName(typeof(EventFolders), FMODevent.FoldersName) + "/" + FMODevent.EventName;
                }
                else
                {
                    path = "snapshot:/" + FMODevent.EventName;
                }

                try
                {
                    FMOD.Studio.EventInstance FmodEventInstance = new EventInstance();
                    FmodEventInstance = FMODUnity.RuntimeManager.CreateInstance(path);
                    EventMap.Add(FMODevent.EventName, FmodEventInstance);
                }
                catch (EventNotFoundException)
                {
                    Debug.LogError("No se ha cargado correctamente el evento: " + path);
                    if (EventMap.ContainsKey(FMODevent.EventName))
                    {
                        EventMap.Remove(FMODevent.EventName);
                    }
                }

            }
        }
        else
        {
            Debug.LogError("No se ha cargado ninguna lista de eventos de FMOD");
        }
    }

    public void CreateEventFMOD(string EventName) 
    {
        if (SoundEvents != null)
        {
            if (EventMap == null)
            {
                EventMap = new Dictionary<string, EventInstance>();
            }
            if (!EventMap.ContainsKey(EventName))
            {

                EventIdentifier FMODevent = GetEvent(EventName);

                if (FMODevent != null) 
                {
                    string path;
                    if (FMODevent.FoldersName != EventFolders.snapshot)
                    {
                        path = FMOD_PATH + Enum.GetName(typeof(EventFolders), FMODevent.FoldersName) + "/" + FMODevent.EventName;
                    }
                    else
                    {
                        path = "snapshot:/" + FMODevent.EventName;
                    }

                    try
                    {
                        FMOD.Studio.EventInstance FmodEventInstance = new EventInstance();
                        FmodEventInstance = FMODUnity.RuntimeManager.CreateInstance(path);
                        EventMap.Add(FMODevent.EventName, FmodEventInstance);
                    }
                    catch (EventNotFoundException)
                    {
                        Debug.LogError("No se ha cargado correctamente el evento: " + path);
                        if (EventMap.ContainsKey(FMODevent.EventName))
                        {
                            EventMap.Remove(FMODevent.EventName);
                        }
                    }
                }
                else
                {
                    Debug.LogError("No existe el evento" + EventName);
                }
            }
        }
        else
        {
            Debug.LogError("No se ha cargado ninguna lista de eventos de FMOD");
        }
    }

    private EventIdentifier GetEvent(string eventName) 
    {
        EventIdentifier result = null;

        for (int i = 0; i < SoundEvents.EventsNames.Count; i++)
        {
            if (SoundEvents.EventsNames[i].EventName == eventName)
            {
                result = SoundEvents.EventsNames[i];
            }
        }

        return result;
    }

    /***** ACTIONS *****/
    public void PlaySFX(string EventName)
    {
        if (!EventMap.ContainsKey(EventName)) 
        {
            CreateEventFMOD(EventName);
        }
        if (EventMap.ContainsKey(EventName))
        {
            EventMap[EventName].start();
        }
        else
        {

            Debug.LogError("No esta registrado el evento de FMOD: " + EventName);
        }
    }

    public void StopSFX(string EventName)
    {
        if (EventMap.ContainsKey(EventName))
        {
            EventMap[EventName].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            Debug.LogError("No esta registrado el evento de FMOD: " + EventName);
        }
    }

    public void PlayCombatSFX()
    {

        CombatTypes type = GameManager.Instance.ProvideBrainManager().GetTypeLasWinnerCard();
        string AttackType = "";

        if (type == CombatTypes.Violence)
        {
            AttackType = "HitViolence";
        }
        else if (type == CombatTypes.Influence)
        {
            AttackType = "HitInfluence";
        }
        else
        {
            AttackType = "HitMoney";
        }

        PlaySFX(AttackType);
    }

    public void PlayCoinSFX() 
    {
        string eventName = "Coin";
        float coinResult = GameManager.Instance.ProvideBrainManager().GetCoinResult();
        if (!EventMap.ContainsKey(eventName))
        {
            CreateEventFMOD(eventName);
        }
        if (EventMap.ContainsKey(eventName))
        {
            EventMap[eventName].setParameterByName("SideWin", coinResult);
            EventMap[eventName].start();
        }
        else
        {
            Debug.LogError("No esta registrado el evento de FMOD: " + eventName);
        }
    }

    public void PlayDialogSFX(float value, float pitch = 0.5f) 
    {
        string eventName = "IndexDIalogue";
        if (!EventMap.ContainsKey(eventName))
        {
            CreateEventFMOD(eventName);
        }
        if (EventMap.ContainsKey(eventName))
        {
            EventMap[eventName].setParameterByName("DialoguePitch", pitch);
            EventMap[eventName].setParameterByName("DialogueStyle", value);
            EventMap[eventName].start();
        }
        else
        {
            Debug.LogError("No esta registrado el evento de FMOD: " + eventName);
        }

    }


    float dialogIndex = 0.0f;
    private void TestDialog() 
    {
        PlayDialogSFX(0, dialogIndex);
        Debug.Log("Indice actual dialogo: " + dialogIndex);
        dialogIndex += 0.2f;
    }

    public void SetMood(int newMood) 
    {
        StoryEventInstance.setParameterByName("Mood", newMood);
        
        //TODO IMPORTANTE ELIMINAR ESTO unicamente es para DEBUG
        //TestDialog();
    }

    public void SetAlarmTime(int alarmValue) 
    {
        StoryEventInstance.setParameterByName("AlarmTime", alarmValue);
    }

    public void PlayMenuMusic() 
    {
        string eventName = "Menu";
        if (!EventMap.ContainsKey(eventName))
        {
            CreateEventFMOD(eventName);
        }
        if (EventMap.ContainsKey(eventName))
        {
            EventMap[eventName].setParameterByName("ExitMenu", 0);
            EventMap[eventName].start();
        }
    }

    public void StopMenuMusic()
    {
        if (EventMap.ContainsKey("Menu"))
        {
            EventMap["Menu"].setParameterByName("ExitMenu", 1);
        }
        
        //EventMap["Menu"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StartGame(MusicZones zone = MusicZones.Dream)
    {
        /*ActualZone = zone;
        ActualEvent = ZoneSoundsMap[zone].FMODEvent;
        
        SetZoneParameters(ActualEvent, ActualZone);
        */

        ChangeZone(zone);
        if (zone != MusicZones.Credits)
        {
            GetActualEventInstance().start();        
        }

    }

    public void ExecuteSoundAction(SoundAction Action)
    {
        /*
        AddSoundAction(Action);
        /**/
        SetStorySound(Action.SoundTag, 1f);
    }

    public void ChangeZone(MusicZones zone)
    {
        if (zone != MusicZones.Credits && zone != MusicZones.GameOver)
        {
            if (zone != MusicZones.Dream && zone != MusicZones.Settlement)
            {
                StepsInstance.start();
            }
            
            SetStorySound(BrainSoundTag.Zone, (int)zone);

            if (zone == MusicZones.Settlement)
            {
                SetStorySound(BrainSoundTag.ApagarHoguera, 0.0f);
            }
            else if (SoundsMap[BrainSoundTag.ApagarHoguera] != 1.0f)
            {
                SetStorySound(BrainSoundTag.ApagarHoguera, 1.0f);
            }
        }
        else
        {
            //Paramos la musica normal
            StoryEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

            if (zone == MusicZones.Credits) 
            {
                if (!EventMap.ContainsKey("Credits"))
                {
                    CreateEventFMOD("Credits");
                }
                if (EventMap.ContainsKey("Credits"))
                {
                    EventMap["Credits"].setParameterByName("ExitCredits", 0.0f);
                    PlaySFX("Credits");
                }
                
            }
            else if (zone == MusicZones.GameOver)
            {
                PlaySFX("GameOver");
            }


        }
        
    }

    public void StopLevelMusic() 
    {
        StoryEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopCreditsMusic()
    {
        if (!EventMap.ContainsKey("Credits"))
        {
            CreateEventFMOD("Credits");
        }
        if (EventMap.ContainsKey("Credits"))
        {
            EventMap["Credits"].setParameterByName("ExitCredits", 1.0f);
        }
    }

    public void ResetNess()
    {
        SetStorySound(BrainSoundTag.Ness, 0.0f);
    }

    public void StartCombat(List<CombatCardTemplate> members, bool bIsBossFight = false)
    {
        //Valor por defecto para que comience el combate
        int CombatValue = 1;
        if (bIsBossFight || GameManager.Instance.ProvideBrainManager().bIsBossFight)
        {
            CombatValue = 2;
        }
        else if (GameManager.Instance.ProvideBrainManager().IsTutorial)
        {
            CombatValue = 3;
        }


        SetStorySound(BrainSoundTag.FinBatalla, 0.0f);
        SetStorySound(BrainSoundTag.LetsFight, CombatValue);

    }

    public void EndCombat(bool bIsBossFight = false)
    {
        //Valor por defecto para que comience el combate
        int CombatValue = 1;
        if (GameManager.Instance.ProvideBrainManager().bIsBossFight)
        {
            CombatValue = 2;
        }
        else if (GameManager.Instance.ProvideBrainManager().IsTutorial)
        {
            CombatValue = 3;
        }

        SetStorySound(BrainSoundTag.FinBatalla, CombatValue);

    }

    public void RestartMusicFromCombat()
    {
        SetStorySound(BrainSoundTag.LetsFight, 0.0f);
    }



    public void StartGameOver()
    {
        GameOverInstance.start();

    }

    public void EndGameOver()
    {
        
        GameOverInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }

    public void PlaySoundCombat(AttackEffectiveness effectiveness)
    {
        switch (effectiveness)
        {
            case AttackEffectiveness.NOT_VERY_EFFECTIVE:
                CombatSoundInstance.setParameterByNameWithLabel(BrainSoundTag.Golpes, CombatSounds.Flojo);
                break;
            case AttackEffectiveness.NEUTRAL:
                CombatSoundInstance.setParameterByNameWithLabel(BrainSoundTag.Golpes, CombatSounds.Medio);
                break;
            case AttackEffectiveness.SUPER_EFFECTIVE:
                CombatSoundInstance.setParameterByNameWithLabel(BrainSoundTag.Golpes, CombatSounds.Fuerte);
                break;
        }

        CombatSoundInstance.start();

        /*
        GameUtils.CreateTemporizer(() =>
        {
            CombatSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }, 0.75f, this);
        */
    }

    public void PlayCardSound(CardSounds type)
    {
        CardSoundsInstance.setParameterByNameWithLabel(BrainSoundTag.Cards, type.ToString());

        /*
        switch (type)
        {
            case CardSounds.Center:
                CardSoundsInstance.setParameterByNameWithLabel(BrainSoundTag.Cards, CardSounds.Center.ToString());
                break;
            case CardSounds.Left:
                CardSoundsInstance.setParameterByNameWithLabel(BrainSoundTag.Cards, CardSounds.Left.ToString());
                break;
            case CardSounds.Right:
                CardSoundsInstance.setParameterByNameWithLabel(BrainSoundTag.Cards, CardSounds.Right.ToString());
                break;
            case CardSounds.Phone:
                CardSoundsInstance.setParameterByNameWithLabel(BrainSoundTag.Cards, CardSounds.Phone.ToString());
                break;
        }
        */
        CardSoundsInstance.start();


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



        /*
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Acordeonista);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Bajista);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Cavaquinhista);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Flautista);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Percusionista);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].ApagarHoguera);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].VolumenHoguera);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Epic);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Fight);
        GetActualEventInstance().setParameterByName(BrainSoundTag., ZoneSoundsMap[zone].Zapato);
        */

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
            case SoundEvent.CombatEvent:
                return CombatInstance;
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
               
            case SoundEvent.CombatEvent:
                return CombatInstance;
               
        }

        Debug.LogError("El evento" + Event.ToString() + " de FMOD no esta configurado");
        return new EventInstance();
    }

    private void TurnOnCampfire(float volume = 1)
    {
        CampfireInstance.setParameterByName(BrainSoundTag.ApagarHoguera.ToString(), 0);
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
