using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public List<Waypoint> waypoints;
    public AircraftHandler aircraftHandler;
    public PathHandler pathHandler;
    // TODO: windhandler
    public Vector2 windVelocity;
    public int time;
    public int length;
    public bool play;


	// Use this for initialization
    public void Start () {
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();
        waypoints = new List<Waypoint>();
        windVelocity = new Vector2(-0.5f, 0f);
        length = 250;

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
        List<Waypoint> waypoints2 = FlightEngine.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), windVelocity, 0.02f, this.length);
        waypoints = FlightEngineRunge.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), new Vector2d ((double)windVelocity.x, (double)windVelocity.y), 1f, this.length);
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

    public void Play() {
        this.play = true;
    }

    public void Pause() {
        this.play = false;
    }

    public void SetTime(int time) {
        this.time = time;
        UpdateAircraftHandler();
    }

    public void SetLength(int length) {
        this.length = length;
        Recalculate();
    }
}
