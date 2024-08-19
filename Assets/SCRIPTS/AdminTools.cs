// AdminTools
// Provides big stat buffs for testing/debugging
// Created by Dima Bethune 12/08

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class AdminTools : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public GameObject player;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public FirstPersonController playerController;
    [HideInInspector] public PlayerInventory inventory;
    [HideInInspector] public Weapon weapon;

    [Header("Admin Tools Enabled")]
    [HideInInspector] public bool adminToolsEnabled = true;

    private void Start()
    {
        player = GameObject.Find("PlayerCapsule");
        playerStats = player.GetComponent<PlayerStats>();
        playerController = player.GetComponent<FirstPersonController>();
        inventory = player.GetComponent<PlayerInventory>();
        weapon = inventory.equippedWeapon;
    }

    private void Update()
    {
        if (adminToolsEnabled && Input.GetKeyDown(KeyCode.P))
        {
            StatBuff();
        }
    }

    /// <summary>
    /// Increases player stats
    /// </summary>
    public void StatBuff()
    {
        playerController.MoveSpeed = 30;
        playerStats.maxHealth = 999999;
        playerStats.health = 999999;
        weapon.baseDamage = 999999;

        Debug.Log("Stat Buff Applied");
    }
}
