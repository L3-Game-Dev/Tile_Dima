// WeaponUpgrader
// Handles all functionality for the weapon upgrader
// Created by Dima Bethune 25/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WeaponUpgrader : MonoBehaviour
{
    [HideInInspector] public UiHandler uiHandler;
    [HideInInspector] public PlayerInteract interact;
    [HideInInspector] public PlayerInventory inventory;
    
    private void Awake()
    {
        // Set references
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
        UpdateStats();
    }

    /// <summary>
    /// Finds and returns the currently equipped weapon
    /// </summary>
    /// <returns>The equipped weapon</returns>
    public Weapon FindEquippedWeapon()
    {
        return inventory.equippedWeapon;
    }

    /// <summary>
    /// Updates the equipped weapon's stats and displays
    /// </summary>
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

        uiHandler.upgradeButton1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Damage: $" + FindEquippedWeapon().damageUpgradeCost;
        uiHandler.upgradeButton2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Max Ammo: $" + FindEquippedWeapon().maxAmmoUpgradeCost;
        uiHandler.upgradeButton3.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Attack Speed: $" + FindEquippedWeapon().attackSpeedUpgradeCost;
        uiHandler.upgradeButton4.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Reload Speed: $" + FindEquippedWeapon().reloadSpeedUpgradeCost;
    }

    /// <summary>
    /// Opens the weapon upgrader panel if player is looking at the object and presses interact keybind
    /// </summary>
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
                // Play sound
                AudioManager.instance.PlayOneShot(FMODEvents.instance.uiClick1, transform.position);
                // Pause the game
                GameStateHandler.Pause();
            }
        }
    }

    /// <summary>
    /// Upgrades the equipped weapon's damage amount
    /// </summary>
    public void UpgradeDamage()
    {
        if (inventory.heldCredits - FindEquippedWeapon().damageUpgradeCost >= 0)
        {
            inventory.heldCredits -= FindEquippedWeapon().damageUpgradeCost;
            FindEquippedWeapon().damageMultiplier += FindEquippedWeapon().damageUpgradeAmount;
            FindEquippedWeapon().damageUpgradeCost *= 2;
            UpgradeSound1();
            UpdateStats();
        }
        else
        {
            ErrorSound1();
        }
    }

    /// <summary>
    /// Upgrades the equipped weapon's max ammo amount
    /// </summary>
    public void UpgradeMaxAmmo()
    {
        if (inventory.heldCredits - FindEquippedWeapon().maxAmmoUpgradeCost >= 0)
        {
            inventory.heldCredits -= FindEquippedWeapon().maxAmmoUpgradeCost;
            FindEquippedWeapon().maxAmmoMultiplier += FindEquippedWeapon().maxAmmoUpgradeAmount;
            FindEquippedWeapon().maxAmmoUpgradeCost *= 2;
            UpgradeSound1();
            UpdateStats();
        }
        else
        {
            ErrorSound1();
        }
    }

    /// <summary>
    /// Upgrades the equipped weapon's attack speed amount
    /// </summary>
    public void UpgradeAttackSpeed()
    {
        if (inventory.heldCredits - FindEquippedWeapon().attackSpeedUpgradeCost >= 0)
        {
            inventory.heldCredits -= FindEquippedWeapon().attackSpeedUpgradeCost;
            FindEquippedWeapon().attackSpeedMultiplier += FindEquippedWeapon().attackSpeedUpgradeAmount;
            FindEquippedWeapon().attackSpeedUpgradeCost *= 2;
            UpgradeSound1();
            UpdateStats();
        }
        else
        {
            ErrorSound1();
        }
    }

    /// <summary>
    /// Upgrades the equipped weapon's reload speed amount
    /// </summary>
    public void UpgradeReloadSpeed()
    {
        if (inventory.heldCredits - FindEquippedWeapon().reloadSpeedUpgradeCost >= 0)
        {
            inventory.heldCredits -= FindEquippedWeapon().reloadSpeedUpgradeCost;
            FindEquippedWeapon().reloadSpeedMultiplier += FindEquippedWeapon().reloadSpeedUpgradeAmount;
            FindEquippedWeapon().reloadSpeedUpgradeCost *= 2;
            UpgradeSound1();
            UpdateStats();
        }
        else
        {
            ErrorSound1();
        }
    }

    /* ----------------------------------- START SOUNDS ----------------------------------- */
    public void UpgradeSound1()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.upgrade1, transform.position);
    }

    public void ErrorSound1()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.error1, transform.position);
    }
    /* ------------------------------------ END SOUNDS ------------------------------------ */
}
