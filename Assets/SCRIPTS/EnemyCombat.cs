// EnemyCombat
// Handles enemy combat functionality
// Created by Dima Bethune 05/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using FMODUnity;

public class EnemyCombat : MonoBehaviour
{
    [Header("References")]
    [HideInInspector]  public EnemyStats enemyStats;
    public Weapon equippedWeapon;
    [HideInInspector] public EnemyController enemyController;

    private void Awake()
    {
        // Set stats reference
        enemyStats = gameObject.GetComponent<EnemyStats>();
        enemyController = gameObject.GetComponent<EnemyController>();

        // Set weapon reference
        foreach (Transform transform in transform.Find("root"))
        {
            if (transform.CompareTag("EnemyWeapon"))
            {
                equippedWeapon = transform.gameObject.GetComponent<Weapon>();
                break;
            }
        }
        if (equippedWeapon == null)
        {
            Debug.Log("No EnemyWeapon found");
        }
    }

    public void Attack()
    {
        float distanceToTarget = Vector3.Distance(transform.position, enemyController.target.position);

        if (distanceToTarget <= enemyController.agent.stoppingDistance)
        {
            enemyController.target.GetComponent<PlayerStats>().ModifyHealth('-', equippedWeapon.damage);
        }

        AudioManager.instance.PlayOneShot(FMODEvents.instance.zombieAttack, transform.position);
    }
}
