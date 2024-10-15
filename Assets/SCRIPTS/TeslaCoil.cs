using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCoil : MonoBehaviour
{
    [Header("Singleton")]
    public static TeslaCoil instance;

    [Header("References")]
    public GameObject zapWarning;
    public GameObject tempZapWarning;
    public GameObject zap;
    public Transform target;

    [Header("Values")]
    private bool isEnabled;
    private bool isOnCooldown;
    private float attackCooldown;

    private void Awake()
    {
        // Singleton functionality
        if (instance == null)
            instance = this;

        isEnabled = false;
    }

    private void Update()
    {
        if (isEnabled)
        {
            if (!isOnCooldown)
            {
                StartZap();
            }
        }
    }

    public void StartZap()
    {
        // Show the attack warning stuff here
        tempZapWarning = Instantiate(zapWarning, target.position, new Quaternion(0, 0, 0, 0));
    }

    public void Zap()
    {
        // Do the attack functionality here
        GameObject obj = Instantiate(zap, tempZapWarning.transform.position, new Quaternion(0, 0, 0, 0));

        isOnCooldown = true;
        Invoke("ReadyAttack", attackCooldown);
    }

    private void ReadyAttack()
    {
        isOnCooldown = false;
    }

    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
    }
}
