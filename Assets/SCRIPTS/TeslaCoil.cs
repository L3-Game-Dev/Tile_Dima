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
    public float attackCooldown;

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

        // Play zap warning sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.zapCharge, tempZapWarning.transform.position);

        isOnCooldown = true;
        Invoke("ReadyAttack", attackCooldown);
    }

    public void Zap()
    {
        // Do the attack functionality here
        GameObject obj = Instantiate(zap, tempZapWarning.transform.position, new Quaternion(0, 0, 0, 0));

        // Play zap sound
        AudioManager.instance.PlayOneShot(FMODEvents.instance.zap, tempZapWarning.transform.position);
    }

    private void ReadyAttack()
    {
        isOnCooldown = false;
    }

    public void SetEnabled()
    {
        Invoke("EnableZapping", 5f);
    }

    public void EnableZapping()
    {
        isEnabled = enabled;
    }
}
