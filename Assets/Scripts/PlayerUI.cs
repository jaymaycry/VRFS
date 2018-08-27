using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;


public class PlayerUI : MonoBehaviour {
    Dropdown simSelector;

    Slider scaleSlider;
    Text scaleValue;

    Slider timeSlider;
    Text timeValue;
    Text timeHigher;

    Button deleteButton;
    Button cloneButton;

    // Use this for initialization
    protected void Awake()
    {
        // todo make this better
        simSelector = GameObject.Find("UI/Player/Panel/SimActions/SelectSimulationDropdown").GetComponent<Dropdown>();

        scaleSlider = GameObject.Find("UI/Player/Panel/Scale/Slider").GetComponent<Slider>();
        scaleValue = GameObject.Find("UI/Player/Panel/Scale/Value").GetComponent<Text>();

        timeSlider = GameObject.Find("UI/Player/Panel/Time/Slider").GetComponent<Slider>();
        timeValue = GameObject.Find("UI/Player/Panel/Time/Value").GetComponent<Text>();
        timeHigher = GameObject.Find("UI/Player/Panel/Time/HigherBound").GetComponent<Text>();

        deleteButton = GameObject.Find("UI/Player/Panel/SimActions/DeleteSimulationButton").GetComponent<Button>();
        cloneButton = GameObject.Find("UI/Player/Panel/SimActions/CloneSimulationButton").GetComponent<Button>();


        EventManager.OnOpenPlayerUI += Show;
        EventManager.OnClosePlayerUI += Hide;
        EventManager.OnSimulationsChanged += SimsChanged;
    }

    protected void Start()
    {
        SimsChanged(SimulationHandler.sims);
    }

    public void FixedUpdate()
    {
        float newTime = (float)SimulationHandler.time * SimulationHandler.deltaTime;
        timeHigher.text = Convert.ToString((float)SimulationHandler.length * SimulationHandler.deltaTime) + "s";
        timeSlider.value = newTime;
        timeValue.text = Convert.ToString((int)newTime) + "s";
        timeSlider.maxValue = (float)SimulationHandler.length * SimulationHandler.deltaTime;


        float scale = Mathf.Pow(SimulationHandler.scale, -1f);
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
        int currentTime = (int) ((float)SimulationHandler.time * SimulationHandler.deltaTime);
        if (currentTime < 16) {
            EventManager.SetTime(0);
        } else {
            EventManager.SetTime(currentTime - 15);
        }
        Debug.Log("rewind button pressed.");
    }

    public void Forward()
    {
        int currentTime = (int)((float)SimulationHandler.time * SimulationHandler.deltaTime);
        int length = (int)((float)SimulationHandler.length / SimulationHandler.deltaTime);
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
        EventManager.SetScale(1f / newScale);
    }

    public void SimsChanged(List<Simulation> sims)
    {
        Debug.Log("simulations changed -> update dropdown");
        Debug.Log(sims.Count);
        deleteButton.interactable = (sims.Count > 1);
        cloneButton.interactable = (sims.Count < 5);
        int selected = 0;
        simSelector.ClearOptions();
        List<string> newOptions = new List<string>();
        for (int i = 0; i < sims.Count; i++) 
        {
            if (sims[i] == SimulationHandler.activeSim) {
                selected = i;
            }
            newOptions.Add("Simulation " + (i + 1));
        }
        simSelector.AddOptions(newOptions);
        simSelector.value = selected;
    }

    public void TimeChanged(float newTime)
    {
        Debug.Log("time slider changed");
        timeValue.text = Convert.ToString(newTime) + "s";
        EventManager.SetTime((int)(newTime / SimulationHandler.deltaTime));
    }
                                          
    public void CreateInteraction()
    {
        Debug.Log("create interaction button pressed");
        EventManager.CreateInteraction(SimulationHandler.activeSim);
    }

    public void EditAircraft()
    {
        Debug.Log("edit aircraft button pressed");
        Aircraft aircraft = SimulationHandler.activeSim.aircraftHandler.GetAircraft();
        EventManager.OpenAircraftUI(SimulationHandler.activeSim, aircraft);
    }

    public void EditEnvironment()
    {
        Debug.Log("edit environment button pressed");
        Simulation simulation = SimulationHandler.activeSim;
        EventManager.OpenEnvironmentUI(simulation);
    }

    public void SelectActiveSimulation(Dropdown dropdown)
    {
        Debug.Log("active simulation changed to " + dropdown.value);
        SimulationHandler.sims[dropdown.value].SetActive();
    }

    public void CloneSimulation()
    {
        Debug.Log("clone simulation button pressed");
        EventManager.CloneSimulation(SimulationHandler.activeSim);
    }

    public void DeleteSimulation()
    {
        Debug.Log("delete simulation button pressed");
        EventManager.RemoveSimulation(SimulationHandler.activeSim);
    }
}
