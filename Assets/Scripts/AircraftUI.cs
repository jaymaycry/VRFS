using System;


public class AircraftUI : MonoBehavior {
	Simulation simulation;
	Aircraft aircraft;

	// One slider per aircraft parameter 
	Slider caSlider;
	Text caValue;

	Slider cwSlider;
	Text cwValue;

	Slider wingAreaSlider;
	Text wingAreaValue;

	Slider massSlider;
	Text massValue;

	Slider maxThrustSlider;
	Text maxThrustValue;

	Slider enginesSlider;
	Text enginesValue;

}