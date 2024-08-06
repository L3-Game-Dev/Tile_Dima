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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            method.Invoke();
    }

    public void AwakenMiniboss(MinibossController miniboss)
    {
        if (miniboss != null)
        {
            if (!miniboss.GetComponent<MinibossStats>().isDead)
            {
                miniboss.anim.SetBool("movementEnabled", true);
                uiHandler.EnableBossBar(miniboss);
                AudioManager.instance.SwitchMusicTrack(MusicTrack.BOSS_FIGHT);
            }
        }
    }
}
