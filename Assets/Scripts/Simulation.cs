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
    public float deltaTime;
    public bool play;
    public float scale;


    // Use this for initialization
    public void Start() {
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();
        waypoints = new List<Waypoint>();
        windVelocity = new Vector2(-0.5f, 0f);
        length = 5000;
        deltaTime = 0.02f;
        scale = 0.01f;

        // register event listener
        EventManager.OnChange += Recalculate;

        Recalculate();
    }


    protected void AdjustScale()
    {
        this.transform.localScale = new Vector3(scale, scale, scale);
    }

    protected void Recalculate() {
        AdjustScale();
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
        waypoints = FlightEngine.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), windVelocity, deltaTime, length);
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
