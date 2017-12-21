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

        EventManager.OnOpenPlayerUI += Show;
        EventManager.OnClosePlayerUI += Hide;
    }

    public void Update()
    {
        timeHigher.text = Convert.ToString((float)Simulation.active.length * Simulation.active.deltaTime) + "s";
        timeSlider.value = (float)Simulation.active.time * Simulation.active.deltaTime;
        timeSlider.maxValue = (float)Simulation.active.length * Simulation.active.deltaTime;


        float scale = Mathf.Pow(Simulation.active.scale, -1f);
        scaleValue.text = "1/" + Convert.ToString(scale);
        scaleSlider.value = scale;
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
        EventManager.Play();
        Debug.Log("play button pressed.");
    }

    public void Pause()
    {
        EventManager.Pause();
        Debug.Log("pause button pressed.");
    }

    public void Rewind()
    {
        int currentTime = (int) ((float)Simulation.active.time * Simulation.active.deltaTime);
        if (currentTime < 16) {
            EventManager.SetTime(0);
        } else {
            EventManager.SetTime(currentTime - 15);
        }
        Debug.Log("rewind button pressed.");
    }

    public void Forward()
    {
        int currentTime = (int)((float)Simulation.active.time * Simulation.active.deltaTime);
        int length = (int)((float)Simulation.active.length / Simulation.active.deltaTime);
        if (currentTime < length - 15)
        {
            EventManager.SetTime(currentTime + 15);
        }
        else
        {
            EventManager.SetTime(length);
        }
        Debug.Log("forward button pressed.");
    }

    public void ScaleChanged(float newScale)
    {
        Debug.Log("scale slider changed");
        Debug.Log(newScale);
        scaleValue.text = "1/" + Convert.ToString(newScale);

        foreach (Simulation sim in simParent.GetComponentsInChildren<Simulation>()) {
            EventManager.SetScale(1f / newScale);
        }
    }

    public void TimeChanged(float newTime)
    {
        Debug.Log("time slider changed");
        timeValue.text = Convert.ToString(newTime) + "s";
        EventManager.SetTime((int) (newTime / Simulation.active.deltaTime));
    }

    public void CreateInteraction()
    {
        Debug.Log("create interaction button pressed");
        EventManager.CreateInteraction(Simulation.active);
    }
}
