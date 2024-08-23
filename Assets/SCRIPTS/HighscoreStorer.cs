// HighscoreStorer
// Handles storing & retrieving highscores
// Created by Dima Bethune 22/06

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class HighscoreStorer : MonoBehaviour
{
    public static string file = "random_first_names.csv"; // Testing file 1 (100 values)
    //public static string file = "blank.csv"; // Testing file 2 (blank data)
    //public static string file = "highscores.csv";
    public static string path = Application.dataPath + "/HIGHSCORES/" + file;
    public static string data;

    // Whether new highscores are currently being stored
    public static bool storingHighscores = false;

    /// <summary>
    /// Reads the currently selected file and sends the data to data string
    /// </summary>
    public static void ReadFile()
    {
        data = "";

        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            StreamReader read = new StreamReader(fileStream);
            data = read.ReadToEnd();
            fileStream.Close();
        }
        else Debug.Log("No file found at " + path);
    }

    /// <summary>
    /// Saves a highscore to file
    /// </summary>
    /// <param name="name">Highscore person's name</param>
    /// <param name="time">Highscore time value</param>
    public static void SaveHighscore(string name, string time)
    {
        if (storingHighscores)
        {
            StreamWriter writer = new StreamWriter(path, true);

            writer.Write("\n" + time + "," + name);

            writer.Flush();
            writer.Close();
        }
    }

    /// <summary>
    /// Gets the number of highscores in the data string
    /// </summary>
    /// <returns>Number of lines</returns>
    public static int GetHighscoreCount()
    {
        ReadFile();

        string[] lines = data.Split("\n");

        return lines.Length;
    }

    /// <summary>
    /// Gets the highscore values at a specified index
    /// </summary>
    /// <param name="i">The index to get</param>
    /// <returns>The highscore name & time as a double string Tuple</returns>
    public static Tuple<string, string> GetHighscores(int i)
    {
        ReadFile();

        string[] lines = data.Split("\n");
        Array.Sort(lines);

        string[] parts = lines[i].Split(",");

        if (parts.Length > 1)
            return new Tuple<string, string>(parts[1], parts[0]);
        else
            return null;
    }
}
