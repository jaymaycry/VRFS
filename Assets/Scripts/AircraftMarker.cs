using System;
using UnityEngine;
using VRTK;

public class AircraftMarker : VRTK_InteractableObject {
	AircraftUI aircraftUI;
	Aircraft aircraft;
	Simulation simulation;

	// Use this for initialization
	protected void Awake () {
		aircraftUI = GameObject.Find ("UI").GetComponent<UIHandler> ().aircraftUI;
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	public void Init(Simulation simulation, Aircraft aircraft)
	{
		this.simulation = simulation;
		this.aircraft = aircraft;
	}

	// Methods from InteractableObject for starting and stopping using objects.
	public override void StartUsing(VRTK_InteractUse usingObject)
	{
		base.StartUsing(usingObject);
		base.ToggleHighlight(true);
		aircraftUI.Init(simulation, aircraft);
		aircraftUI.Show();
	}

	public override void StopUsing(VRTK_InteractUse usingObject)
	{
		base.StopUsing(usingObject);
		base.ToggleHighlight(false);
		aircraftUI.Hide();
	}
}