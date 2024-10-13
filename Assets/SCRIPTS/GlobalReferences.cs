using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalReferences : MonoBehaviour
{
    [Header("Singleton")]
    public static GlobalReferences instance;

    [Header("References")]
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    public Button door1Button;
    public Button door2Button;
    public Button door3Button;
    public Button door4Button;
    public Button door5Button;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
