// EnemySpawner
// Handles spawning enemies at active spawn locations
// Created by Dima Bethune 26/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public int activeTile;

    public List<Transform> spawnLocations;
    public Transform spawnLocationParent;

    public GameObject currentEnemy;

    public float spawnInterval;

    public bool spawningEnabled;
    [HideInInspector] public bool spawning;
    [HideInInspector] public MinimapIconSpawner iconSpawner;

    private void Awake()
    {
        iconSpawner = GameObject.Find("MinimapIcons").GetComponent<MinimapIconSpawner>();

        spawnLocations = new List<Transform>();
        spawnLocations.Clear();

        foreach (Transform location in GameObject.Find("EnemySpawnLocations").transform)
            spawnLocations.Add(location);
    }

    private void Start()
    {
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

    private IEnumerator SpawnInterval()
    {
        spawning = true;
        yield return new WaitForSeconds(spawnInterval);
        Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];
        SpawnEnemy(currentEnemy, spawnLocation);
        spawning = false;
    }

    private void SpawnEnemy(GameObject enemyPrefab, Transform location)
    {
        GameObject enemy = Instantiate(enemyPrefab, location.position, location.rotation, spawnLocationParent);
        iconSpawner.InstantiateIcon(enemy, iconSpawner.enemyIcon);
    }

    public void EnableSpawning(bool enabled)
    {
        spawningEnabled = enabled;
    }
}
