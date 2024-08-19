// HitmarkerHandler
// Handles spawning & despawning of hitmarkers
// Created by Dima Bethune 02/08

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitmarkerHandler : MonoBehaviour
{
    public float lifetime;

    [HideInInspector] public Transform target;
    public TextMeshProUGUI textObj;

    private void Awake()
    {
        // Set reference
        target = GameObject.Find("MainCamera").transform;
    }

    private void Start()
    {
        // Destroys the hitmarker after lifetime
        Invoke("DestroyHitmarker", lifetime);
    }

    private void Update()
    {
        // Rotate to look at the player
        transform.LookAt(transform.position - (target.position - transform.position));
    }

    /// <summary>
    /// Sets the hitmarker's text number to provided float
    /// </summary>
    /// <param name="n">The number to set</param>
    public void SetNumber(float n)
    {
        textObj.text = "-" + n.ToString();
    }

    /// <summary>
    /// Destroys the hitmarker gameObject
    /// </summary>
    public void DestroyHitmarker()
    {
        Destroy(gameObject);
    }
}
