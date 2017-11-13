using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public FlightEngine engine;
    public List<Waypoint> waypoints;
    public AircraftHandler aircraftHandler;
    public PathHandler pathHandler;
    public Waypoint current;
    public Waypoint next;
    public float time;
    public int item;


	// Use this for initialization
	void Start () {
        
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();

        engine = new FlightEngine(aircraftHandler.getAircraft());
        waypoints = engine.CalculateWaypoints(3000, 0.02f);

        updatePath();
        resetSimulator();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void updatePath() {
        pathHandler.updateWaypoints(waypoints.ToArray());
    }

    private void resetSimulator() {
		current = waypoints[0];
		next = waypoints[1];
        time = 0;
        item = 1;
    }

    private void FixedUpdate() {
        if (time >= next.time) {
            item++;
            if (item >= waypoints.Count) {
                resetSimulator();
            } else {
				current = next;
				next = waypoints[item];
            }
        }

        float deltaTime = next.time - current.time;
        Vector3 deltaPosition = next.position - current.position;
        Vector3 segment = deltaPosition / deltaTime;

        Vector3 newPosition = current.position + ((time - current.time) * segment);
        Vector3 newRotation = current.rotation;
        aircraftHandler.reposition(newPosition, newRotation);

        time++;
    }
}
