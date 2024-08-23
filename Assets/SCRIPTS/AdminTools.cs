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
    [HideInInspector] public UiHandler uiHandler;

    [Header("Admin Tools Enabled")]
    [HideInInspector] public bool adminToolsEnabled = true;

    private void Start()
    {
        player = GameObject.Find("PlayerCapsule");
        playerStats = player.GetComponent<PlayerStats>();
        playerController = player.GetComponent<FirstPersonController>();
        inventory = player.GetComponent<PlayerInventory>();
        weapon = inventory.equippedWeapon;
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
    }

    private void Update()
    {
        if (adminToolsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.P))
                StatBuff();
            if (Input.GetKeyDown(KeyCode.O))
                MinibossTeleport();
            if (Input.GetKeyDown(KeyCode.U))
                VictoryScreenInstant();
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

    /// <summary>
    /// Teleports the player to the miniboss
    /// </summary>
    public void MinibossTeleport()
    {
        Vector3 teleportPos = new Vector3(-25, 0, -61);
        player.transform.position = teleportPos;

        Debug.Log("Teleported");
    }

    /// <summary>
    /// Skips straight to the enter name screen
    /// </summary>
    public void VictoryScreenInstant()
    {
        uiHandler.VictoryScreen();

        Debug.Log("Skipped to Victory Screen");
    }
}
