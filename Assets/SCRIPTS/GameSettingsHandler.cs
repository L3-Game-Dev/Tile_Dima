// GameSettingsHandler
// Handles all global game settings
// Created by Dima Bethune 01/07

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSettingsHandler : MonoBehaviour
{
    public static bool settingsInitialised;

    public static float difficulty = 1f;

    public static float sensitivity;
    public static bool fullscreen;

    /// <summary>
    /// Sets the current difficulty based on the chosen difficulty item from the main menu
    /// </summary>
    public void SetDifficulty()
    {
        difficulty = CheckDifficultyName(GameObject.Find("DifficultySlider").GetComponent<Slider>().value);
    }

    /// <summary>
    /// Converts a slider value to its respective difficulty value
    /// </summary>
    /// <param name="diff">The difficulty value to convert</param>
    /// <returns></returns>
    public static float CheckDifficultyName(float diff)
    {
        switch (diff)
        {
            case 0: // Easy
                return 1f;
            case 1: // Medium
                return 1.5f;
            case 2: // Hard
                return 2f;
            default:
                return 1f;
        }
    }

    /// <summary>
    /// Changes the sensitivity based on an InputField value
    /// </summary>
    /// <param name="inputField">The InputField to recieve the value from</param>
    public static void ChangeSensitivity(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int sens))
            sensitivity = sens;
    }

    /// <summary>
    /// Enables/disables fullscreen mode depending on a given toggle value
    /// </summary>
    /// <param name="toggle">The toggle to check</param>
    public static void ChangeFullscreen(Toggle toggle)
    {
        fullscreen = toggle.isOn;

        if (fullscreen)
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else
            Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    /// <summary>
    /// Initialises all game settings to default values
    /// </summary>
    public static void InitialiseGameSettings()
    {
        if (!settingsInitialised)
        {
            difficulty = 1f;
            sensitivity = 20f;
            fullscreen = true;
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

            settingsInitialised = true;
        }
    }
}
