using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;


public class PlayerUI : MonoBehaviour {
    GameObject simParent;
    VRTK_ControllerEvents controllerEvents;

    Slider scaleSlider;
    Text scaleValue;

    Slider timeSlider;
    Text timeValue;
    Text timeHigher;


    // Use this for initialization
    protected void Awake()
    {
        scaleSlider = GameObject.Find("UI/Player/Panel/Scale/Slider").GetComponent<Slider>();
        scaleValue = GameObject.Find("UI/Player/Panel/Scale/Value").GetComponent<Text>();

        timeSlider = GameObject.Find("UI/Player/Panel/Time/Slider").GetComponent<Slider>();
        timeValue = GameObject.Find("UI/Player/Panel/Time/Value").GetComponent<Text>();
        timeHigher = GameObject.Find("UI/Player/Panel/Time/HigherBound").GetComponent<Text>();

        simParent = GameObject.Find("Simulations");
        controllerEvents = GameObject.Find("RightController").GetComponent<VRTK_ControllerEvents>();

        controllerEvents.TriggerPressed += new ControllerInteractionEventHandler(TriggerPressed);
        controllerEvents.TriggerReleased += new ControllerInteractionEventHandler(TriggerReleased);
    }

    public void Update()
    {
        timeHigher.text = Convert.ToString(Simulation.length * Simulation.deltaTime) + "s";
        // timeSlider.value = (float)Simulation.time * Simulation.deltaTime;
        timeSlider.maxValue = (float)Simulation.length * Simulation.deltaTime;


        float scale = Mathf.Pow(Simulation.scale, -1f);
        scaleValue.text = "1/" + Convert.ToString(scale);
        scaleSlider.value = scale;
    }

    private void TriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Show();
    }

    private void TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        Hide();
    }

    public void Show()
    {
        Debug.Log("show player ui");
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("hide player ui");
        this.gameObject.SetActive(false);
    }

    public void Play()
    {
        Simulation.Play();
        Debug.Log("Play pressed.");
    }

    public void Pause()
    {
        Simulation.Pause();
        Debug.Log("Pause pressed.");
    }

    public void Rewind()
    {
        int currentTime = (int) ((float)Simulation.time * Simulation.deltaTime);
        if (currentTime < 16) {
            Simulation.SetTime(0);
        } else {
            Simulation.SetTime(currentTime - 15);
        }
        Debug.Log("Rewind pressed.");
    }

    public void Forward()
    {
        int currentTime = (int)((float)Simulation.time * Simulation.deltaTime);
        int length = (int)((float)Simulation.length / Simulation.deltaTime);
        if (currentTime < length - 15)
        {
            Simulation.SetTime(currentTime + 15);
        }
        else
        {
            Simulation.SetTime(length);
        }
        Debug.Log("Forward pressed.");
    }

    public void ScaleChanged(float newScale)
    {
        Debug.Log("scale changed");
        Debug.Log(newScale);
        scaleValue.text = "1/" + Convert.ToString(newScale);

        foreach (Simulation sim in simParent.GetComponentsInChildren<Simulation>()) {
            sim.SetScale(1f / newScale);
        }
    }

    public void TimeChanged(float newTime)
    {
        Debug.Log("time changed");
        timeValue.text = Convert.ToString(newTime) + "s";
        Simulation.SetTime((int) (newTime / Simulation.deltaTime));
    }
}
