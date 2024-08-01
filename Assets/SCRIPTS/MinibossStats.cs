// MinibossStats
// Stores & handles miniboss stats
// Created by Dima Bethune 26/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject notePrefab;

    private void Awake()
    {
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
    }

    private void Start()
    {
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
                uiHandler.MinibossDeath();
                Instantiate(notePrefab, transform.position, new Quaternion(0, 0, 0, 0), transform);
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
