// MinimapIcon
// Handles movement following for minimap icons
// Created by Dima Bethune 22/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public GameObject owner;

    private void Update()
    {
        if (owner.GetComponent<EnemyStats>() != null)
        {
            if (!owner.GetComponent<EnemyStats>().isDead)
                UpdatePosition();
            else
                DeleteIcon();
        }
        else
            UpdatePosition();
    }

    /// <summary>
    /// Moves the icon to its owner's position, excluding y axis which is static
    /// </summary>
    public void UpdatePosition()
    {
        Vector3 newPosition = owner.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

    /// <summary>
    /// Destroys the icon gameObject
    /// </summary>
    public void DeleteIcon()
    {
        Destroy(gameObject);
    }
}
