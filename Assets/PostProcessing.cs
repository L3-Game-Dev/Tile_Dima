// PostProcessing
// Handles changes of post processing effects
// Created by Dima Bethune 22/06

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessing : MonoBehaviour
{
    // Singleton
    public static PostProcessing instance;

    [SerializeField] private Volume volume;
    private ColorAdjustments colorAdjustments;

    private bool transitioning;
    Color color1, color2;
    private float targetPoint;
    public float transitionTime;

    private void Awake()
    {
        // Set singleton reference
        if (instance == null)
            instance = this;

        transitioning = false;
        targetPoint = 0;
    }

    private void Update()
    {
        if (transitioning)
            TransitionColor();
    }

    /// <summary>
    /// Transitions between color1 and color2 over transitionTime
    /// </summary>
    private void TransitionColor()
    {
        targetPoint += Time.deltaTime / transitionTime;
        UpdateColorFilter(Color.Lerp(color1, color2, targetPoint));
    }
    
    /// <summary>
    /// Starts transitioning from color1 to color2
    /// </summary>
    /// <param name="color1">Color to transition from</param>
    /// <param name="color2">Color to transition to</param>
    public void BeginColorTransition(Color color1, Color color2)
    {
        transitioning = true;
        targetPoint = 0;
        this.color1 = color1;
        this.color2 = color2;
    }

    /// <summary>
    /// Sets the post processing color filter to the input color
    /// </summary>
    /// <param name="color">The color to change to</param>
    public void UpdateColorFilter(Color color)
    {
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        colorAdjustments.colorFilter.value = color;
    }
}
