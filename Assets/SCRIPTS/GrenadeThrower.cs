// Weapon
// Stores weapon stats and handles the weapon's functionality
// Created by Dima Bethune 05/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GrenadeThrower : MonoBehaviour
{
    [Header("Weapon Stats")]
    [Tooltip("Interval between grenade throws")]
    public float throwInterval;
    [Tooltip("Maximum grenade storage")]
    public int maxGrenadesHeld;
    [Tooltip("Current # of grenades held")]
    public int grenadesHeld;

    [Header("Bool Variables")]
    [Tooltip("Whether the grenade is ready to throw")]
    public bool readyToThrow;
    [Tooltip("Whether the player is currently trying to throw")]
    public bool throwing;

    private bool allowInvoke; // Bug Fix

    [Header("References")]
    [HideInInspector] public Camera playerCamera;
    public Transform throwPoint;
    public GameObject grenade;
    public Sprite grenadeSprite;
    [HideInInspector] public UiHandler uiHandler;

    private void Awake()
    {
        // Set references
        playerCamera = GameObject.Find("PlayerCharacter").transform.Find("MainCamera").GetComponent<Camera>();
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();

        // Make sure grenade storage is full on startup
        grenadesHeld = maxGrenadesHeld;
        readyToThrow = true;
        allowInvoke = true;
    }

    private void Start()
    {
        uiHandler.grenadeDisplayAmmoNumber.text = grenadesHeld.ToString();
    }

    public void Throw()
    {
        readyToThrow = false; // No longer ready to throw

        // Find the exact hit position using a raycast
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - throwPoint.position;

        // Insantiate grenade
        GameObject currentGrenade = Instantiate(grenade, throwPoint.position, Quaternion.identity, GameObject.Find("PlayerProjectiles").transform);

        // Rotate grenade to throw direction
        currentGrenade.transform.forward = directionWithoutSpread.normalized;

        // Add forces to grenade
        currentGrenade.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * grenade.GetComponent<Grenade>().throwSpeed, ForceMode.Impulse);

        grenadesHeld--; // Take away one from held grenades amount

        // Update text
        uiHandler.grenadeDisplayAmmoNumber.text = grenadesHeld.ToString();

        // Play throw sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.grenadeThrow1, transform.position);

        // Invoke resetShot function
        if (allowInvoke)
        {
            Invoke("ResetThrow", throwInterval);
            allowInvoke = false;
        }
    }

    private void ResetThrow()
    {
        readyToThrow = true;
        allowInvoke = true;
    }
}