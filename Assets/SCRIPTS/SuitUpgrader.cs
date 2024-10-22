using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public class SuitUpgrader : MonoBehaviour
{
    [Header("Singleton")]
    public static SuitUpgrader instance;

    [Header("References")]
    public Sprite suitImage;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        UpdateStats();
    }

    private void Update()
    {
        SuitUpgraderPanel();
        UpdateStats();
    }

    /// <summary>
    /// Updates the player suit's stats and displays
    /// </summary>
    public void UpdateStats()
    {
        // Set weapon display references
        UiHandler.instance.upgraderWeaponDisplayImage.sprite = suitImage;
        UiHandler.instance.upgraderWeaponDisplayText.text = "Suit";

        UiHandler.instance.suitStat1.text = "Suit Max Health : " + PlayerStats.instance.maxHealth.ToString("N2");
        UiHandler.instance.suitStat2.text = "Shield Resistance Multiplier : " + PlayerStats.instance.damageResistance.ToString("N2");
        UiHandler.instance.suitStat3.text = "Suit Sprint Energy : " + PlayerStats.instance.maxStamina.ToString("N2");
        UiHandler.instance.suitStat4.text = "Sprint Speed : " + FirstPersonController.instance.SprintSpeed.ToString("N2") + "m/s";

        UiHandler.instance.suitUpgradeButton1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Suit Health: $" + PlayerStats.instance.healthUpgradeCost;
        UiHandler.instance.suitUpgradeButton2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Shield Resistance: $" + PlayerStats.instance.shieldUpgradeCost;
        UiHandler.instance.suitUpgradeButton3.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Suit Energy: $" + PlayerStats.instance.staminaUpgradeCost;
        UiHandler.instance.suitUpgradeButton4.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Upgrade Sprint Speed: $" + PlayerStats.instance.sprintSpeedUpgradeCost;
    }

    /// <summary>
    /// Opens the suit upgrader panel if player is looking at the object and presses interact keybind
    /// </summary>
    public void SuitUpgraderPanel()
    {
        if (PlayerInteract.instance.lookingAt == gameObject && GameStateHandler.gameState == "PLAYING")
        {
            if (Input.GetKeyDown(PlayerInteract.instance.keyBind)) // If player presses interact keyBind
            {
                // Open console panel
                UiHandler.instance.ToggleUI(true, UiHandler.instance.suitUpgraderPanel);
                // Update the display stats
                UpdateStats();
                // Play sound
                AudioManager.instance.PlayOneShot(FMODEvents.instance.uiClick1, transform.position);
                // Pause the game
                GameStateHandler.Pause();
            }
        }
    }

    /// <summary>
    /// Upgrades the suit's max health
    /// </summary>
    public void UpgradeSuitHealth()
    {
        if (PlayerInventory.instance.heldCredits - PlayerStats.instance.healthUpgradeCost >= 0)
        {
            PlayerInventory.instance.heldCredits -= PlayerStats.instance.healthUpgradeCost ;
            PlayerStats.instance.maxHealth += PlayerStats.instance.healthUpgradeAmount;
            PlayerStats.instance.healthUpgradeCost *= 2;

            // Add additional health if less than max
            if (PlayerStats.instance.health + PlayerStats.instance.healthUpgradeAmount > PlayerStats.instance.maxHealth)
            {
                PlayerStats.instance.health = PlayerStats.instance.maxHealth;
            }
            else
            {
                PlayerStats.instance.health += PlayerStats.instance.healthUpgradeAmount;
            }

            UpgradeSound1();
            UpdateStats();

            // Update the stat bar UIs
            UiHandler.instance.healthBar1.fillAmount = PlayerStats.instance.health / PlayerStats.instance.maxHealth;
            UiHandler.instance.healthBarNumber.text = PlayerStats.instance.health.ToString();
        }
        else
        {
            ErrorSound1();
        }
    }

    /// <summary>
    /// Upgrades the suit's damage resistance
    /// </summary>
    public void UpgradeSuitShield()
    {
        if (PlayerInventory.instance.heldCredits - PlayerStats.instance.shieldUpgradeCost >= 0)
        {
            PlayerInventory.instance.heldCredits -= PlayerStats.instance.shieldUpgradeCost;
            PlayerStats.instance.damageResistance += PlayerStats.instance.shieldUpgradeAmount;
            PlayerStats.instance.shieldUpgradeCost *= 2;
            UpgradeSound1();
            UpdateStats();
        }
        else
        {
            ErrorSound1();
        }
    }

    /// <summary>
    /// Upgrades the suit's max stamina
    /// </summary>
    public void UpgradeSuitEnergy()
    {
        if (PlayerInventory.instance.heldCredits - PlayerStats.instance.staminaUpgradeCost >= 0)
        {
            PlayerInventory.instance.heldCredits -= PlayerStats.instance.staminaUpgradeCost;
            PlayerStats.instance.maxStamina += PlayerStats.instance.staminaUpgradeAmount;
            PlayerStats.instance.staminaUpgradeCost *= 2;
            UpgradeSound1();
            UpdateStats();
        }
        else
        {
            ErrorSound1();
        }
    }

    /// <summary>
    /// Upgrades the suit's sprint speed
    /// </summary>
    public void UpgradeSuitSprintSpeed()
    {
        if (PlayerInventory.instance.heldCredits - PlayerStats.instance.sprintSpeedUpgradeCost >= 0)
        {
            PlayerInventory.instance.heldCredits -= PlayerStats.instance.sprintSpeedUpgradeCost;
            FirstPersonController.instance.SprintSpeed += PlayerStats.instance.sprintSpeedUpgradeAmount;
            PlayerStats.instance.sprintSpeedUpgradeCost *= 2;
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
