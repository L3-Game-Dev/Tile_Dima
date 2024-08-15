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

    public void SetDifficulty()
    {
        difficulty = CheckDifficultyName(GameObject.Find("DifficultyDropdown").transform.Find("Difficulty").Find("Label").GetComponent<TextMeshProUGUI>().text.ToString());
    }

    public static float CheckDifficultyName(string diffName)
    {
        switch (diffName)
        {
            case "EASY":
                return 1f;
            case "MEDIUM":
                return 1.5f;
            case "HARD":
                return 2f;
            default:
                return 1f;
        }
    }

    public static void ChangeSensitivity(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int sens))
            sensitivity = sens;
    }

    public static void ChangeFullscreen(Toggle toggle)
    {
        fullscreen = toggle.isOn;
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public static void InitialiseGameSettings()
    {
        if (!settingsInitialised)
        {
            difficulty = 1f;
            sensitivity = 100f;
            fullscreen = true;
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

            settingsInitialised = true;
        }
    }
}
