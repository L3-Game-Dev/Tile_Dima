// EnemySpawner
// Handles spawning enemies at active spawn locations
// Created by Dima Bethune 26/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Singleton")]
    public static EnemySpawner instance;

    public List<Transform> spawnLocations;
    public Transform spawnLocationParent;

    public GameObject currentEnemy;

    public float spawnInterval;

    public bool spawningEnabled;
    [HideInInspector] public bool spawning;
    [HideInInspector] public MinimapIconSpawner iconSpawner;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;

        spawnLocations = new List<Transform>();
        spawnLocations.Clear();
    }

    private void Start()
    {
        // Set references
        iconSpawner = GameObject.Find("MinimapIcons").GetComponent<MinimapIconSpawner>();

        // Initialise variables
        spawningEnabled = false;
        spawning = false;
    }

    private void Update()
    {
        if (spawningEnabled && !spawning)
        {
            StartCoroutine("SpawnInterval");
        }
    }

    /// <summary>
    /// Sets the tile in which to spawn enemies
    /// </summary>
    /// <param name="i">The tile to set to</param>
    public void SetSpawningTile(int i)
    {
        string tile = "Tile ";
        tile += i.ToString();

        if (i < 1 || i > 5)
        {
            Debug.Log("Invalid Spawning Tile");
        }
        else
        {
            spawnLocations.Clear();

            foreach (Transform location in GameObject.Find("EnemySpawnLocations").transform.Find(tile))
                spawnLocations.Add(location);
        }
    }

    /// <summary>
    /// Spawns the currently selected enemy after an interval
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnInterval()
    {
        spawning = true;
        yield return new WaitForSeconds(spawnInterval);
        Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];
        SpawnEnemy(currentEnemy, spawnLocation);
        spawning = false;
    }

    /// <summary>
    /// Spawns a given enemy at a given location
    /// </summary>
    /// <param name="enemyPrefab">The enemy to spawn</param>
    /// <param name="location">The location to spawn at</param>
    private void SpawnEnemy(GameObject enemyPrefab, Transform location)
    {
        GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation, spawnLocationParent);
        iconSpawner.InstantiateIcon(enemy, iconSpawner.enemyIcon);
    }

    /// <summary>
    /// Enables/disables enemy spawning
    /// </summary>
    /// <param name="enabled">Spawning enabled</param>
    public void EnableSpawning(bool enabled)
    {
        spawningEnabled = enabled;
    }
}
