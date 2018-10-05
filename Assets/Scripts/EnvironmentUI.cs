using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentUI : MonoBehaviour {
    Simulation sim;

    Slider temperatureSlider;
    Text temperatureValue;

    Slider heightSlider;
    Text heightValue;

    Slider windVelocityXSlider;
    Text windVelocityXValue;

    Slider windVelocityYSlider;
    Text windVelocityYValue;

    private void Awake()
    {
        temperatureSlider = GameObject.Find("UI/Environment/Panel/Temperature/Slider").GetComponent<Slider>();
        temperatureValue = GameObject.Find("UI/Environment/Panel/Temperature/Value").GetComponent<Text>();

        heightSlider = GameObject.Find("UI/Environment/Panel/Height/Slider").GetComponent<Slider>();
        heightValue = GameObject.Find("UI/Environment/Panel/Height/Value").GetComponent<Text>();

        windVelocityXSlider = GameObject.Find("UI/Environment/Panel/WindVelocityX/Slider").GetComponent<Slider>();
        windVelocityXValue = GameObject.Find("UI/Environment/Panel/WindVelocityX/Value").GetComponent<Text>();

        windVelocityYSlider = GameObject.Find("UI/Environment/Panel/WindVelocityY/Slider").GetComponent<Slider>();
        windVelocityYValue = GameObject.Find("UI/Environment/Panel/WindVelocityY/Value").GetComponent<Text>();

        EventManager.OnOpenEnvironmentUI += Open;
        EventManager.OnCloseEnvironmentUI += Close;
    }

    protected void Init()
    {
        temperatureSlider.value = (float)sim.temperature;
        heightSlider.value = (float)sim.metersAboveSeaLevel;

        windVelocityXSlider.value = sim.windVelocity.x;
        windVelocityYSlider.value = sim.windVelocity.y;

        UpdateValue();
    }

    protected void UpdateValue()
    {
        temperatureValue.text = Convert.ToString(sim.temperature) + "°C";
        heightValue.text = Convert.ToString(sim.metersAboveSeaLevel) + "MAMSL";
        windVelocityXValue.text = Convert.ToString(sim.windVelocity.x) + "m/s";
        windVelocityYValue.text = Convert.ToString(sim.windVelocity.y) + "m/s";
    }

    public void Open(Simulation sim) {
        this.sim = sim;
        Show();
    }

    public void TriggerClose()
    {
        EventManager.CloseEnvironmentUI();
    }

    public void Close()
    {
        Hide();
    }

    public void Show()
    {
        Debug.Log("show environment ui");
        Init();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("hide environment ui");
        this.gameObject.SetActive(false);
    }

    public void TemperatureChanged(float newTemperature) {
        Debug.Log("Temperature changed");
        sim.temperature = (double)newTemperature;
        UpdateValue();
        EventManager.EnvironmentChanged(sim);
    }

    public void HeightChanged(float newHeight) {
        Debug.Log("Height changed");
        sim.metersAboveSeaLevel = (double)newHeight;
        UpdateValue();
        EventManager.EnvironmentChanged(sim);
    }

    public void WindVelocityXChanged(float newWindVelocityX) {
        Debug.Log("Windvelocity X changed");
        sim.windVelocity.x = newWindVelocityX;
        UpdateValue();
        EventManager.EnvironmentChanged(sim);
    }

    public void WindVelocityYChanged(float newWindVelocityY)
    {
        Debug.Log("Windvelocity Y changed");
        sim.windVelocity.y = newWindVelocityY;
        UpdateValue();
        EventManager.EnvironmentChanged(sim);
    }
}
