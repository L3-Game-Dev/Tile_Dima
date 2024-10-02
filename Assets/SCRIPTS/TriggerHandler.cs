// TriggerHandler
// Handles events created by collider triggers
// Created by Dima Bethune 26/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerHandler : MonoBehaviour
{
    public UnityEvent method;

    [HideInInspector] public UiHandler uiHandler;

    private void Awake()
    {
        uiHandler = GameObject.Find("-- UI ELEMENTS --").GetComponent<UiHandler>();
    }

    /// <summary>
    /// Executes a specific method on collision
    /// </summary>
    /// <param name="other">The object collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            method.Invoke();
    }

    /// <summary>
    /// Awakens the provided miniboss
    /// </summary>
    /// <param name="miniboss">The miniboss to awaken</param>
    public void AwakenMiniboss(MinibossController miniboss)
    {
        if (miniboss != null)
        {
            if (!miniboss.GetComponent<MinibossStats>().isDead)
            {
                miniboss.anim.SetBool("movementEnabled", true);
                uiHandler.EnableBossBar(miniboss);
                AudioManager.instance.SwitchMusicTrack(MusicTrack.BOSS_FIGHT);

                // Handle color change
                PostProcessing.instance.BeginColorTransition(Color.white, Color.red);
            }
        }
    }
}
