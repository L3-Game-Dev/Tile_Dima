// MinimapIconSpawner
// Handles spawning of minimap icons
// Created by Dima Bethune 22/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIconSpawner : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject playerIcon;
    public GameObject enemyIcon;
    public GameObject bossIcon;

    [Header("Variables")]
    public float iconHeight;

    private void Start()
    {
        // Player Icon
        InstantiateIcon(GameObject.Find("PlayerCapsule"), playerIcon);

        // Enemy Icons
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<EnemyStats>())
                InstantiateIcon(enemy, enemyIcon);
            else if (enemy.GetComponent<MinibossStats>())
                InstantiateIcon(enemy, bossIcon);
        }
    }

    /// <summary>
    /// Spawns a minimap icon from the provided references
    /// </summary>
    /// <param name="owner">The object to attach the icon to</param>
    /// <param name="prefab">The icon to spawn</param>
    public void InstantiateIcon(GameObject owner, GameObject prefab)
    {
        Vector3 newPosition = owner.transform.position;
        newPosition.y = iconHeight;
        GameObject icon = Instantiate(prefab, newPosition, new Quaternion(0, 0, 0, 0), transform);

        icon.GetComponent<MinimapIcon>().owner = owner;
    }
}