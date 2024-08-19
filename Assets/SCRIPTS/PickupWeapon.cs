// PickupWeapon
// Handles functionality for pickup up dropped weapons
// Created by Dima Bethune 13/08

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [HideInInspector] public PlayerInteract interact;
    [HideInInspector] public PlayerInventory inventory;

    private void Awake()
    {
        // Set references
        interact = GameObject.Find("PlayerCapsule").GetComponent<PlayerInteract>();
        inventory = GameObject.Find("PlayerCapsule").GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        PickupWeaponCheck();
    }

    /// <summary>
    /// Picks up a dropped weapon if player is looking at it and presses the interact keybind
    /// </summary>
    public void PickupWeaponCheck()
    {
        foreach (Transform weaponObject in transform)
        {
            if (interact.lookingAt == weaponObject.gameObject && GameStateHandler.gameState == "PLAYING")
            {
                if (Input.GetKeyDown(interact.keyBind)) // If player presses interact keyBind
                {
                    // Pickup the weapon
                    inventory.PickupWeapon(weaponObject.gameObject);
                }
            }
        }
    }
}
