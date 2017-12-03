﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    Simulation sim;
    InteractionMarker interaction;

    Slider pitchSlider;
    Text pitchValue;

    Slider thrustSlider;
    Text thrustValue;

    Slider timeSlider;
    Text timeValue;
    Text timeHigher;


    // Use this for initialization
    protected void Awake()
    {
        pitchSlider = GameObject.Find("UI/Interaction/Panel/Pitch/Slider").GetComponent<Slider>();
        pitchValue = GameObject.Find("UI/Interaction/Panel/Pitch/Value").GetComponent<Text>();


        thrustSlider = GameObject.Find("UI/Interaction/Panel/Thrust/Slider").GetComponent<Slider>();
        thrustValue = GameObject.Find("UI/Interaction/Panel/Thrust/Value").GetComponent<Text>();

        timeSlider = GameObject.Find("UI/Interaction/Panel/Time/Slider").GetComponent<Slider>();
        timeValue = GameObject.Find("UI/Interaction/Panel/Time/Value").GetComponent<Text>();
        timeHigher = GameObject.Find("UI/Interaction/Panel/Time/HigherBound").GetComponent<Text>();
    }

    public void Init(InteractionMarker interaction, Simulation sim)
    {
        this.interaction = interaction;
        this.sim = sim;
    }

    public void Show()
    {
        Debug.Log("show interaction ui");
        pitchSlider.value = (float)interaction.GetPitch();
        thrustSlider.value = (float)interaction.GetThrust() * 100f;

        timeHigher.text = Convert.ToString(Simulation.length * Simulation.deltaTime) + "s";
        timeSlider.value = (float)interaction.GetTime() * Simulation.deltaTime;
        timeSlider.maxValue = Simulation.length * Simulation.deltaTime;

        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("hide interaction ui");
        this.gameObject.SetActive(false);
    }

    public void PitchChanged(float newPitch)
    {
        Debug.Log("pitch changed");
        pitchValue.text = Convert.ToString(newPitch) + "°";
        interaction.SetPitch(newPitch);
    }

    public void ThrustChanged(float newThrust)
    {
        Debug.Log("thrust changed");
        thrustValue.text = Convert.ToString(newThrust) + "%";
        interaction.SetThrust(newThrust / 100f);
    }

    public void TimeChanged(float newTime)
    {
        Debug.Log("time changed");
        timeValue.text = Convert.ToString(newTime) + "s";
        interaction.SetTime((int)(newTime / Simulation.deltaTime));
    }
}