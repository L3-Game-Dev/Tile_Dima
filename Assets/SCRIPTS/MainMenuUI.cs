// MainMenuUI
// Handles the main menu UI elements/navigation
// Created by Dima Bethune 17/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;

public class MainMenuUI : MonoBehaviour
{
    public string sceneName;
    [HideInInspector] public bool cutscenePlaying;
    public KeyCode skipKey;

    [Header("Main Screen")]
    public GameObject mainScreen;
    public GameObject playScreen;
    public GameObject settingsScreen;
    public GameObject creditsScreen;

    [Header("Settings Screen")]
    public TMP_InputField sensitivityField;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    [Header("Cutscene Screen")]
    public GameObject cutsceneScreen;

    [Header("Audio")]
    public EventInstance humAmbience;
    public EventInstance typing;

    private void Awake()
    {
        GameSettingsHandler.InitialiseGameSettings();
    }

    private void Start()
    {
        cutscenePlaying = false;
        mainScreen.SetActive(true);
        cutsceneScreen.SetActive(false);

        playScreen.SetActive(true);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        humAmbience = AudioManager.instance.CreateEventInstance(FMODEvents.instance.humAmbience);
        typing = AudioManager.instance.CreateEventInstance(FMODEvents.instance.typing);
    }

    private void Update()
    {
        if (cutscenePlaying && Input.GetKeyDown(skipKey))
        {
            LoadGame(); // Skip cutscene, load the game
        }
    }

    public void PressPlay()
    {
        playScreen.SetActive(true);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        AudioManager.instance.ClickSound2();
    }

    public void PressSettings()
    {
        playScreen.SetActive(false);
        settingsScreen.SetActive(true);
        creditsScreen.SetActive(false);
        UpdateGameSettingsDisplays();
        AudioManager.instance.ClickSound2();
    }

    public void PressCredits()
    {
        playScreen.SetActive(false);
        settingsScreen.SetActive(false);
        creditsScreen.SetActive(true);
        AudioManager.instance.ClickSound2();
    }

    public void PressQuit()
    {
        AudioManager.instance.ClickSound2();
        Application.Quit();
    }

    public void PressStart()
    {
        AudioManager.instance.ClickSound2();
        PlayCutscene();
    }

    public void PlayCutscene()
    {
        mainScreen.SetActive(false);
        cutsceneScreen.SetActive(true);
        cutscenePlaying = true;

        PlayCutsceneAudio();

        StartCoroutine(Typewrite(0.5f, 0.05f, 5f));
    }

    public void PlayCutsceneAudio()
    {
        // Stop playing music
        AudioManager.instance.StopMusic();

        PLAYBACK_STATE playback_state;

        // Play Hum Ambience
        humAmbience.getPlaybackState(out playback_state);
        if (playback_state.Equals(PLAYBACK_STATE.STOPPED))
            humAmbience.start();

        // Play typing sound
        typing.getPlaybackState(out playback_state);
        if (playback_state.Equals(PLAYBACK_STATE.STOPPED))
            typing.start();
    }

    public void UpdateGameSettingsDisplays()
    {
        sensitivityField.text = GameSettingsHandler.sensitivity.ToString();
        fullscreenToggle.isOn = GameSettingsHandler.fullscreen;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneName); // Load the game scene
    }

    /// <summary>
    /// Animates printing given text in a typewriter format
    /// </summary>
    /// <param name="startDelay">Delay before starts printing</param>
    /// <param name="interval">Delay between characters, excludes spaces</param>
    /// <param name="endDelay">Delay once line is printed, before starting next</param>
    /// <returns></returns>
    public IEnumerator Typewrite(float startDelay, float interval, float endDelay)
    {
        List<Transform> textTransforms = new List<Transform> { };
        foreach (Transform t in cutsceneScreen.transform.Find("TextLines"))
            textTransforms.Add(t); // Add all text transforms to refernece list

        foreach (Transform textTransform in textTransforms)
        {
            textTransform.gameObject.SetActive(true); // Set object to active

            TextMeshProUGUI t2 = textTransform.GetComponent<TextMeshProUGUI>();

            string text = t2.text; // Store text as string
            t2.text = ""; // Clear object text

            foreach (Transform other in cutsceneScreen.transform.Find("TextLines"))
            {
                if (other != textTransform) // Set other objects to inactive
                {
                    other.gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(startDelay); // Wait start delay

            foreach (char c in text)
            {
                t2.text += c; // Add character to text

                if (c != ' ') // Skip delay on spaces
                {
                    yield return new WaitForSeconds(interval); // Wait interval delay
                }
            }

            yield return new WaitForSeconds(endDelay); // Wait end delay

            if (textTransform.Equals(textTransforms[^1])) // If final iteration
            {
                LoadGame(); // Load the game
            }
        }
    }
}
