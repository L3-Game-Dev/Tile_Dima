// PlayerInventory
// Handles the player's inventory
// Created by Dima Bethune 15/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerInventory : MonoBehaviour
{
    [Header("References")]
    [HideInInspector] public List<Weapon> heldWeapons = new List<Weapon>(4);
    public Weapon equippedWeapon;
    public GrenadeThrower grenadeThrower;
    [HideInInspector] public StarterAssetsInputs input;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public UiHandler uiHandler;

    public LayerMask playerWeaponLayer;
    public LayerMask interactableLayer;

    public GameObject tempWeapon;

    public int heldCredits;

    private void Awake()
    {
        // Initialise variable
        heldCredits = 0;

        // Set references
        input = GetComponent<StarterAssetsInputs>();
        playerStats = GetComponent<PlayerStats>();
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();

        // Add every existing player weapon to inventory
        foreach (Transform w in transform.Find("PlayerCameraRoot"))
        {
            if (w.CompareTag("PlayerWeapon")) // Objects with 'PlayerWeapon' tag
            {
                heldWeapons.Add(w.gameObject.GetComponent<Weapon>()); // Add to inventory
            }
        }

        // If weapon inventory is not empty
        if (heldWeapons.Count != 0)
        {
            equippedWeapon = heldWeapons[0]; // Equip the first weapon
            uiHandler.UpdateInventoryUI(heldWeapons);
        }
        else // If it is empty
        {
            Debug.Log("No weapons found...");
        }

        UpdateEnabledWeapons();
    }

    private void Update()
    {
        if (!playerStats.isDead && GameStateHandler.gameState == "PLAYING")
        {
            EquipWeaponInput();
            DropWeapon();
            uiHandler.UpdateAmmoNumberUI(heldWeapons);
        }
    }

    /// <summary>
    /// Equip specified weapon on player input
    /// </summary>
    public void EquipWeaponInput()
    {
        if (input.equipWeapon1 && heldWeapons.Count >= 1)
            EquipWeapon(0);
        if (input.equipWeapon2 && heldWeapons.Count >= 2)
            EquipWeapon(1);
        if (input.equipWeapon3 && heldWeapons.Count >= 3)
            EquipWeapon(2);
        if (input.equipWeapon4 && heldWeapons.Count >= 4)
            EquipWeapon(3);
    }

    /// <summary>
    /// Equip weapon in the provided index
    /// </summary>
    /// <param name="i">The index of weapon to equip</param>
    public void EquipWeapon(int i)
    {
        if (!equippedWeapon.Equals(null))
        {
            // Cancel previous weapon's reloading
            equippedWeapon.CancelInvoke("ReloadFinished");
            equippedWeapon.reloading = false;
        }
        // Equip new weapon
        equippedWeapon = heldWeapons[i];
        UpdateEnabledWeapons();
        equippedWeapon.gameObject.SetActive(true);
    }

    /// <summary>
    /// Enables the equipped weapon and disables all other held weapons
    /// </summary>
    public void UpdateEnabledWeapons()
    {
        // Disable all non-equipped weapons, enabled equipped weapon
        foreach (Weapon w in heldWeapons)
        {
            if (w != equippedWeapon)
                w.gameObject.SetActive(false);
            else
            {
                w.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Adds the specified weapon into the player's inventory
    /// </summary>
    /// <param name="weapon"></param>
    public void PickupWeapon(GameObject weapon)
    {
        if (heldWeapons.Count + 1 > heldWeapons.Capacity)
        {
            uiHandler.ToggleUI(true, uiHandler.fullInventory);
            CancelInvoke("HideFullInventory");
            Invoke("HideFullInventory", 1f);
        }
        else
        {
            // Set weapon's parent to playercameraroot
            weapon.transform.SetParent(GameObject.Find("PlayerCameraRoot").transform);

            // Set weapon's layer to weapon
            weapon.layer = LayerMask.NameToLayer("Weapon");

            // Reset weapon's position
            weapon.transform.localPosition = new Vector3(0.294f, -0.258f, 0.108f);
            weapon.transform.localEulerAngles = Vector3.zero;

            //GameObject newWeapon = Instantiate(tempWeapon, GameObject.Find("PlayerCameraRoot").transform);
            heldWeapons.Add(weapon.GetComponent<Weapon>());
        }

        uiHandler.UpdateInventoryUI(heldWeapons);
        UpdateEnabledWeapons();
    }

    /// <summary>
    /// Hides the 'full inventory' UI prompt
    /// </summary>
    public void HideFullInventory()
    {
        uiHandler.ToggleUI(false, uiHandler.fullInventory);
    }

    /// <summary>
    /// Drops the currently equipped weapon
    /// </summary>
    public void DropWeapon()
    {
        if (heldWeapons.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                // PRINT CURRENT POS AND ROT
                //Debug.Log(equippedWeapon.transform.localPosition + ", " + equippedWeapon.transform.localEulerAngles);

                // Get index of current equipped weapon
                int index = heldWeapons.IndexOf(equippedWeapon);

                // Move weapon to 'droppeditems' category
                equippedWeapon.transform.SetParent(GameObject.Find("DroppedItems").transform);

                // Set weapon's layer to interactable
                equippedWeapon.gameObject.layer = LayerMask.NameToLayer("Interactable");

                // Set new position & rotation
                Vector3 newPos = transform.position;
                newPos.y = transform.position.y;

                Vector3 newRot = new Vector3(0, Random.Range(0, 360), 90);

                // Apply to the weapon
                equippedWeapon.transform.position = newPos;
                equippedWeapon.transform.eulerAngles = newRot;

                // Remove the weapon from player's inventory
                heldWeapons.Remove(heldWeapons[index]);

                // Equip new weapon
                if (index - 1 >= 0)
                    EquipWeapon(index - 1);
                else
                    EquipWeapon(0);

                // Update UI
                UpdateEnabledWeapons();
                uiHandler.UpdateInventoryUI(heldWeapons);
            }
        }
    }
}
