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
using System.Text.RegularExpressions;

public class UiHandler : MonoBehaviour
{
    // Singleton
    [HideInInspector] public static UiHandler instance { get; private set; }

    [Header("Uncategorised")]
    public TimerHandler timer;
    public TextMeshProUGUI creditDisplay;
    public TextMeshProUGUI objectiveDisplay;

    [Header("Keycodes")]
    public KeyCode pauseKey;
    public KeyCode continueKey;

    [Header("General References")]
    public GameObject uiCanvas;
    public GameObject interactPrompt;
    public GameObject crosshair;
    public GameObject hitmarker;
    public GameObject notification;
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    public GameObject bloodOverlay;

    [Header("Pause Menu References")]
    public GameObject pauseMenu;
    public GameObject pauseScreen;
    public GameObject settingsScreen;
    public GameObject controlsScreen;
    public GameObject creditsScreen;

    [Header("Settings Menu References")]
    public TMP_InputField sensitivityField;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;

    [Header("Interactable References")]
    public GameObject doorConsolePanel;
    public GameObject passwordPanel;

    [Header("Weapon Upgrader References")]
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

    [Header("Suit Upgrader References")]
    public GameObject suitUpgraderPanel;
    public Image upgraderSuitDisplayImage;
    public TextMeshProUGUI upgraderSuitDisplayText;
    public TextMeshProUGUI suitStat1;
    public TextMeshProUGUI suitStat2;
    public TextMeshProUGUI suitStat3;
    public TextMeshProUGUI suitStat4;
    public Button suitUpgradeButton1;
    public Button suitUpgradeButton2;
    public Button suitUpgradeButton3;
    public Button suitUpgradeButton4;

    [Header("Note References")]
    public GameObject noteImage;
    public GameObject code1Image;

    [Header("EnterName Screen References")]
    public GameObject enterNameScreen;
    public TextMeshProUGUI enteredName;
    public GameObject invalidName;
    public float invalidNameDuration;

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

    public Image staminaBar1;
    public Image healthBar1;

    [Header("Boss Bar References")]
    public GameObject bossBar;
    public Slider bossBarSlider;
    public TextMeshProUGUI bossBarNumber;
    public GameObject activeBoss;

    [Header("Weapon Display References")]
    public GameObject grenadeDisplay;
    public TextMeshProUGUI grenadeDisplayAmmoNumber;

    public GameObject weaponDisplay1;
    public GameObject weaponDisplay2;
    public GameObject weaponDisplay3;
    public GameObject weaponDisplay4;

    public Image weaponDisplay1Image;
    public Image weaponDisplay2Image;
    public Image weaponDisplay3Image;
    public Image weaponDisplay4Image;

    public TextMeshProUGUI weaponDisplay1Number;
    public TextMeshProUGUI weaponDisplay2Number;
    public TextMeshProUGUI weaponDisplay3Number;
    public TextMeshProUGUI weaponDisplay4Number;

    public TextMeshProUGUI weaponDisplay1Ammo;
    public TextMeshProUGUI weaponDisplay2Ammo;
    public TextMeshProUGUI weaponDisplay3Ammo;
    public TextMeshProUGUI weaponDisplay4Ammo;

    [Header("Reloading Display")]
    public Image reloadDisplay;

    [HideInInspector] public float reloadTime;
    [HideInInspector] public float reloadTimer;

    [Header("Scene Name References")]
    public string mainMenuSceneName;

    [Header("Variables")]
    [Tooltip("Time (seconds) to show hitmarker for")]
    public float hitmarkerShowTime;

    private void Awake()
    {
        // Set singleton instance
        instance = this;
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
                                                enterNameScreen, bossBar, passwordPanel, noteImage,
                                                notification, reloadDisplay.gameObject, suitUpgraderPanel });

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
                    HighscoreStorer.SaveHighscore(enteredName.text.Trim((char)8203).Trim(), StatisticsTracker.finalTime);
                    HighscoresScreen(); // Go to highscore screen
                }
                else
                {
                    ToggleUI(true, invalidName);
                    CancelInvoke("HideInvalidName");
                    Invoke("HideInvalidName", invalidNameDuration);
                }
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

        // Reloading display
        if (PlayerInventory.instance.equippedWeapon.reloading)
            ReloadDisplay();
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

    /// <summary>
    /// Updates the inventory UI
    /// </summary>
    /// <param name="weapons"></param>
    public void UpdateInventoryUI(List<Weapon> weapons)
    {
        switch (weapons.Count)
        {
            case 1:
                weaponDisplay4Image.sprite = weapons[0].weaponSprite;
                weaponDisplay4Number.text = "1";
                ToggleMultiUI(false, new GameObject[] { weaponDisplay1, weaponDisplay2, weaponDisplay3 });
                ToggleMultiUI(true, new GameObject[] { weaponDisplay4 });
                break;
            case 2:
                weaponDisplay4Image.sprite = weapons[1].weaponSprite;
                weaponDisplay3Image.sprite = weapons[0].weaponSprite;
                weaponDisplay4Number.text = "2";
                weaponDisplay3Number.text = "1";
                ToggleMultiUI(false, new GameObject[] { weaponDisplay1, weaponDisplay2 });
                ToggleMultiUI(true, new GameObject[] { weaponDisplay4, weaponDisplay3 });
                break;
            case 3:
                weaponDisplay4Image.sprite = weapons[2].weaponSprite;
                weaponDisplay3Image.sprite = weapons[1].weaponSprite;
                weaponDisplay2Image.sprite = weapons[0].weaponSprite;
                weaponDisplay4Number.text = "3";
                weaponDisplay3Number.text = "2";
                weaponDisplay2Number.text = "1";
                ToggleMultiUI(false, new GameObject[] { weaponDisplay1 });
                ToggleMultiUI(true, new GameObject[] { weaponDisplay4, weaponDisplay3, weaponDisplay2 });
                break;
            case 4:
                weaponDisplay4Image.sprite = weapons[3].weaponSprite;
                weaponDisplay3Image.sprite = weapons[2].weaponSprite;
                weaponDisplay2Image.sprite = weapons[1].weaponSprite;
                weaponDisplay1Image.sprite = weapons[0].weaponSprite;
                weaponDisplay4Number.text = "4";
                weaponDisplay3Number.text = "3";
                weaponDisplay2Number.text = "2";
                weaponDisplay1Number.text = "1";
                ToggleMultiUI(true, new GameObject[] { weaponDisplay4, weaponDisplay3, weaponDisplay2, weaponDisplay1 });
                break;
            default:
                // Number of weapons is not between 1 & 4
                Debug.Log("INVALID # OF WEAPONS IN INVENTORY");
                break;
        }
    }

    /// <summary>
    /// Updates the ammo number UI of each weapon provided
    /// </summary>
    /// <param name="weapons"></param>
    public void UpdateAmmoNumberUI(List<Weapon> weapons)
    {
            // Create temporary reference list
            List<TextMeshProUGUI> l =  new List<TextMeshProUGUI>();

            // Add ammo numbers to reference list, depending on number of weapons
            switch (weapons.Count)
            {
                case 1:
                    l = new List<TextMeshProUGUI>() { weaponDisplay4Ammo };
                    break;
                case 2:
                    l = new List<TextMeshProUGUI>() { weaponDisplay3Ammo,
                                                      weaponDisplay4Ammo };
                    break;
                case 3:
                    l = new List<TextMeshProUGUI>() { weaponDisplay2Ammo,
                                                      weaponDisplay3Ammo,
                                                      weaponDisplay4Ammo };
                    break;
                case 4:
                    l = new List<TextMeshProUGUI>() { weaponDisplay1Ammo,
                                                      weaponDisplay2Ammo,
                                                      weaponDisplay3Ammo,
                                                      weaponDisplay4Ammo };
                    break;
                default:
                    // Number of weapons is not between 1 & 4
                    Debug.Log("INVALID # OF WEAPONS IN INVENTORY");
                    break;
            }

        // For every weapon inside weapons[]
        for (int j = weapons.Count - 1; j >= 0; j--)
        {
            UpdateUIText(l[j], weapons[j].ammo.ToString());
        }
    }

    /// <summary>
    /// Sets a specified text component's text to the specified string
    /// </summary>
    /// <param name="t">The text component to change</param>
    /// <param name="s">The string to set the text to</param>
    public void UpdateUIText(TextMeshProUGUI t, string s)
    {
        t.text = s;
    }

    /// <summary>
    /// Disables the hitmarker UI
    /// </summary>
    public void DisableHitmarker()
    {
        hitmarker.SetActive(false);
    }

    /// <summary>
    /// Initialises the reloading display values
    /// </summary>
    public void StartReload()
    {
        // Initialise values
        reloadTime = PlayerInventory.instance.equippedWeapon.reloadTime;
        reloadTimer = reloadTime;
    }

    /// <summary>
    /// Shows & updates the reloading display
    /// </summary>
    public void ReloadDisplay()
    {
        // Update passed time
        reloadTimer -= Time.deltaTime;

        if (reloadTimer < 0f)
        {
            ToggleUI(false, reloadDisplay.gameObject);
            reloadDisplay.fillAmount = 0f;
        }
        else
        {
            ToggleUI(true, reloadDisplay.gameObject);
            reloadDisplay.fillAmount = reloadTimer / reloadTime;
        }
    }

    /// <summary>
    /// Shows a notification to the player
    /// </summary>
    /// <param name="message">The message to show</param>
    public void ShowNotification(string message, float time)
    {
        // Set the message string
        notification.GetComponent<TextMeshProUGUI>().text = message;

        // Show the UI object
        ToggleUI(true, notification);

        // Cancel previous invoke if exists
        CancelInvoke("HideNotification");
        // (re)invoke
        Invoke("HideNotification", time);
    }

    /// <summary>
    /// Hides the notification shown
    /// </summary>
    public void HideNotification()
    {
        // Hide the UI object
        ToggleUI(false, notification);
    }

    /// <summary>
    /// Enables the boss bar UI and sets active boss to provided miniboss
    /// </summary>
    /// <param name="minibossController"></param>
    public void EnableBossBar(MinibossController minibossController)
    {
        activeBoss = minibossController.gameObject;
        ToggleUI(true, bossBar);
    }

    /// <summary>
    /// Disables the boss bar and clears active boss
    /// </summary>
    public void DisableBossBar()
    {
        activeBoss = null;
        ToggleUI(false, bossBar);
    }

    /// <summary>
    /// Sets the objectiveDisplay to show a new objective
    /// </summary>
    /// <param name="objective">The new objective</param>
    public void NewObjective(string objective)
    {
        string text = "Current Objective: ";
        text += objective;
        objectiveDisplay.text = text;
    }

    /// <summary>
    /// Updates the blood overlay's transparency
    /// </summary>
    /// <param name="transparency"></param>
    public void UpdateBloodOverlay(float transparency)
    {
        // Get references
        Image bloodImage = bloodOverlay.GetComponent<Image>();
        Color newColor = bloodImage.color;

        // Set transparency
        if (transparency > 1)
            newColor.a = 1;
        else
            newColor.a = transparency;

        // Apply new color
        bloodImage.color = newColor;
    }

    /// <summary>
    /// Enables the blood overlay
    /// </summary>
    public void ShowBloodOverlay()
    {
        ToggleUI(true, bloodOverlay);
    }

    /// <summary>
    /// Disables the blood overlay
    /// </summary>
    public void HideBloodOverlay()
    {
        ToggleUI(false, bloodOverlay);
    }

    /// <summary>
    /// Shows the defeat screen
    /// </summary>
    public void PlayerDeath()
    {
        DefeatScreen();
    }

    /// <summary>
    /// Disables boss bar and shows victory screen
    /// </summary>
    public void MinibossDeath()
    {
        DisableBossBar();
    }

    /* ---- START UI BUTTONS ---- */
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

    public void ControlsButton()
    {
        Controls();
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
    /* ---- END UI BUTTONS ---- */

    /// <summary>
    /// Opens the pause menu and pauses the game
    /// </summary>
    private void Pause()
    {
        ToggleMultiUI(true, new GameObject[] { pauseMenu, pauseScreen });
        ToggleMultiUI(false, new GameObject[] { settingsScreen, creditsScreen });
        UpdateGameSettingsDisplays();
        GameStateHandler.Pause();
        AudioManager.instance.ClickSound1();
    }

    /// <summary>
    /// Closes the pause menu and resumes the game
    /// </summary>
    private void Resume()
    {
        ToggleMultiUI(false, new GameObject[] { pauseMenu, doorConsolePanel, weaponUpgraderPanel, noteImage, passwordPanel, suitUpgraderPanel });
        GameStateHandler.Resume();
        AudioManager.instance.ClickSound1();
    }

    /// <summary>
    /// Returns back to the main pause menu page
    /// </summary>
    private void Back()
    {
        ToggleMultiUI(false, new GameObject[] { settingsScreen, creditsScreen, controlsScreen });
        ToggleUI(true, pauseScreen);
        AudioManager.instance.ClickSound2();
    }

    /// <summary>
    /// Opens the settings page of pause menu
    /// </summary>
    private void Settings()
    {
        ToggleUI(false, pauseScreen);
        ToggleUI(true, settingsScreen);
        AudioManager.instance.ClickSound2();
    }

    /// <summary>
    /// Opens the controls page of pause menu
    /// </summary>
    private void Controls()
    {
        ToggleUI(false, settingsScreen);
        ToggleUI(true, controlsScreen);
        AudioManager.instance.ClickSound2();
    }

    /// <summary>
    /// Opens the credits page of pause menu
    /// </summary>
    private void Credits()
    {
        ToggleUI(false, pauseScreen);
        ToggleUI(true, creditsScreen);
        AudioManager.instance.ClickSound2();
    }

    /// <summary>
    /// Returns back to the main menu
    /// </summary>
    private void Quit()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Displays a victory screen and sets gamestate to victory
    /// </summary>
    public void VictoryScreen()
    {
        if (GameStateHandler.gameState == "PLAYING")
        {
            ToggleUI(true, victoryScreen);
            StartCoroutine(FadeScreen(victoryScreen, 1, 10f));
            AudioManager.instance.PlayOneShot(FMODEvents.instance.victory, transform.position);
            AudioManager.instance.SwitchMusicTrack(MusicTrack.VICTORY);
            GameStateHandler.Victory();
        }
    }

    /// <summary>
    /// Displays a defeat screen and sets gamestate to defeat
    /// </summary>
    private void DefeatScreen()
    {
        if (GameStateHandler.gameState == "PLAYING")
        {
            ToggleUI(true, defeatScreen);
            StartCoroutine(FadeScreen(defeatScreen, 1, 10f));
            AudioManager.instance.SwitchMusicTrack(MusicTrack.DEFEAT);
            GameStateHandler.Defeat();
        }
    }

    /// <summary>
    /// Sets the pause menu gamesettings displays to actual settings values
    /// </summary>
    private void UpdateGameSettingsDisplays()
    {
        sensitivityField.text = GameSettingsHandler.sensitivity.ToString();
        fullscreenToggle.isOn = GameSettingsHandler.fullscreen;
    }

    /// <summary>
    /// Fades in a provided screen over a specified duration
    /// </summary>
    /// <param name="screen">The screen to fade in</param>
    /// <param name="goal">The transparency to reach</param>
    /// <param name="duration">The duration to fade across</param>
    /// <returns></returns>
    private IEnumerator FadeScreen(GameObject screen, float goal, float duration)
    {
        // Initialisation
        Image image = screen.GetComponent<Image>();
        Color goalColor = image.color;
        goalColor.a = goal;

        for (float t = image.color.a; t < duration; t += 1 / duration)
        {
            yield return new WaitForSeconds(1 / duration);
            Color newColor = image.color;
            newColor.a = goal / duration + t;
            image.color = newColor;
        }

        image.color = goalColor;
    }

    /// <summary>
    /// Displays the statistics screen and sets all statistics values
    /// </summary>
    private void StatisticsScreen()
    {
        // Show the UI
        ToggleMultiUI(false, new GameObject[] { victoryScreen, defeatScreen });
        ToggleUI(true, statisticsScreen);
        GameStateHandler.Statistics();

        string tempTime = timer.time.ToString();
        StatisticsTracker.finalTime = tempTime.Remove(tempTime.Length-4, 4);

        // Initialises new tuple list containing round statistics
        List<Tuple<string, string>> statTuples = new List<Tuple<string, string>>{
            new Tuple<string, string>("Total Time", StatisticsTracker.finalTime),
            new Tuple<string, string>("Total Kills", StatisticsTracker.kills.ToString()),
            new Tuple<string, string>("Total Damage Dealt", StatisticsTracker.damageDealt.ToString()),
            new Tuple<string, string>("Total Damage Taken", StatisticsTracker.damageTaken.ToString())
        };

        // Creates the display objects and provides stats to them
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

    /// <summary>
    /// Displays the enter name screen
    /// </summary>
    private void EnterNameScreen()
    {
        ToggleUI(false, statisticsScreen);
        ToggleUI(true, enterNameScreen);
        GameStateHandler.EnterName();
    }

    /// <summary>
    /// Checks the entered name to make sure it is valid
    /// </summary>
    /// <returns>Whether the entered name is valid</returns>
    private bool CheckEnteredName()
    {
        string pattern = @"^[a-zA-Z0-9]+$";
        Regex regex = new Regex(pattern);
        
        string name = enteredName.text.Trim((char)8203).Trim();

        // Make sure the entered name is Alphanumeric only
        if (regex.IsMatch(name) && name.Length >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Hides the invalid name prompt
    /// </summary>
    private void HideInvalidName()
    {
        ToggleUI(false, invalidName);
    }

    /// <summary>
    /// Displays the highscores screen and displays all highscores
    /// </summary>
    private void HighscoresScreen()
    {
        ToggleMultiUI(false, new GameObject[] { statisticsScreen, enterNameScreen });
        ToggleUI(true, highscoresScreen);
        GameStateHandler.Highscores();

        // Creates a new display object for each highscore entry in file
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
