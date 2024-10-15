// FMODEvents
// Stores references to all FMOD events
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
    [SerializeField] public EventReference minibossFootstepMetal;
    [SerializeField] public EventReference zombieAttack;
    [SerializeField] public EventReference zombieHurt;
    [SerializeField] public EventReference turretShoot;
    [SerializeField] public EventReference turretHurt;
    [SerializeField] public EventReference playerDeath;
    [SerializeField] public EventReference playerHit;
    [SerializeField] public EventReference kick;
    [SerializeField] public EventReference grenadeExplosion1;
    [SerializeField] public EventReference grenadeThrow1;
    [SerializeField] public EventReference zapCharge;
    [SerializeField] public EventReference zap;
    [SerializeField] public EventReference explosion;
    [SerializeField] public EventReference dink;

    [Header("UI")]
    [SerializeField] public EventReference beep;
    [SerializeField] public EventReference success;
    [SerializeField] public EventReference uiClick1;
    [SerializeField] public EventReference uiClick2;
    [SerializeField] public EventReference upgrade1;
    [SerializeField] public EventReference error1;
    [SerializeField] public EventReference typing;
    [SerializeField] public EventReference victory;
    [SerializeField] public EventReference defeat;

    [Header("Music")]
    [SerializeField] public EventReference musicTrack1;
    [SerializeField] public EventReference mainMenuTrack;

    [Header("Ambience")]
    [SerializeField] public EventReference humAmbience;

    // Singleton functionality
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
