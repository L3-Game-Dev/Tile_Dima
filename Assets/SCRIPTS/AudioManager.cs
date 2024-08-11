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
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    private Bus masterBus;

    private List<EventInstance> eventInstances;

    public EventInstance musicEventInstance;

    // Singleton functionality
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one AudioManager found");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        masterBus = RuntimeManager.GetBus("bus:/");
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
    }

    /// <summary>
    /// Called whenever the active scene is changed (includes first load of the game)
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        StopTracks();
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

    public void ClickSound1()
    {
        instance.PlayOneShot(FMODEvents.instance.uiClick1, transform.position);
    }

    public void ClickSound2()
    {
        instance.PlayOneShot(FMODEvents.instance.uiClick2, transform.position);
    }

    private void InitialiseTrack(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void StopInstance(EventInstance instance, bool immediate = true)
    {
        FMOD.Studio.STOP_MODE stop_mode;
        if (immediate)
            stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE;
        else
            stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
            
        Debug.Log(instance.stop(stop_mode));
    }

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
            eventInstance.getDescription(out FMOD.Studio.EventDescription desc);
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
