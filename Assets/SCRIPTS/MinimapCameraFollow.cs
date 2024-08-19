// MinimapCameraFollow
// Handles moving the minimap camera to follow the player
// Created by Dima Bethune 21/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform player;

    private void Awake()
    {
        player = GameObject.Find("PlayerCapsule").transform;
    }

    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
