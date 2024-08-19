// TimerHandler
// Handles game timer
// Created by Dima Bethune 20/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerHandler : MonoBehaviour
{
    [HideInInspector] public float currentTime;
    [HideInInspector] public TimeSpan time;

    public TextMeshProUGUI timerText;

    private void Update()
    {
        UpdateTimer();
    }

    /// <summary>
    /// Updates the timer display and currentTime
    /// </summary>
    private void UpdateTimer()
    {
        switch (GameStateHandler.gameState)
        {
            case "PLAYING":
                currentTime += Time.deltaTime;
                time = TimeSpan.FromSeconds(currentTime);
                string minutes = time.Minutes.ToString("D2");
                string seconds = time.Seconds.ToString("D2");
                string milliseconds = (time.Milliseconds/10).ToString("D2");
                timerText.text = "<mspace=.9em>" + minutes + "</mspace>:<mspace=.9em>" + seconds + "</mspace>:<mspace=.9em>" + milliseconds + "</mspace>";
                break;
            default:
                break;
        }
    }
}
