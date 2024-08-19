// GameStateHandler
// Handles game states & switching between them
// Created by Dima Bethune 17/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public static string gameState = "PLAYING";

    /// <summary>
    /// Sets the gameState to PAUSED
    /// </summary>
    /// <param name="unlockCursor"></param>
    public static void Pause(bool unlockCursor = true)
    {
        gameState = "PAUSED";
        Time.timeScale = 0; // Pause time

        if (unlockCursor)
        {
            Cursor.visible = true; // Show the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        }
    }

    /// <summary>
    /// Sets the gameState to PLAYING
    /// </summary>
    /// <param name="lockCursor"></param>
    public static void Resume(bool lockCursor = true)
    {
        gameState = "PLAYING";
        Time.timeScale = 1; // Resume time

        if (lockCursor)
        {
            Cursor.visible = false; // Hide the cursor
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        }
    }

    /// <summary>
    /// Sets the gameState to DEFEAT
    /// </summary>
    public static void Defeat()
    {
        gameState = "DEFEAT";
    }

    /// <summary>
    /// Sets the gameState to VICTORY
    /// </summary>
    public static void Victory()
    {
        gameState = "VICTORY";
    }

    /// <summary>
    /// Sets the gameState to STATISTICS
    /// </summary>
    public static void Statistics()
    {
        gameState = "STATISTICS";
        Cursor.visible = true; // Show cursor
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
    }

    /// <summary>
    /// Sets the gameState to ENTERNAME
    /// </summary>
    public static void EnterName()
    {
        gameState = "ENTERNAME";
    }

    /// <summary>
    /// Sets the gameState to HIGHSCORES
    /// </summary>
    public static void Highscores()
    {
        gameState = "HIGHSCORES";
    }
}
