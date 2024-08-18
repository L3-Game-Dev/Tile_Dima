// EnemyStats
// Stores & handles enemy stats
// Created by Dima Bethune 05/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stat Values")]
    public float baseMaxHealth;
    public float maxHealth;
    public float health;

    public int creditValue;

    [HideInInspector] public bool isDead;

    private void Start()
    {
        // Scale stats based on difficulty
        baseMaxHealth *= GameSettingsHandler.difficulty;

        // Initialise values
        maxHealth = baseMaxHealth;
        health = maxHealth;
    }

    public void ModifyHealth(char op, float amt)
    {
        if (op == '+') // Adding health
        {
            // Health can't increase beyond max
            if (health + amt <= maxHealth)
            {
                health += amt;
            }
            else
            {
                health = maxHealth;
            }
        }
        else if (op == '-') // Removing health
        {
            // Health can't decrease below 0
            if (health - amt > 0)
            {
                health -= amt;
                StatisticsTracker.damageDealt += amt;
            }
            else // 0 health = dead
            {
                StatisticsTracker.damageDealt += health;
                StatisticsTracker.kills += 1;
                GameObject.Find("PlayerCapsule").GetComponent<PlayerInventory>().heldCredits += creditValue;
                health = 0;
                isDead = true;
            }

            // Play hurt sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.zombieHurt, transform.position);
        }
    }
}
