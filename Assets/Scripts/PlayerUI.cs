using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
    GameObject simParent;

    Slider scaleSlider;
    Text scaleValue;

    Slider timeSlider;
    Text timeValue;
    Text timeHigher;


    // Use this for initialization
    protected void Awake ()
    {
        scaleSlider = GameObject.Find("UI/Player/Panel/Scale/Slider").GetComponent<Slider>();
        scaleValue = GameObject.Find("UI/Player/Panel/Scale/Value").GetComponent<Text>();

        timeSlider = GameObject.Find("UI/Player/Panel/Time/Slider").GetComponent<Slider>();
        timeValue = GameObject.Find("UI/Player/Panel/Time/Value").GetComponent<Text>();
        timeHigher = GameObject.Find("UI/Player/Panel/Time/HigherBound").GetComponent<Text>();

        simParent = GameObject.Find("Simulations");
    }

    public void Update()
    {
        
        timeHigher.text = Convert.ToString(Simulation.length * Simulation.deltaTime) + "s";
        timeSlider.value = (float)Simulation.time * Simulation.deltaTime;
        timeSlider.maxValue = Simulation.length * Simulation.deltaTime;
        scaleSlider.value = 1f / Simulation.scale;
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
    }

    public void Pause()
    {
        Simulation.Pause();
    }

    public void Rewind()
    {
        int currentTime = (int) ((float)Simulation.time * Simulation.deltaTime);
        if (currentTime < 16) {
            Simulation.SetTime(0);
        } else {
            Simulation.SetTime(currentTime - 15);
        }
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
    }

    public void ScaleChanged(float newScale)
    {
        Debug.Log("scale changed");
        scaleValue.text = "1/" + Convert.ToString(newScale);

        foreach (Simulation sim in simParent.GetComponentsInChildren<Simulation>()) {
            sim.SetScale(1 / newScale);
        }
    }

    public void TimeChanged(float newTime)
    {
        Debug.Log("time changed");
        timeValue.text = Convert.ToString(newTime) + "s";
        Simulation.SetTime((int) (newTime / Simulation.deltaTime));
    }
}
