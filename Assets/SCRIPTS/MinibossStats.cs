// MinibossStats
// Stores & handles miniboss stats
// Created by Dima Bethune 26/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinibossStats : MonoBehaviour
{
    [Header("Enemy Stat Values")]
    public float baseMaxHealth;
    public float maxHealth;
    public float health;

    public int creditValue;

    [Header("Movement Stats")]
    public float shootRange;
    public float meleeRange;

    [HideInInspector] public bool isDead;
    [HideInInspector] public UiHandler uiHandler;

    public Button nextDoor;

    private void Awake()
    {
        // Set reference
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
    }

    private void Start()
    {
        // Scale stats based on difficulty
        baseMaxHealth *= GameSettingsHandler.difficulty;

        // Initialise values
        maxHealth = baseMaxHealth;
        health = maxHealth;
    }

    /// <summary>
    /// Modifies the miniboss' health
    /// </summary>
    /// <param name="op">Usage: '+' || '-'</param>
    /// <param name="amt">The amount to increase/decrease by</param>
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
                PostProcessing.instance.BeginColorTransition(Color.red, Color.white);
                EnemySpawner.instance.spawningEnabled = false;
                AudioManager.instance.SwitchMusicTrack(MusicTrack.DEFAULT);
                // Delete minimap icon
                Destroy(GameObject.Find("MinimapElements").transform.Find("MinimapIcons").Find("BossIcon(Clone)").gameObject);
                nextDoor.interactable = true;
                UiHandler.instance.ShowNotification("New Area Unlocked", 3f);
                UiHandler.instance.NewObjective("Proceed to laboratory");
                uiHandler.MinibossDeath();
            }
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Color shootRangeColor = Color.red;
        Gizmos.color = shootRangeColor;
        Gizmos.DrawWireSphere(transform.position, shootRange);

        Color meleeRangeColor = Color.magenta;
        Gizmos.color = meleeRangeColor;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

#endif

}
