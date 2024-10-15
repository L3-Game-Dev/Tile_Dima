using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zap : MonoBehaviour
{
    public float damage;
    public GameObject zapEffect;

    private void Start()
    {
        Damage();

        // Spawn grenade effect
        Instantiate(zapEffect, transform.position, transform.rotation, GameObject.Find("EnemyProjectiles").transform);
    }

    public void Damage()
    {
        // Reference player
        GameObject player = GameObject.Find("PlayerCapsule");

        // If player is within zap radius
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance <= transform.localScale.x / 2)
        {
            player.GetComponent<PlayerStats>().ModifyHealth('-', damage);
        }

        // Get all enemies
        GameObject[] enemiesInRange = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemiesInRange)
        {
            // If enemy is within grenade explosion radius
            distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance <= transform.localScale.x / 2)
            {
                // If enemy
                if (enemy.GetComponent<EnemyStats>())
                    enemy.GetComponent<EnemyStats>().ModifyHealth('-', damage); // Damage the enemy
            }
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
