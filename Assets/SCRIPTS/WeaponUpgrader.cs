// WeaponUpgrader
// Handles all functionality for the weapon upgrader
// Created by Dima Bethune 25/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponUpgrader : MonoBehaviour
{
    [HideInInspector] public UiHandler uiHandler;
    [HideInInspector] public PlayerInteract interact;
    [HideInInspector] public PlayerInventory inventory;
    
    private void Awake()
    {
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
        interact = GameObject.Find("PlayerCapsule").GetComponent<PlayerInteract>();
        inventory = GameObject.Find("PlayerCapsule").GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        UpdateStats();
    }

    private void Update()
    {
        WeaponUpgraderPanel();
        UpdateStats(); // More efficient way to do this?
    }

    public Weapon FindEquippedWeapon()
    {
        return inventory.equippedWeapon;
    }

    public void UpdateStats()
    {
        // Set weapon display references
        uiHandler.upgraderWeaponDisplayImage.sprite = FindEquippedWeapon().weaponSprite;
        uiHandler.upgraderWeaponDisplayText.text = FindEquippedWeapon().weaponName;

        FindEquippedWeapon().damage = FindEquippedWeapon().baseDamage * FindEquippedWeapon().damageMultiplier;
        FindEquippedWeapon().maxAmmo = Convert.ToInt32(FindEquippedWeapon().baseMaxAmmo * FindEquippedWeapon().maxAmmoMultiplier);
        FindEquippedWeapon().attackInterval = FindEquippedWeapon().baseAttackInterval / FindEquippedWeapon().attackSpeedMultiplier;
        FindEquippedWeapon().reloadTime = FindEquippedWeapon().baseReloadTime / FindEquippedWeapon().reloadSpeedMultiplier;

        uiHandler.weaponStat1.text = "Damage Multiplier : " + inventory.equippedWeapon.damageMultiplier.ToString("N2");
        uiHandler.weaponStat2.text = "Max Ammo Multiplier : " + inventory.equippedWeapon.maxAmmoMultiplier.ToString("N2");
        uiHandler.weaponStat3.text = "Attack Speed Multiplier : " + inventory.equippedWeapon.attackSpeedMultiplier.ToString("N2");
        uiHandler.weaponStat4.text = "Reload Speed Multiplier : " + inventory.equippedWeapon.reloadSpeedMultiplier.ToString("N2");
    }

    public void WeaponUpgraderPanel()
    {
        if (interact.lookingAt == gameObject && GameStateHandler.gameState == "PLAYING")
        {
            if (Input.GetKeyDown(interact.keyBind)) // If player presses interact keyBind
            {
                // Open console panel
                uiHandler.ToggleUI(true, uiHandler.weaponUpgraderPanel);
                // Set weapon display references
                uiHandler.upgraderWeaponDisplayImage.sprite = FindEquippedWeapon().weaponSprite;
                uiHandler.upgraderWeaponDisplayText.text = FindEquippedWeapon().weaponName;
                // Pause the game
                GameStateHandler.Pause();
            }
        }
    }

    public void UpgradeDamage()
    {
        FindEquippedWeapon().damageMultiplier += FindEquippedWeapon().damageUpgradeAmount;
        UpdateStats();
    }

    public void UpgradeMaxAmmo()
    {
        FindEquippedWeapon().maxAmmoMultiplier += FindEquippedWeapon().maxAmmoUpgradeAmount;
        UpdateStats();
    }

    public void UpgradeAttackSpeed()
    {
        FindEquippedWeapon().attackSpeedMultiplier += FindEquippedWeapon().attackSpeedUpgradeAmount;
        UpdateStats();
    }

    public void UpgradeReloadSpeed()
    {
        FindEquippedWeapon().reloadSpeedMultiplier += FindEquippedWeapon().reloadSpeedUpgradeAmount;
        UpdateStats();
    }
}
