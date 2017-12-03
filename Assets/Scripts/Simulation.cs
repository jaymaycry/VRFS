using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public List<Waypoint> waypoints;
    public AircraftHandler aircraftHandler;
    public PathHandler pathHandler;
    // TODO: windhandler
    public Vector2 windVelocity;
    public static int time;
    public static int length;
    public static float deltaTime;
    public static bool play;
    public static float scale;


    // Use this for initialization
    public void Start() {
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();
        waypoints = new List<Waypoint>();
        windVelocity = new Vector2(-0.5f, 0f);
        length = 5000;
        deltaTime = 0.02f;
        scale = 0.01f;

        SetScale(scale);
        Recalculate();
    }

    public void SetScale(float newScale)
    {
        scale = newScale;
        this.transform.localScale = new Vector3(scale, scale, scale);
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
        waypoints = FlightEngine.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), windVelocity, deltaTime, length);
    }

    protected void FixedUpdate() {
        if (time > waypoints.Count) {
            ResetSimulator();
        }

        aircraftHandler.UpdatePosition(time);

        if (play) {
            time++;
        }

    }

    protected void ResetSimulator() {
        time = 0;
        //play = false;
    }

    public static void Play() {
        Simulation.play = true;
    }

    public static void Pause() {
        Simulation.play = false;
    }

    public static void SetTime(int time) {
        Simulation.time = time;
    }

    public static void SetLength(int length) {
        Simulation.length = length;
        // Recalculate(); 
    }

    public void AircraftChanged() {
        Recalculate();
    }

    public void InteractionsChanged() {
        Recalculate();
    }
}
