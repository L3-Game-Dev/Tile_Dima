using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public static int turretCount = 0;

    public Transform attackPoint1;
    public Transform attackPoint2;

    public Transform sightPoint;

    public float maxHealth;
    public float health;

    public float speed;

    public bool readyToShoot;
    public int currentGun;
    public float shootInterval;

    public GameObject projectile;

    public int creditValue;

    private void Start()
    {
        readyToShoot = true;
        currentGun = 1;
        turretCount++;

        sightPoint = transform.Find("SightPos");
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            transform.LookAt(PlayerStats.instance.transform.position);
            if (readyToShoot)
            {
                Shoot(PlayerStats.instance.transform.position);
            }
        }
    }

    /// <summary>
    /// Checks whether the turret is able to see the player
    /// </summary>
    /// <returns></returns>
    private bool CanSeePlayer()
    {
        Vector3 directionToTarget = (PlayerStats.instance.transform.position - transform.position).normalized;

        Ray ray = new Ray(sightPoint.position, directionToTarget);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Shoots at the provided location
    /// </summary>
    /// <param name="pos">Where to shoot</param>
    private void Shoot(Vector3 pos)
    {
        Vector3 shootPos;
        if (currentGun == 1)
        {
            shootPos = attackPoint1.position;
            currentGun = 2;
        }
        else
        {
            shootPos = attackPoint2.position;
            currentGun = 1;
        }

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = PlayerStats.instance.transform.position - shootPos;

        // Insantiate bullet/projectile
        GameObject currentBullet = Instantiate(projectile, shootPos, new Quaternion(0, 0, 0, 0));

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * speed, ForceMode.Impulse);

        // Play shoot sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.turretShoot, transform.position);

        readyToShoot = false;
        Invoke("Reload", shootInterval);
    }

    /// <summary>
    /// Modifies the turret's health
    /// </summary>
    /// <param name="op">Usage: '+' || '-'</param>
    /// <param name="amt">The amount to increase/decrease by</param>
    public void ModifyHealth(char op, float amt)
    {
        if (op == '+') // Adding health
        {
            // Health can't increase beyond max
            if (health + amt <= maxHealth)
            {
                health += amt;
            }
            else
            {
                health = maxHealth;
            }
        }
        else if (op == '-') // Removing health
        {
            // Play hurt sound
            AudioManager.instance.PlayOneShot(FMODEvents.instance.turretHurt, transform.position);

            // Health can't decrease below 0
            if (health - amt > 0)
            {
                health -= amt;
                StatisticsTracker.damageDealt += amt;
            }
            else // 0 health = dead
            {
                StatisticsTracker.damageDealt += health;
                StatisticsTracker.kills += 1;
                GameObject.Find("PlayerCapsule").GetComponent<PlayerInventory>().heldCredits += creditValue;
                health = 0;
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Reloads the turret's weapon
    /// </summary>
    private void Reload()
    {
        readyToShoot = true;
    }

    private void OnDestroy()
    {
        turretCount--;

        if (turretCount == 0)
        {
            EnemySpawner.instance.spawningEnabled = false;
            GlobalReferences.instance.door3Button.interactable = true;
            UiHandler.instance.ShowNotification("New Area Unlocked", 3f);
        }
    }
}
