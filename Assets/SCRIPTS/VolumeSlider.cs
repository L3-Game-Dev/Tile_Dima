// VolumeSlider
// Handles changing volumes based on UI volume sliders
// Created by Dima Bethune 12/08

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            default:
                Debug.Log("Unsupported Volume Type: " + volumeType);
                break;
        }
    }

    /// <summary>
    /// Sets relevant volume to entered value
    /// </summary>
    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                break;
            default:
                Debug.Log("Unsupported Volume Type: " + volumeType);
                break;
        }
    }
}
