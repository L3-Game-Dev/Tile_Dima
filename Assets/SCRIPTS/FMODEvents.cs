// FMODEvents
// Stores references to general FMOD events
// Created by Dima Bethune 05/07

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] public EventReference playerFootstepsMetal;
    [SerializeField] public EventReference enemyFootstepMetal;
    [SerializeField] public EventReference zombieAttack;
    [SerializeField] public EventReference playerDeath;
    [SerializeField] public EventReference playerHit;
    [SerializeField] public EventReference kick;

    [Header("UI")]
    [SerializeField] public EventReference uiClick1;
    [SerializeField] public EventReference uiClick2;
    [SerializeField] public EventReference typing;

    [Header("Music")]
    [SerializeField] public EventReference musicTrack1;
    [SerializeField] public EventReference mainMenuTrack;

    [Header("Ambience")]
    [SerializeField] public EventReference humAmbience;

    // Singleton functionality
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one FMODEvents found");
        }
        instance = this;
    }
}
