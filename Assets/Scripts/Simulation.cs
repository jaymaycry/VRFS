using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public List<Waypoint> waypoints;
    public AircraftHandler aircraftHandler;
    public PathHandler pathHandler;
    // TODO: windhandler
    public Vector2 windVelocity;
    public double metersAboveSeaLevel = 0d;
    public double temperature = 20d;
    public int time = 0;
    public bool play = false;
    public Color color;




    // Use this for initialization
    public void Awake() {
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();
        waypoints = new List<Waypoint>();

        EventManager.OnChange += OnChange;
        EventManager.OnCreateInteraction += CreateInteraction;
    }

    public void Init(Aircraft aircraft, List<Interaction> interactions, Vector2 windVelocity, double metersAboveSeaLevel, double temperature, Color color)
    {
        this.color = color;
        this.temperature = temperature;
        this.metersAboveSeaLevel = metersAboveSeaLevel;
        this.windVelocity = windVelocity;
        aircraftHandler.SetAircraft(aircraft);
        pathHandler.SetInteractions(interactions);

        Recalculate();
    }

    protected void CreateInteraction(Simulation sim)
    {
        if (sim && sim == this)
        {
            Interaction interaction = new Interaction(0d, 0d, 0, true);
            pathHandler.AddInteraction(interaction);
            EventManager.OpenInteractionUI(this, interaction);
        }
    }

    protected void OnChange(Simulation sim)
    {
        if (sim == null || sim == this)
            Recalculate();
    }

    protected void Recalculate() {
        CalculateWaypoints();
        UpdatePathHandler();
        UpdateAircraftHandler();
    }

    protected void UpdatePathHandler() {
        pathHandler.SetWaypoints(waypoints);
    }

    protected void UpdateAircraftHandler() {
        aircraftHandler.SetWaypoints(waypoints);
    }

    protected void CalculateWaypoints() {
        waypoints = FlightEngine.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), windVelocity, metersAboveSeaLevel, temperature, SimulationHandler.deltaTime, SimulationHandler.length);
    }

    public void UpdateSimulation() {
        aircraftHandler.UpdatePosition(SimulationHandler.time);
    }

    public void SetActive()
    {
        Debug.Log("set active sim");
        SimulationHandler.activeSim = this;
    }
}
