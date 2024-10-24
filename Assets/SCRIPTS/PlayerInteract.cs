// PlayerInteract
// Handles allowing player to interact with interactables
// Created by Dima Bethune 24/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Singleton")]
    public static PlayerInteract instance;

    [HideInInspector] public GameObject playerCamera;
    [HideInInspector] public UiHandler uiHandler;

    public float maxDistance;
    public LayerMask mask;
    public GameObject lookingAt;
    public KeyCode keyBind;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;

        // Set references
        playerCamera = transform.parent.Find("MainCamera").gameObject;
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
    }

    private void Update()
    {
        lookingAt = CheckPlayerDistance();
        InteractPrompt();
    }

    /// <summary>
    /// Checks whether player is looking at an interactable object within interaction range
    /// </summary>
    /// <returns>The gameobject being looked at</returns>
    public GameObject CheckPlayerDistance()
    {
        // Send out a ray to check if player is in range & looking at interactable
        Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out var hit, maxDistance, mask);

#if UNITY_EDITOR
        // Visualise the ray within editor
        Vector3 forward = transform.TransformDirection(Vector3.forward) * maxDistance;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * maxDistance, Color.green);
#endif

        if (hit.collider != null) // If raycast hits an interactable
        {
            return hit.transform.gameObject;
        }
        else // If raycast does not an interactable
        {
            return null;
        }
    }

    /// <summary>
    /// Shows/hides the interact prompt depending on whether an interactable object within range is being looked at
    /// </summary>
    public void InteractPrompt()
    {
        if (lookingAt != null && GameStateHandler.gameState == "PLAYING") // Show interact prompt
        {
            uiHandler.ToggleUI(true, uiHandler.interactPrompt);
        }
        else // Hide interact prompt
        {
            uiHandler.ToggleUI(false, uiHandler.interactPrompt);
        }
    }
}
