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

    private void OnTriggerEnter(Collider collision)
    {
        if (gameObject.CompareTag("PlayerProjectile"))
        {
            if (collision.CompareTag("Enemy"))
            {
                if (collision.GetComponent<EnemyStats>())
                {
                    collision.GetComponent<EnemyStats>().ModifyHealth('-', damage);
                }
                else if (collision.GetComponent<MinibossStats>())
                {
                    collision.GetComponent<MinibossStats>().ModifyHealth('-', damage);
                }

                SpawnHitmarker(collision.gameObject);
            }
        }
        else if (gameObject.CompareTag("EnemyProjectile"))
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerStats>().ModifyHealth('-', damage);
            }
        }
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

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