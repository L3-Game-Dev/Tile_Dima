// AudioManager
// Handles playing audio
// Created by Dima Bethune 04/07

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Bus masterBus;

    private List<EventInstance> eventInstances;

    public EventInstance musicEventInstance;

    // Singleton functionality
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        // Initialise singleton
        if (instance != null)
        {
            Debug.Log("More than one AudioManager found");
        }
        else
        {
            instance = this;
        }

        eventInstances = new List<EventInstance>();

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        masterBus = RuntimeManager.GetBus("bus:/");
    }

    private void Update()
    {
        masterBus.setVolume(GameSettingsHandler.masterVolume);
    }

    /// <summary>
    /// Called whenever the active scene is changed (includes first load of the game)
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        StopTracks(true);
        InitialiseMusic();
    }

    /// <summary>
    /// Initialises a music track depending on the active scene
    /// </summary>
    public void InitialiseMusic()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            InitialiseTrack(FMODEvents.instance.mainMenuTrack);
        }
        else if (SceneManager.GetActiveScene().name.Equals("Level"))
        {
            InitialiseTrack(FMODEvents.instance.musicTrack1);
        }
    }

    /// <summary>
    /// Plays a provided audio clip once
    /// </summary>
    /// <param name="sound">The sound to play</param>
    /// <param name="worldPos">The world position to play it at</param>
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    /// <summary>
    /// Creates an EventInstance of the provided EventReference
    /// </summary>
    /// <param name="eventReference">The created EventInstance</param>
    /// <returns></returns>
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        // Create event instance
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);

        eventInstances.Add(eventInstance); // Add to list of events
        return eventInstance;
    }

    /* ----------------------- Start UI Sounds ----------------------------- */
    public void ClickSound1()
    {
        instance.PlayOneShot(FMODEvents.instance.uiClick1, transform.position);
    }

    public void ClickSound2()
    {
        instance.PlayOneShot(FMODEvents.instance.uiClick2, transform.position);
    }
    /* ------------------------ End UI Sounds ------------------------------ */

    /// <summary>
    /// Initialises a track from a given EventReference
    /// </summary>
    /// <param name="musicEventReference">The EventReference to initialise from</param>
    private void InitialiseTrack(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    /// <summary>
    /// Stops playing a provided EventInstance
    /// </summary>
    /// <param name="instance">The instance to stop</param>
    /// <param name="immediate">Whether to stop immediately</param>
    public void StopInstance(EventInstance instance, bool immediate = true)
    {
        FMOD.Studio.STOP_MODE stop_mode;
        if (immediate)
            stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE;
        else
            stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
        
        instance.stop(stop_mode);
    }

    /// <summary>
    /// Stops the currently playing music track
    /// </summary>
    public void StopMusic()
    {
        if (musicEventInstance.isValid())
        {
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            musicEventInstance.release();
        }
        else
        {
            Debug.LogWarning("Music event instance is not valid.");
        }
    }

    /// <summary>
    /// Switches the current music track
    /// </summary>
    /// <param name="track">The track to switch to</param>
    public void SwitchMusicTrack(MusicTrack track)
    {
        musicEventInstance.setParameterByName("MusicTrack", (float) track);
    }

    /// <summary>
    /// Displays the path of every EventInstance in the list into the console log
    /// </summary>
    public void DisplayInstances()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.getDescription(out EventDescription desc);
            desc.getPath(out string path);
            Debug.Log(path);
        }
    }

    /// <summary>
    /// Stops every EventInstance in the list
    /// </summary>
    /// <param name="release">Whether to release the tracks too</param>
    private void StopTracks(bool release = false)
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            if (release)
                eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        StopTracks(true);
    }
}
