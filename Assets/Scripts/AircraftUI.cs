using System;
using UnityEngine;
using UnityEngine.UI;


public class AircraftUI : MonoBehaviour {
	// Not needed
	Simulation simulation;
	Aircraft aircraft;

	// One slider per aircraft parameter 
//	Slider caSlider;
//	Text caValue;

//	Slider cwSlider;
//	Text cwValue;

//	Slider wingAreaSlider;
//	Text wingAreaValue;

	Slider massSlider;
	Text massValue;

	Slider maxThrustSlider;
	Text maxThrustValue;

	Slider enginesSlider;
	Text enginesValue;

	// Use this for initialization
	protected void Awake () {
//		wingAreaSlider = GameObject.Find("UI/Interaction/AircraftPanel/WingArea/Slider").GetComponent<Slider>();
//		wingAreaValue = GameObject.Find ("UI/Interaction/AircraftPanel/WingArea/Value").GetComponent<Text> ();

		massSlider = GameObject.Find ("UI/Interaction/AircraftPanel/Mass/Slider").GetComponent<Slider> ();
		massValue = GameObject.Find ("UI/Interaction/AircraftPanel/Mass/Value").GetComponent<Text> ();

		maxThrustSlider = GameObject.Find ("UI/Interaction/AircraftPanel/MaxThrust/Slider").GetComponent<Slider> ();
		maxThrustValue = GameObject.Find ("UI/Interaction/AircraftPanel/MaxThrust/Value").GetComponent<Text> ();

		enginesSlider = GameObject.Find ("UI/Interaction/AircraftPanel/NumOfEngines/Slider").GetComponent<Slider> ();
		enginesValue = GameObject.Find ("UI/Interaction/AircraftPanel/NumOfEngines/Value").GetComponent<Text> ();
	}

	public void Init(Simulation simulation, Aircraft aircraft)
	{
		this.simulation = simulation;
		this.aircraft = aircraft;
	}

	// Load aircraft parameters when panel is showing up.
	public void Show()
	{
		Debug.Log("show aircraft ui");
//		wingAreaSlider.value = (float)aircraft.wingArea;
		massSlider.value = (float)aircraft.mass;
		maxThrustSlider.value = (float)aircraft.maxThrust;
		enginesSlider.value = (float)aircraft.engines;
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		Debug.Log("hide aircraft ui");
		this.gameObject.SetActive(false);
	}

	// Method SetWingArea does not exist yet.
//	public void WingAreaChanged(float newWingArea)
//	{
//		Debug.Log("wing area changed");
//		wingAreaValue.text = Convert.ToString(newWingArea) + "square meter";
//		aircraft.SetWingArea(newWingArea);
//	}

	public void MassChanged(float newMass)
	{
		Debug.Log("mass changed");
		massValue.text = Convert.ToString(newMass) + "kg";
		aircraft.SetMass(newMass);
	}

	public void MaxThrustChanged(float newMaxThrust)
	{
		Debug.Log("max thrust changed");
		maxThrustValue.text = Convert.ToString(newMaxThrust);
		aircraft.SetMaxThrust(newMaxThrust);
	}

	public void EnginesChanged(float newEngines)
	{
		Debug.Log("engines changed");
		enginesValue.text = Convert.ToString(newEngines);
		aircraft.SetEngines((int) newEngines);
	}
}