// WeaponProjectile
// Handles spawning & functionality of weapon projectiles
// Created by Dima Bethune 05/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponProjectile : MonoBehaviour
{
    [Header("Variables")]
    [HideInInspector] public float damage;
    public Weapon weapon;
    [HideInInspector] public PlayerCombat playerCombat;
    public float decayTime;

    [Header("References")]
    public GameObject hitmarkerObj;

    private void Awake()
    {
        // Set references
        playerCombat = GameObject.Find("PlayerCharacter").transform.Find("PlayerCapsule").GetComponent<PlayerCombat>();
    }

    private void Start()
    {
        // Set variable values
        damage = weapon.GetComponent<Weapon>().damage * weapon.GetComponent<Weapon>().damageMultiplier;

        // Destroy after decayTime
        Invoke("DestroyProjectile", decayTime);
    }

    /// <summary>
    /// Determines what object the projectile collided with and makes action accordingly
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        // Player projectiles
        if (gameObject.CompareTag("PlayerProjectile"))
        {
            // If it hits an enemy
            if (collision.CompareTag("Enemy"))
            {
                // If it hits a normal enemy
                if (collision.GetComponent<EnemyStats>())
                {
                    // Damage the enemy
                    collision.GetComponent<EnemyStats>().ModifyHealth('-', damage);
                }
                // If it hits a miniboss
                else if (collision.GetComponent<MinibossStats>())
                {
                    // Damage the minibosss
                    collision.GetComponent<MinibossStats>().ModifyHealth('-', damage);
                }

                SpawnHitmarker(collision.gameObject);
            }
        }
        // Enemy projectiles
        else if (gameObject.CompareTag("EnemyProjectile"))
        {
            // If it hits a player
            if (collision.CompareTag("Player"))
            {
                // Damage the player
                collision.GetComponent<PlayerStats>().ModifyHealth('-', damage);
            }
        }
        DestroyProjectile();
    }

    /// <summary>
    /// Destroy the projectile object
    /// </summary>
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Spawns a hitmarker number on the hit object
    /// </summary>
    /// <param name="hit"></param>
    private void SpawnHitmarker(GameObject hit)
    {
        // Add some random variation to location
        Vector3 loc = hit.transform.position;
        loc.x += UnityEngine.Random.value * 0.5f;
        loc.y += UnityEngine.Random.value * 0.5f + 1f;
        loc.z += UnityEngine.Random.value * 0.5f;
        GameObject hitmarker = Instantiate(hitmarkerObj, loc, new Quaternion(0, 0, 0, 0), hit.transform);

        hitmarker.GetComponent<HitmarkerHandler>().SetNumber(damage);
    }
}