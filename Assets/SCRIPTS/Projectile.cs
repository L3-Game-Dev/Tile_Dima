using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider collision)
    {
        // If it hits a player
        if (collision.CompareTag("Player"))
        {
            // Damage the player
            collision.GetComponent<PlayerStats>().ModifyHealth('-', damage);
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
}
