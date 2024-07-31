// UIHandler
// Handles UI elements and their functionality
// Created by Dima Bethune 07/06

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UiHandler : MonoBehaviour
{
    [Header("Uncategorised")]
    public TimerHandler timer;
    public TextMeshProUGUI creditDisplay;

    [Header("Keycodes")]
    public KeyCode pauseKey;
    public KeyCode continueKey;

    [Header("General References")]
    public GameObject uiCanvas;
    public GameObject interactPrompt;
    public GameObject crosshair;
    public GameObject hitmarker;
    public GameObject victoryScreen;
    public GameObject defeatScreen;

    [Header("Pause Menu References")]
    public GameObject pauseMenu;
    public GameObject pauseScreen;
    public GameObject settingsScreen;
    public GameObject creditsScreen;

    [Header("Settings Menu References")]
    public TMP_InputField sensitivityField;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    [Header("Interactable References")]
    public GameObject doorConsolePanel;
    public GameObject passwordPanel;

    [Header("Upgrader References")]
    public GameObject weaponUpgraderPanel;
    public Image upgraderWeaponDisplayImage;
    public TextMeshProUGUI upgraderWeaponDisplayText;
    public TextMeshProUGUI weaponStat1;
    public TextMeshProUGUI weaponStat2;
    public TextMeshProUGUI weaponStat3;
    public TextMeshProUGUI weaponStat4;
    public Button upgradeButton1;
    public Button upgradeButton2;
    public Button upgradeButton3;
    public Button upgradeButton4;

    [Header("Note References")]
    public GameObject noteImage;
    public GameObject code1Image;

    [Header("EnterName Screen References")]
    public GameObject enterNameScreen;
    public TextMeshProUGUI enteredName;

    [Header("Statistic Screen References")]
    public GameObject statisticsScreen;
    public Transform statisticContainer;
    public GameObject statisticItemPrefab;

    [Header("Highscore Screen References")]
    public GameObject highscoresScreen;
    public Transform highscoreContainer;
    public GameObject highscoreItemPrefab;

    [Header("Stat Bar References")]
    public GameObject healthBar;
    public Slider healthBarSlider;
    public TextMeshProUGUI healthBarNumber;
    public GameObject staminaBar;
    public Slider staminaBarSlider;
    public TextMeshProUGUI staminaBarNumber;

    [Header("Boss Bar References")]
    public GameObject bossBar;
    public Slider bossBarSlider;
    public TextMeshProUGUI bossBarNumber;
    public GameObject activeBoss;

    [Header("Weapon Display References")]
    public GameObject weaponDisplay1;
    public GameObject weaponDisplay2;
    public GameObject weaponDisplay3;
    public GameObject weaponDisplay4;

    public Image weaponDisplay1Image;
    public Image weaponDisplay2Image;
    public Image weaponDisplay3Image;
    public Image weaponDisplay4Image;

    public TextMeshProUGUI weaponDisplay1AmmoNumber;
    public TextMeshProUGUI weaponDisplay2AmmoNumber;
    public TextMeshProUGUI weaponDisplay3AmmoNumber;
    public TextMeshProUGUI weaponDisplay4AmmoNumber;

    [Header("Scene Name References")]
    public string mainMenuSceneName;

    [Header("Variables")]
    [Tooltip("Time (seconds) to show hitmarker for")]
    public float hitmarkerShowTime;

    private void Awake()
    {
        GameSettingsHandler.InitialiseGameSettings();
    }

    private void Start()
    {
        // Enable elements that should be shown
        ToggleMultiUI(true, new GameObject[] { crosshair });

        // Disable elements that should be hidden
        ToggleMultiUI(false, new GameObject[] { interactPrompt, doorConsolePanel, hitmarker,
                                                pauseMenu, victoryScreen, defeatScreen,
                                                statisticsScreen, highscoresScreen, weaponUpgraderPanel,
                                                enterNameScreen, bossBar, passwordPanel, noteImage });

        GameStateHandler.Resume();
    }

    private void Update()
    {
        // Pause Button Input
        if (Input.GetKeyDown(pauseKey)) { PauseButton(); }

        // Continue Button Input
        if (Input.GetKeyDown(continueKey))
        {
            if (GameStateHandler.gameState == "DEFEAT" ||
                GameStateHandler.gameState == "VICTORY")
            { StatisticsScreen(); } // Go to statistics screen

            else if (GameStateHandler.gameState == "STATISTICS")
            {
                if (GameObject.Find("PlayerCapsule").GetComponent<PlayerStats>().isDead)
                    HighscoresScreen();  // Go to highscore screen
                else
                    EnterNameScreen(); // Go to enterName screen
            }

            else if (GameStateHandler.gameState == "ENTERNAME")
            {
                if (CheckEnteredName())
                {
                    HighscoreStorer.SaveHighscore(enteredName.text.Trim((char)8203), StatisticsTracker.finalTime);
                    HighscoresScreen(); // Go to highscore screen
                }
                else
                    Debug.Log("NAME CANNOT BE EMPTY");
            }

            else if (GameStateHandler.gameState == "HIGHSCORES")
            { Quit(); } // Go back to main menu
        }

        // Update heldCredits display
        creditDisplay.text = GameObject.Find("PlayerCapsule").GetComponent<PlayerInventory>().heldCredits.ToString();

        // Update bossBar
        if (activeBoss != null)
        {
            MinibossStats minibossStats = activeBoss.GetComponent<MinibossStats>();
            float maxHealth = minibossStats.maxHealth;
            float currentHealth = minibossStats.health;
            bossBarSlider.maxValue = maxHealth;
            bossBarSlider.value = currentHealth;
            bossBarNumber.text = currentHealth.ToString();
        }
    }

    /// <summary>
    /// Toggles the visibility of a single ui element provided
    /// </summary>
    /// <param name="b">true to enable, false to disable</param>
    /// <param name="ui">Element to enable/disable</param>
    public void ToggleUI(bool b, GameObject ui)
    {
        ui.SetActive(b); // Set given object active/inactive based on given bool
    }

    /// <summary>
    /// Toggles the visibility of all ui element provided
    /// </summary>
    /// <param name="b">true to enable, false to disable</param>
    /// <param name="ui">Elements to enable/disable | USAGE: new GameObject[] { obj1, obj2... }</param>
    public void ToggleMultiUI(bool b, GameObject[] ui)
    {
        foreach (GameObject element in ui)
        {
            element.SetActive(b);
        }
    }

    public void UpdateInventoryUI(List<Weapon> weapons)
    {
        switch (weapons.Count)
        {
            case 1:
                weaponDisplay4Image.sprite = weapons[0].weaponSprite;
                ToggleMultiUI(false, new GameObject[] { weaponDisplay1, weaponDisplay2, weaponDisplay3 });
                break;
            case 2:
                weaponDisplay4Image.sprite = weapons[1].weaponSprite;
                weaponDisplay3Image.sprite = weapons[0].weaponSprite;
                ToggleMultiUI(false, new GameObject[] { weaponDisplay1, weaponDisplay2 });
                break;
            case 3:
                weaponDisplay4Image.sprite = weapons[2].weaponSprite;
                weaponDisplay3Image.sprite = weapons[1].weaponSprite;
                weaponDisplay2Image.sprite = weapons[0].weaponSprite;
                ToggleMultiUI(false, new GameObject[] { weaponDisplay1 });
                break;
            case 4:
                weaponDisplay4Image.sprite = weapons[3].weaponSprite;
                weaponDisplay3Image.sprite = weapons[2].weaponSprite;
                weaponDisplay2Image.sprite = weapons[1].weaponSprite;
                weaponDisplay1Image.sprite = weapons[0].weaponSprite;
                break;
            default:
                // Number of weapons is not between 1 & 4
                Debug.Log("INVALID # OF WEAPONS IN INVENTORY");
                break;
        }
    }

    public void UpdateAmmoNumberUI(List<Weapon> weapons)
    {
        // For every weapon inside weapons[]
        for (int j = 0; j < weapons.Count; j++)
        {
            // Create temporary reference list
            List<TextMeshProUGUI> l = new List<TextMeshProUGUI>() { weaponDisplay4AmmoNumber,
                                                                    weaponDisplay3AmmoNumber,
                                                                    weaponDisplay2AmmoNumber,
                                                                    weaponDisplay1AmmoNumber };
            UpdateUIText(l[j], weapons[j].ammo.ToString());
        }
    }

    public void UpdateUIText(TextMeshProUGUI t, string s)
    {
        t.text = s;
    }

    public void DisableHitmarker()
    {
        hitmarker.SetActive(false);
    }

    public void EnableBossBar(MinibossController minibossController)
    {
        activeBoss = minibossController.gameObject;
        ToggleUI(true, bossBar);
    }

    public void DisableBossBar()
    {
        activeBoss = null;
        ToggleUI(false, bossBar);
    }

    public void PlayerDeath()
    {
        DefeatScreen();
    }

    public void MinibossDeath()
    {
        DisableBossBar();
    }

    public void ResumeButton()
    {
        Resume();
    }

    public void BackButton()
    {
        Back();
    }

    public void SettingsButton()
    {
        Settings();
    }

    public void CreditsButton()
    {
        Credits();
    }

    public void QuitButton()
    {
        Quit();
    }

    public void PauseButton()
    {
        switch (GameStateHandler.gameState)
        {
            case "PLAYING":
                Pause();
                break;
            case "PAUSED":
                Resume();
                break;
        }
    }

    private void Pause()
    {
        ToggleMultiUI(true, new GameObject[] { pauseMenu, pauseScreen });
        ToggleMultiUI(false, new GameObject[] { settingsScreen, creditsScreen });
        UpdateGameSettingsDisplays();
        GameStateHandler.Pause();
    }

    private void Resume()
    {
        ToggleMultiUI(false, new GameObject[] { pauseMenu, doorConsolePanel, weaponUpgraderPanel, noteImage, passwordPanel });
        GameStateHandler.Resume();
    }

    private void Back()
    {
        ToggleMultiUI(false, new GameObject[] { settingsScreen, creditsScreen });
        ToggleUI(true, pauseScreen);
    }

    private void Settings()
    {
        ToggleUI(false, pauseScreen);
        ToggleUI(true, settingsScreen);
    }

    private void Credits()
    {
        ToggleUI(false, pauseScreen);
        ToggleUI(true, creditsScreen);
    }

    private void Quit()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void VictoryScreen()
    {
        ToggleUI(true, victoryScreen);
        StartCoroutine(FadeInScreen(victoryScreen, 1, 100f));
        GameStateHandler.Victory();
    }

    private void DefeatScreen()
    {
        ToggleUI(true, defeatScreen);
        StartCoroutine(FadeInScreen(defeatScreen, 1, 100f));
        GameStateHandler.Defeat();
    }

    private void UpdateGameSettingsDisplays()
    {
        sensitivityField.text = GameSettingsHandler.sensitivity.ToString();
        fullscreenToggle.isOn = GameSettingsHandler.fullscreen;
        volumeSlider.value = GameSettingsHandler.volume;
    }

    private IEnumerator FadeInScreen(GameObject screen, float goal, float duration)
    {
        Image image = screen.GetComponent<Image>();
        Color goalColor = image.color;
        goalColor.a = goal;

        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            float normalisedDuration = t / duration;
            image.color = Color.Lerp(image.color, goalColor, normalisedDuration);
        }
        image.color = goalColor;
        GameStateHandler.Pause();
        yield return null;
    }

    private void StatisticsScreen()
    {
        ToggleMultiUI(false, new GameObject[] { victoryScreen, defeatScreen });
        ToggleUI(true, statisticsScreen);
        GameStateHandler.Statistics();

        string tempTime = timer.time.ToString();
        StatisticsTracker.finalTime = tempTime.Remove(tempTime.Length-4, 4);

        List<Tuple<string, string>> statTuples = new List<Tuple<string, string>>{
            new Tuple<string, string>("Total Time", StatisticsTracker.finalTime),
            new Tuple<string, string>("Total Kills", StatisticsTracker.kills.ToString()),
            new Tuple<string, string>("Total Damage Dealt", StatisticsTracker.damageDealt.ToString()),
            new Tuple<string, string>("Total Damage Taken", StatisticsTracker.damageTaken.ToString())
        };

        for (int i = 0; i < statTuples.Count; i++)
        {
            Tuple<string, string> t = statTuples[i];

            GameObject item = Instantiate(statisticItemPrefab, statisticContainer);
            item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = t.Item1;
            item.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = t.Item2;

            RectTransform rt = statisticContainer.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 40 * (i + 1));
            rt.anchoredPosition = new Vector2(0, -20 * (i + 1));
        }
    }

    private void EnterNameScreen()
    {
        ToggleUI(false, statisticsScreen);
        ToggleUI(true, enterNameScreen);
        GameStateHandler.EnterName();
    }

    private bool CheckEnteredName()
    {
        string name = enteredName.text.Trim((char)8203);
        if (name != null && name != "") // Make sure entered name isnt empty
            return true;
        else
            return false;
    }

    private void HighscoresScreen()
    {
        ToggleMultiUI(false, new GameObject[] { statisticsScreen, enterNameScreen });
        ToggleUI(true, highscoresScreen);
        GameStateHandler.Highscores();

        for (int i = 0; i < HighscoreStorer.GetHighscoreCount(); i++)
        {
            Tuple<string, string> t = HighscoreStorer.GetHighscores(i);
            if (t != null)
            {
                GameObject item = Instantiate(highscoreItemPrefab, highscoreContainer);
                item.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = t.Item1;
                item.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = t.Item2;

                RectTransform rt = highscoreContainer.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, 40 * (i + 1));
                rt.anchoredPosition = new Vector2(0, -20 * (i + 1));
            }
        }
    }
}
