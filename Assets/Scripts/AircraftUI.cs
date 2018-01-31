using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class AircraftUI : MonoBehaviour {

    Simulation sim;
    Aircraft aircraft;

    Slider enginesSlider;
    Text enginesValue;

    Slider thrustSlider;
    Text thrustValue;

    Slider wingAreaSlider;
    Text wingAreaValue;

    Slider massSlider;
    Text massValue;

    protected void Awake() {
        enginesSlider = GameObject.Find("UI/Aircraft/Panel/Engines/Slider").GetComponent<Slider>();
        enginesValue = GameObject.Find("UI/Aircraft/Panel/Engines/Value").GetComponent<Text>();

        thrustSlider = GameObject.Find("UI/Aircraft/Panel/Thrust/Slider").GetComponent<Slider>();
        thrustValue = GameObject.Find("UI/Aircraft/Panel/Thrust/Value").GetComponent<Text>();

        wingAreaSlider = GameObject.Find("UI/Aircraft/Panel/WingArea/Slider").GetComponent<Slider>();
        wingAreaValue = GameObject.Find("UI/Aircraft/Panel/WingArea/Value").GetComponent<Text>();

        massSlider = GameObject.Find("UI/Aircraft/Panel/Mass/Slider").GetComponent<Slider>();
        massValue = GameObject.Find("UI/Aircraft/Panel/Mass/Value").GetComponent<Text>();

        EventManager.OnOpenAircraftUI += Open;
        EventManager.OnCloseAircraftUI += Close;
    }

    protected  void Init()
    {
        //enginesSlider.value = (float)aircraft.engines;
        thrustSlider.value = (float)(aircraft.maxThrust/1000);
        wingAreaSlider.value = (float)aircraft.wingArea;
        massSlider.value = (float)aircraft.mass;

        UpdateValues();
    }

    protected void UpdateValues()
    {
        //enginesValue.text = Convert.ToString(aircraft.engines);
        thrustValue.text = Convert.ToString((int)(aircraft.maxThrust / 1000d)) + "kN";
        wingAreaValue.text = Convert.ToString((int)aircraft.wingArea) + "m2";
        massValue.text = Convert.ToString((int)(aircraft.mass / 1000d)) + "t";
    }

    public void Open(Simulation sim, Aircraft aircraft)
    {
        this.aircraft = aircraft;
        this.sim = sim;
        Show();
    }

    public void Close()
    {
        Hide();
    }

    public void Show()
    {
        Debug.Log("show aircraft ui");
        Init();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("hide aircraft ui");
        this.gameObject.SetActive(false);
    }

    public void TriggerClose()
    {
        EventManager.CloseAircraftUI();
    }

    public void EnginesChanged(float newAmount)
    {
        Debug.Log(newAmount);
        aircraft.engines = (int)newAmount;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void ThrustChanged(float newThrust)
    {
        aircraft.maxThrust = (int)newThrust * 1000;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void WingAreaChanged(float newWingArea)
    {
        aircraft.wingArea = (int)newWingArea;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void MassChanged(float newMass)
    {
        aircraft.mass = (int)newMass;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void EditPolarCurve()
    {
        Debug.Log("edit polar curve button pressed");
        EventManager.OpenPolarCurveUI(sim, aircraft);
    }
}
