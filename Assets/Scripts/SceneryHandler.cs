using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneryHandler : MonoBehaviour {

    public Text simName;
    public Text simLocation;
    public Text simWind;
    Simulation parentSim;

    // Use this for initialization
    void Start() {
        parentSim = this.transform.parent.GetComponent<Simulation>();
        UpdateView();

        EventManager.OnSimulationsChanged += SimsChanged;
        EventManager.OnChange += EnvChanged;
    }

    // NO AWAKE! use manual references cause of duplicate simulations in env

    public string GenerateLocationText(double height, double temperature)
    {
        return "Temperature: " + Convert.ToString(temperature) + "°, Height: " + Convert.ToString(height) + "m";
    }

    public string GenerateWindText(Vector2 windVelocity)
    {
        return "Wind speed (x,y): " + Convert.ToString(windVelocity.x) + "m/s, " + Convert.ToString(windVelocity.y) + "m/s";
    }
    public void SimsChanged(List<Simulation> sims)
    {
        UpdateView();
    }

    public void EnvChanged(Simulation sim)
    {
        UpdateView();
    }

    public void UpdateView()
    {
        simName.text = parentSim.name;
        simLocation.text = GenerateLocationText(parentSim.metersAboveSeaLevel, parentSim.temperature);
        simWind.text = GenerateWindText(parentSim.windVelocity);
    }
}
