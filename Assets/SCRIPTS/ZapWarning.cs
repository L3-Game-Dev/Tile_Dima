using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapWarning : MonoBehaviour
{
    public void Zap()
    {
        TeslaCoil.instance.Zap();
        Destroy(transform.parent.gameObject);
    }
}