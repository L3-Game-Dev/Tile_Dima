// PlayerCombat
// Handles player combat functionality
// Created by Dima Bethune 05/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public PlayerInventory playerInventory;
    public UiHandler uiHandler;
    private StarterAssetsInputs input;

    private void Awake()
    {
        // Set references
        playerStats = gameObject.GetComponent<PlayerStats>();
        playerInventory = gameObject.GetComponent<PlayerInventory>();
        input = gameObject.GetComponent<StarterAssetsInputs>();
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();

        foreach (Transform transform in transform.Find("PlayerCameraRoot"))
        {
            if (transform.CompareTag("PlayerWeapon"))
            {
                playerInventory.equippedWeapon = transform.gameObject.GetComponent<Weapon>();
                break;
            }
        }
    }

    private void Update()
    {
        if (!playerStats.isDead)
        {
            if (GameStateHandler.gameState == "PLAYING")
            {
                AttackInput();
                GrenadeInput();
            }
        }
    }

    /// <summary>
    /// Determines what action to perform with equipped weapon and performs it
    /// </summary>
    private void AttackInput()
    {
        // Check if allowed to hold down button and take corresponding input
        if (playerInventory.equippedWeapon.allowAutoAttack) playerInventory.equippedWeapon.attacking = input.attack;
        else playerInventory.equippedWeapon.attacking = input.attack; // Need to adjust this

        // Reloading
        if (input.reload && playerInventory.equippedWeapon.ammo < playerInventory.equippedWeapon.maxAmmo && !playerInventory.equippedWeapon.reloading && playerInventory.equippedWeapon.usesAmmo)
            playerInventory.equippedWeapon.Reload();
        // Reload automatically when trying to shoot without ammo
        if (playerInventory.equippedWeapon.readyToAttack && playerInventory.equippedWeapon.attacking && !playerInventory.equippedWeapon.reloading && playerInventory.equippedWeapon.ammo <= 0)
            playerInventory.equippedWeapon.Reload();

        // Shooting
        if (playerInventory.equippedWeapon.readyToAttack && playerInventory.equippedWeapon.attacking && !playerInventory.equippedWeapon.reloading && playerInventory.equippedWeapon.ammo > 0)
            playerInventory.equippedWeapon.Attack();
    }

    /// <summary>
    /// Throws a grenade if player presses throw button
    /// </summary>
    private void GrenadeInput()
    {
        playerInventory.grenadeThrower.throwing = input.throwingGrenade;

        if (playerInventory.grenadeThrower.readyToThrow && playerInventory.grenadeThrower.throwing && playerInventory.grenadeThrower.grenadesHeld > 0)
            playerInventory.grenadeThrower.Throw();
    }
}
