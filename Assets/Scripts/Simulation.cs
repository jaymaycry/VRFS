using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public Vector2 windVelocity;
    public List<Waypoint> waypoints;
    public AircraftHandler aircraftHandler;
    public PathHandler pathHandler;
    public int time;
    public bool play;


	// Use this for initialization
    protected void Start () {
        windVelocity = new Vector2(-0.5f, 0f);
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();

        waypoints = new List<Waypoint>();

        // register event listener
        EventManager.OnChange += Recalculate;

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
        waypoints = FlightEngine.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), windVelocity, 0.02f, 5000);
    }

    protected void FixedUpdate() {
        if (time > waypoints.Count) {
            ResetSimulator();
        }

        if (play) {
            aircraftHandler.UpdatePosition(time);
            time++;
        }
    }

    protected void ResetSimulator() {
        time = 0;
        //play = false;
    }
}
