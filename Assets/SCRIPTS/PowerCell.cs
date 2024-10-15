using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCell : MonoBehaviour
{
    public static int remaining = 0;

    [Header("Stats")]
    public float health;

    [Header("References")]
    public GameObject explosionEffect;

    private void Awake()
    {
        remaining++;
    }

    /// <summary>
    /// Modifies the battery's health
    /// </summary>
    /// <param name="op">Usage: '+' || '-'</param>
    /// <param name="amt">The amount to modify by</param>
    public void ModifyHealth(char op, float amt)
    {
        float newHealthAmount = health;

        if (op == '+') // Adding health
        {
            newHealthAmount += amt;
        }
        else if (op == '-') // Removing health
        {
            // Health can't decrease below 0
            if (health - amt > 0)
            {
                newHealthAmount = health - amt;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.dink, transform.position);
            }
            else // 0 health = dead
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.explosion, transform.position);
                remaining--;

                if (remaining == 0)
                {
                    // Do something
                    Debug.Log("All Destroyed");
                    UiHandler.instance.VictoryScreen();
                }

                // Spawn explosion effect
                Instantiate(explosionEffect, transform.position, transform.rotation, GameObject.Find("EnemyProjectiles").transform);

                Destroy(gameObject);
            }
        }

        health = newHealthAmount;
    }
}
