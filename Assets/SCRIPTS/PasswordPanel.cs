// PasswordPanel
// Handles opening a locked door using a password, through a GUI
// Created by Dima Bethune 26/07

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PasswordPanel : MonoBehaviour
{
    // References
    public GameObject panel1;

    public GameObject door1;

    public List<Tuple<GameObject, string, GameObject>> panelPasswords = new List<Tuple<GameObject, string, GameObject>>();

    [HideInInspector] public string password;

    [HideInInspector] public UiHandler uiHandler;
    [HideInInspector] public PlayerInteract interact;

    public List<TextMeshProUGUI> digits = new List<TextMeshProUGUI>(4);

    [HideInInspector] public TextMeshProUGUI selectedDigit;

    private void Awake()
    {
        // Set references
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
        interact = GameObject.Find("PlayerCapsule").GetComponent<PlayerInteract>();

        // Initialise passwords
        panelPasswords.Add(Tuple.Create(panel1, "2953", door1));
    }
    
    private void Update()
    {
        PasswordPanelUI();
    }

    /// <summary>
    /// Opens the panel UI if player is looking at a panel object and presses the interact keybind
    /// </summary>
    public void PasswordPanelUI()
    {
        foreach (Tuple<GameObject, string, GameObject> panel in panelPasswords)
        {
            if (interact.lookingAt == panel.Item1 && GameStateHandler.gameState == "PLAYING")
            {
                if (Input.GetKeyDown(interact.keyBind)) // If player presses interact keyBind
                {
                    // Open panel
                    uiHandler.ToggleUI(true, uiHandler.passwordPanel);
                    // Initialise the panel
                    InitialisePanelUI();
                    // Set password to current panel's password
                    password = panel.Item2;
                    // Play sound
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.uiClick1, transform.position);
                    // Pause the game
                    GameStateHandler.Pause();
                }
            }
        }
    }

    /// <summary>
    /// Initialises the panel UI
    /// </summary>
    public void InitialisePanelUI()
    {
        ResetDigits();
    }

    /// <summary>
    /// Deletes each entered digit
    /// </summary>
    public void ResetDigits()
    {
        // Reset each digit to default (0, not underlined)
        foreach (TextMeshProUGUI digit in digits)
        {
            digit.text = "0";
            digit.fontStyle = FontStyles.Normal;
        }

        // Set selected digit to first digit
        selectedDigit = digits[0];
        // Underline it
        selectedDigit.fontStyle = FontStyles.Underline;
    }

    /// <summary>
    /// Sets the currently indexed digit
    /// </summary>
    /// <param name="n">The number to set the digit to</param>
    public void SetDigit(int n)
    {
        selectedDigit.text = n.ToString();

        selectedDigit.fontStyle = FontStyles.Normal;

        // Select next digit
        if (digits.IndexOf(selectedDigit)+1 < digits.Count)
            selectedDigit = digits[digits.IndexOf(selectedDigit)+1];

        selectedDigit.fontStyle = FontStyles.Underline;

        // Play beep sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.beep, transform.position);
    }

    /// <summary>
    /// Resets digits
    /// </summary>
    public void DeleteDigit()
    {
        // Play beep sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.beep, transform.position);
        ResetDigits();
    }

    /// <summary>
    /// Submits the currently entered password and checks whether it is correct
    /// </summary>
    public void SubmitPassword()
    {
        string p1 = digits[0].text;
        string p2 = digits[1].text;
        string p3 = digits[2].text;
        string p4 = digits[3].text;
        string code = p1 + p2 + p3 + p4;

        if (code == password)
            CorrectPassword();
        else
            IncorrectPassword();
    }

    /// <summary>
    /// Plays a success sound and opens the door
    /// </summary>
    public void CorrectPassword()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.success, transform.position);
        OpenDoor();
    }

    /// <summary>
    /// Plays an error sound
    /// </summary>
    public void IncorrectPassword()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.error1, transform.position);
    }

    /// <summary>
    /// Opens the door associated with the panel being interacted with
    /// </summary>
    public void OpenDoor()
    {
        // Check which door should be opened based on panel being interacted with
        foreach (Tuple<GameObject, string, GameObject> panel in panelPasswords)
        {
            if (interact.lookingAt == panel.Item1)
            {
                // Disable the door object
                panel.Item3.SetActive(false);
            }
        }
    }
}
