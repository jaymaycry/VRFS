using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public List<Waypoint> waypoints;
    public AircraftHandler aircraftHandler;
    public PathHandler pathHandler;
    // TODO: windhandler
    public Vector2 windVelocity;
    public static int time = 0;
    public static int length = 5000;
    public static float deltaTime = 0.02f;
    public static bool play = false;
    public static float scale = 0.01f;


    // Use this for initialization
    public void Start() {
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();
        waypoints = new List<Waypoint>();
        windVelocity = new Vector2(-0.5f, 0f);

        SetScale(scale);
        Recalculate();
    }

    public void SetScale(float newScale)
    {
        scale = newScale;
        this.transform.localScale = new Vector3(scale, scale, scale);
        Debug.Log("Scale set");
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
