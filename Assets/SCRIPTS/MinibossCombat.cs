// MinibossCombat
// Handles miniboss combat functionality
// Created by Dima Bethune 26/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class MinibossCombat : MonoBehaviour
{
    [Header("References")]
    [HideInInspector]  public MinibossStats minibossStats;
    public Weapon equippedWeapon;
    [HideInInspector] public MinibossController minibossController;

    private void Awake()
    {
        // Set stats reference
        minibossStats = gameObject.GetComponent<MinibossStats>();
        minibossController = gameObject.GetComponent<MinibossController>();

        // Set weapon reference
        foreach (Transform transform in transform)
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

    /// <summary>
    /// Shoots a projectile in the direction of the player
    /// </summary>
    public void Shoot()
    {
        if (GameStateHandler.gameState == "PLAYING")
        {
            if (!minibossController.anim.GetBool("isReloading"))
            {
                Vector3 targetPoint = minibossController.target.position;
                targetPoint.y += 1; // Add height to shoot at player's torso rather than feet

                // Calculate direction from attackPoint to targetPoint
                Vector3 directionWithoutSpread = targetPoint - equippedWeapon.attackPoint.position;

                // Insantiate bullet/projectile
                GameObject currentBullet = Instantiate(equippedWeapon.projectile, equippedWeapon.attackPoint.position, Quaternion.identity, GameObject.Find("EnemyProjectiles").transform);

                // Set weapon reference
                currentBullet.GetComponent<WeaponProjectile>().weapon = equippedWeapon.GetComponent<Weapon>();

                // Rotate bullet to shoot direction
                currentBullet.transform.forward = directionWithoutSpread.normalized;

                // Add forces to bullet
                currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * equippedWeapon.projectileSpeed, ForceMode.Impulse);

                // Play shoot sound
                AudioManager.instance.PlayOneShot(equippedWeapon.attackSound, transform.position);
            }
        }
    }

    /// <summary>
    /// Kicks in front of the miniboss
    /// </summary>
    public void Kick()
    {
        if (GameStateHandler.gameState == "PLAYING")
        {
            float distanceToTarget = Vector3.Distance(transform.position, minibossController.target.position);

            if (distanceToTarget <= minibossStats.meleeRange)
            {
                minibossController.target.GetComponent<PlayerStats>().ModifyHealth('-', equippedWeapon.damage);

                // Play kick sound
                AudioManager.instance.PlayOneShot(FMODEvents.instance.kick, transform.position);
            }
        }
    }

    /// <summary>
    /// Starts the miniboss' reload
    /// </summary>
    public void StartReload()
    {
        // Set reloading bool to true
        minibossController.anim.SetBool("isReloading", true);
    }

    /// <summary>
    /// Finishes the miniboss' reload
    /// </summary>
    public void FinishReload()
    {
        // Revert reloading bool to false
        minibossController.anim.SetBool("isReloading", false);
    }
}
