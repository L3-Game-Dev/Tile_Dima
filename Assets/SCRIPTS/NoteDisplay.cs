using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDisplay : MonoBehaviour
{
    [HideInInspector] public UiHandler uiHandler;
    [HideInInspector] public PlayerInteract interact;

    private void Awake()
    {
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
        interact = GameObject.Find("PlayerCapsule").GetComponent<PlayerInteract>();
    }

    private void Update()
    {
        NoteDisplayUI();
    }

    public void NoteDisplayUI()
    {
        if (interact.lookingAt == gameObject && GameStateHandler.gameState == "PLAYING")
        {
            if (Input.GetKeyDown(interact.keyBind)) // If player presses interact keyBind
            {
                // Open display ui
                uiHandler.ToggleUI(true, uiHandler.noteImage);
                // Hide all code images
                foreach (Transform child in uiHandler.noteImage.transform)
                    uiHandler.ToggleUI(false, child.gameObject);
                // Show relevant codes
                if (gameObject.name.Contains("NoteObject1"))
                    uiHandler.ToggleUI(true, uiHandler.code1Image);
                // Pause the game
                GameStateHandler.Pause();
            }
        }
    }
}
