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
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;
    public GameObject panel4;

    public List<Tuple<GameObject, string>> panelPasswords = new List<Tuple<GameObject, string>>();

    [HideInInspector] public string password;

    [HideInInspector] public UiHandler uiHandler;
    [HideInInspector] public PlayerInteract interact;

    public static List<TextMeshProUGUI> digits = new List<TextMeshProUGUI>(4);

    public static TextMeshProUGUI selectedDigit;

    private void Awake()
    {
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
        interact = GameObject.Find("PlayerCapsule").GetComponent<PlayerInteract>();

        // Initialise passwords
        panelPasswords.Add(Tuple.Create(panel1, "2953"));
        panelPasswords.Add(Tuple.Create(panel2, "0000"));
        panelPasswords.Add(Tuple.Create(panel3, "0000"));
        panelPasswords.Add(Tuple.Create(panel4, "0000"));
    }
    
    private void Update()
    {
        PasswordPanelUI();
    }

    public void PasswordPanelUI()
    {
        foreach (Tuple<GameObject, string> panel in panelPasswords)
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
                    // Pause the game
                    GameStateHandler.Pause();
                }
            }
        }
    }

    public void InitialisePanelUI()
    {
        ResetDigits();
    }

    public void ResetDigits()
    {
        foreach (TextMeshProUGUI digit in digits)
        {
            digit.text = "0";
        }

        selectedDigit = digits[0];

        selectedDigit.fontStyle = FontStyles.Underline;
    }

    public void SetDigit(int n)
    {
        selectedDigit.text = n.ToString();
    }

    public void DeleteDigit()
    {
        //selectedDigit.text = "0";

        ResetDigits(); // Try replace this with individual digit deletion
    }

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

    public void CorrectPassword()
    {
        Debug.Log("CORRECT PASSWORD ENTERED");
    }

    public void IncorrectPassword()
    {
        Debug.Log("INCORRECT PASSWORD ENTERED");
    }
}
