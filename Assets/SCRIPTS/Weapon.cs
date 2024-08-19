// Weapon
// Stores weapon stats and handles the weapon's functionality
// Created by Dima Bethune 05/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FMODUnity;

public class Weapon : MonoBehaviour
{
    public string weaponName;

    [Header("Audio")]
    [SerializeField] public EventReference attackSound;
    [SerializeField] public EventReference reloadStartSound;

    [Header("Weapon Stats")]
    [Tooltip("Damage amount per hit")]
    public float baseDamage;
    [Tooltip("Damage amount multiplier")]
    public float damageMultiplier;
    [HideInInspector] public float damage;
    [Tooltip("Maximum ammo storage")]
    public int baseMaxAmmo;
    [Tooltip("Maxmimum ammo multiplier")]
    public float maxAmmoMultiplier;
    [HideInInspector] public int maxAmmo;
    [Tooltip("Current ammo held")]
    public int ammo;
    [Tooltip("Time (seconds) between attacks")]
    public float baseAttackInterval;
    [Tooltip("Attack speed multiplier")]
    public float attackSpeedMultiplier;
    [HideInInspector] public float attackInterval;
    [Tooltip("Speed of shot projectiles")]
    public float projectileSpeed;
    [Tooltip("Time (seconds) to reload")]
    public float baseReloadTime;
    [Tooltip("Reload speed multiplier")]
    public float reloadSpeedMultiplier;
    [HideInInspector] public float reloadTime;

    [Header("Upgrade Stats")]
    [Tooltip("Damage increase per upgrade (0.01 = 1%)")]
    public float damageUpgradeAmount;
    [Tooltip("Cost (credits) per damage upgrade")]
    public int damageUpgradeCost;
    [Tooltip("Max Ammo increase per upgrade (0.01 = 1%)")]
    public float maxAmmoUpgradeAmount;
    [Tooltip("Cost (credits) per max ammo upgrade")]
    public int maxAmmoUpgradeCost;
    [Tooltip("Attack Speed increase per upgrade (0.01 = 1%)")]
    public float attackSpeedUpgradeAmount;
    [Tooltip("Cost (credits) per attack speed upgrade")]
    public int attackSpeedUpgradeCost;
    [Tooltip("Reload Speed increase per upgrade (0.01 = 1%)")]
    public float reloadSpeedUpgradeAmount;
    [Tooltip("Cost (credits) per reload speed upgrade")]
    public int reloadSpeedUpgradeCost;

    [Header("Bool Variables")]
    [Tooltip("Whether the weapon uses ammmo")]
    public bool usesAmmo;
    [Tooltip("Whether holding down the attack button will repeatedly attack")]
    public bool allowAutoAttack;
    [Tooltip("Whether the weapon is a melee weapon")]
    public bool isMelee;
    [HideInInspector] public bool readyToAttack;
    [HideInInspector] public bool attacking;
    [HideInInspector] public bool reloading;

    private bool allowInvoke; // Bug Fix

    [Header("References")]
    public Transform attackPoint;
    public GameObject projectile;
    public Sprite weaponSprite;
    [HideInInspector] public Camera playerCamera;
    [HideInInspector] public UiHandler uiHandler;

    private void Awake()
    {
        // Set stat values
        damage = baseDamage * damageMultiplier;
        maxAmmo = Convert.ToInt32(baseMaxAmmo * maxAmmoMultiplier);
        attackInterval = baseAttackInterval / attackSpeedMultiplier;
        reloadTime = baseReloadTime / reloadSpeedMultiplier;

        // Set references
        playerCamera = GameObject.Find("PlayerCharacter").transform.Find("MainCamera").GetComponent<Camera>();
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();

        // Make sure magazine is full on startup
        if (usesAmmo)
            ammo = maxAmmo;
        readyToAttack = true;
        allowInvoke = true;
    }

    private void Start()
    {
        if (gameObject.CompareTag("EnemyWeapon"))
        {
            baseDamage *= GameSettingsHandler.difficulty;
        }
    }

    /// <summary>
    /// Uses the weapon's attack
    /// </summary>
    public void Attack()
    {
        if (!isMelee) // Non-melee weapons
        {
            readyToAttack = false; // No longer ready to attack

            // Find the exact hit position using a raycast
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if ray hits something
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(75);

            // Calculate direction from attackPoint to targetPoint
            Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

            // Insantiate bullet/projectile
            GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity, GameObject.Find("PlayerProjectiles").transform);

            // Set weapon reference
            currentBullet.GetComponent<WeaponProjectile>().weapon = this;

            // Rotate bullet to shoot direction
            currentBullet.transform.forward = directionWithoutSpread.normalized;

            // Add forces to bullet
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * projectileSpeed, ForceMode.Impulse);

            // Play shoot sound
            AudioManager.instance.PlayOneShot(attackSound, transform.position);

            ammo--; // Take away one from ammo pool

            // Invoke resetShot function
            if (allowInvoke)
            {
                Invoke("ResetShot", attackInterval * attackSpeedMultiplier);
                allowInvoke = false;
            }
        }
        else // Melee weapons
        {
            // None
        }
    }

    /// <summary>
    /// Resets the gun's shot, allowing it to shoot again
    /// </summary>
    private void ResetShot()
    {
        readyToAttack = true;
        allowInvoke = true;
    }

    /// <summary>
    /// Starts reloading the gun
    /// </summary>
    public void Reload()
    {
        reloading = true;
        AudioManager.instance.PlayOneShot(reloadStartSound, transform.position);
        Invoke("ReloadFinished", reloadTime * reloadSpeedMultiplier);
    }

    /// <summary>
    /// Finishes reloading and refills ammo
    /// </summary>
    private void ReloadFinished()
    {
        ammo = maxAmmo; // Refill ammo
        reloading = false;
    }
}