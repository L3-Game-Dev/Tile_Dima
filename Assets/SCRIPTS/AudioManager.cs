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
    private List<EventInstance> eventInstances;

    private EventInstance musicEventInstance;

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
    }

    private void Start()
    {
        InitialiseMusic();
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        StopTracks();
        InitialiseMusic();
    }

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

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void InitialiseTrack(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SwitchMusicTrack(MusicTrack track)
    {
        musicEventInstance.setParameterByName("MusicTrack", (float) track);
    }

    private void StopTracks()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
