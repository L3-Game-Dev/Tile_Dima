// CameraShake
// Handles the player camera's shaking
// Created by Dima Bethune 11/09

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [Header("Impulse Sources")]
    public CinemachineImpulseSource recoilSource;
    public CinemachineImpulseSource explosionSource;
    public CinemachineImpulseSource hitSource;
    public CinemachineImpulseSource jumpSource;

    public static CameraShake instance;

    public void Awake()
    {
        instance = this;
    }

    public void RecoilShake()
    {
        recoilSource.GenerateImpulse();
    }

    public void ExplosionShake()
    {
        explosionSource.GenerateImpulse();
    }

    public void HitShake()
    {
        hitSource.GenerateImpulse();
    }

    public void JumpShake()
    {
        jumpSource.GenerateImpulse();
    }
}