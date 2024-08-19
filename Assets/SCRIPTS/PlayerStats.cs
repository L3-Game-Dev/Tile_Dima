// PlayerStats
// Stores & handles player stats
// Created by Dima Bethune 05/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stat Values")]
    public float baseMaxHealth;
    public float maxHealth;
    public float health;

    public float baseMaxStamina;
    public float maxStamina;
    public float stamina;

    public float staminaDrainSpeed;
    public float staminaRegainSpeed;

    [HideInInspector] public bool isDead;

    [Header("References")]
    [HideInInspector] public UiHandler uiHandler;
    [HideInInspector] public FirstPersonController controller;
    [HideInInspector] public StarterAssetsInputs input;

    private void Awake()
    {
        // Set references
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
        controller = gameObject.GetComponent<FirstPersonController>();
        input = gameObject.GetComponent<StarterAssetsInputs>();
    }

    private void Start()
    {
        // Initialise values
        maxHealth = baseMaxHealth;
        health = maxHealth;

        uiHandler.healthBarSlider.maxValue = maxHealth;
        uiHandler.healthBarSlider.minValue = 0;

        uiHandler.staminaBarSlider.maxValue = maxStamina;
        uiHandler.staminaBarSlider.minValue = 0;

        uiHandler.healthBarNumber.text = health.ToString();
        uiHandler.staminaBarNumber.text = stamina.ToString();
    }

    private void Update()
    {
        UpdateStamina();
    }

    /// <summary>
    /// Modifies the players health
    /// </summary>
    /// <param name="op">Usage: '+' || '-'</param>
    /// <param name="amt">The amount to modify by</param>
    public void ModifyHealth(char op, float amt)
    {
        if (GameStateHandler.gameState == "PLAYING")
        {
            float newHealthAmount = health;

            if (op == '+') // Adding health
            {
                // Health can't increase beyond max
                if (health + amt <= maxHealth)
                {
                    newHealthAmount = health + amt;
                }
                else
                {
                    newHealthAmount = maxHealth;
                }
            }
            else if (op == '-') // Removing health
            {
                // Health can't decrease below 0
                if (health - amt > 0)
                {
                    newHealthAmount = health - amt;
                    StatisticsTracker.damageTaken += amt;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.playerHit, transform.position);
                }
                else // 0 health = dead
                {
                    StatisticsTracker.damageTaken += health;
                    newHealthAmount = 0;
                    isDead = true;
                    // Death Functionality
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.playerDeath, transform.position);
                    uiHandler.PlayerDeath();
                    GameStateHandler.Defeat();
                }
            }

            health = newHealthAmount;
            uiHandler.healthBarSlider.value = newHealthAmount;
            uiHandler.healthBarNumber.text = newHealthAmount.ToString();
        }
    }

    /// <summary>
    /// Updates the players stamina
    /// </summary>
    public void UpdateStamina()
    {
        if (controller.sprinting && (input.move.x != 0 || input.move.y != 0))
        {
            CancelInvoke("RegainStamina");
            if (stamina - 1 * staminaDrainSpeed * Time.deltaTime > 0)
            {
                stamina -= 1 * staminaDrainSpeed * Time.deltaTime;
            }
            else
            {
                stamina = 0;
            }
        }
        else
        {
            Invoke("RegainStamina", 5);
        }
        uiHandler.staminaBarSlider.value = stamina;
        uiHandler.staminaBarNumber.text = ((int)stamina).ToString();
    }

    /// <summary>
    /// Regains the players stamina
    /// </summary>
    public void RegainStamina()
    {
        if (stamina + 1 * staminaRegainSpeed * Time.deltaTime < maxStamina)
        {
            stamina += 1 * staminaRegainSpeed * Time.deltaTime;
        }
        else
        {
            stamina = maxStamina;
        }
    }
}
