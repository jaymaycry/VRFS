using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    Simulation sim;
    Interaction interaction;

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

        EventManager.OnOpenInteractionUI += Open;
        EventManager.OnCloseInteractionUI += Close;
    }

    protected void Init()
    {
        pitchSlider.value = (float)interaction.pitch;
        thrustSlider.value = (float)interaction.thrust * 100f;

        timeSlider.value = (float)interaction.time * sim.deltaTime;
        timeSlider.maxValue = sim.length * sim.deltaTime;

        UpdateValue();
    }

    protected void UpdateValue()
    {
        timeHigher.text = Convert.ToString(sim.length * sim.deltaTime) + "s";
        pitchValue.text = Convert.ToString(interaction.pitch) + "°";
        thrustValue.text = Convert.ToString(interaction.thrust * 100) + "%";
        timeValue.text = Convert.ToString((int)interaction.time * sim.deltaTime) + "s";
    }

    public void Open(Simulation sim, Interaction interaction)
    {
        this.interaction = interaction;
        Debug.Log("interaction edit bool:");
        Debug.Log(interaction.edit);
        this.sim = sim;
        Show();
    }

    public void TriggerClose()
    {
        EventManager.CloseInteractionUI();
    }

    public void Close()
    {
        Hide();
    }

    public void Show()
    {
        Debug.Log("show interaction ui");
        Init();
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
        interaction.pitch = newPitch;
        UpdateValue();
        EventManager.InteractionChanged(sim);
    }

    public void ThrustChanged(float newThrust)
    {
        Debug.Log("thrust changed");
        interaction.thrust = newThrust / 100f;
        UpdateValue();
        EventManager.InteractionChanged(sim);
    }

    public void TimeChanged(float newTime)
    {
        Debug.Log("time changed");
        timeValue.text = Convert.ToString(newTime) + "s";
        interaction.time = (int)(newTime / sim.deltaTime);
        UpdateValue();
        EventManager.InteractionChanged(sim);
    }
}
