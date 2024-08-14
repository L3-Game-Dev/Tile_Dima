// GameStateHandler
// Handles game states & switching between them
// Created by Dima Bethune 17/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public static string gameState = "PLAYING";

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

    public static void Defeat()
    {
        gameState = "DEFEAT";
    }

    public static void Victory()
    {
        gameState = "VICTORY";
    }

    public static void Statistics()
    {
        gameState = "STATISTICS";
        Cursor.visible = true; // Show cursor
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
    }

    public static void EnterName()
    {
        gameState = "ENTERNAME";
    }

    public static void Highscores()
    {
        gameState = "HIGHSCORES";
    }
}
