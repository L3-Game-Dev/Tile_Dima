// DoorControl
// Handles opening tile doors through console panel UI
// Created by Dima Bethune 29/05

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class DoorControl : MonoBehaviour
{
    // References
    [HideInInspector] public UiHandler uiHandler;
    [HideInInspector] public PlayerInteract interact;

    public bool doorControlEnabled;

    private void Awake()
    {
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
        interact = GameObject.Find("PlayerCapsule").GetComponent<PlayerInteract>();
    }

    private void Update()
    {
        DoorControlPanel();
    }

    /// <summary>
    /// Opens the door control panel UI if the player is looking at console and presses the interact key
    /// </summary>
    public void DoorControlPanel()
    {
        if (interact.lookingAt == gameObject && GameStateHandler.gameState == "PLAYING")
        {
            if (Input.GetKeyDown(interact.keyBind)) // If player presses interact keyBind
            {
                // Open console panel
                uiHandler.ToggleUI(true, uiHandler.doorConsolePanel);
                // Play sound
                AudioManager.instance.PlayOneShot(FMODEvents.instance.uiClick1, transform.position);
                // Pause the game
                GameStateHandler.Pause();
            }
        }
    }

    /// <summary>
    /// Deletes all objects from a specified door
    /// </summary>
    /// <param name="door">The door to open/delete</param>
    public void OpenDoor(GameObject door)
    {
        if (doorControlEnabled)
        {
            // Set 'DoorParts' reference
            Transform doorParts = door.transform.Find("DoorParts");

            // Disable every doorPart from the door
            foreach (Transform doorPart in doorParts)
            {
                doorPart.gameObject.SetActive(false);
            }

            // Play sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.success, transform.position);
        }
    }

    /// <summary>
    /// Disables a specified button
    /// </summary>
    /// <param name="button">The button to disable</param>
    public void DisableButton(Button button)
    {
        button.interactable = false;
    }
}
