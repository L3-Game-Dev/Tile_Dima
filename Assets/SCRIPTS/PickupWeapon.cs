using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [HideInInspector] public PlayerInteract interact;
    [HideInInspector] public PlayerInventory inventory;

    private void Awake()
    {
        interact = GameObject.Find("PlayerCapsule").GetComponent<PlayerInteract>();
        inventory = GameObject.Find("PlayerCapsule").GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        PickupWeaponCheck();
    }

    public void PickupWeaponCheck()
    {
        foreach (Transform weaponObject in transform)
        {
            if (interact.lookingAt == weaponObject.gameObject && GameStateHandler.gameState == "PLAYING")
            {
                if (Input.GetKeyDown(interact.keyBind)) // If player presses interact keyBind
                {
                    // Pickup the weapon
                    inventory.PickupWeapon(weaponObject.gameObject);
                }
            }
        }
    }
}
