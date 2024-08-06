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

    [Header("Music")]
    [SerializeField] public EventReference musicTrack1;
    [SerializeField] public EventReference mainMenuTrack;

    // Singleton functionality
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one AudioManager found");
        }
        instance = this;
    }
}
