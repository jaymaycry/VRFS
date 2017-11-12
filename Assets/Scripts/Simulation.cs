using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public FlightEngine engine;
    public List<Waypoint> waypoints;
    public Airplane airplane;
    public Path path;
    public Waypoint current;
    public Waypoint next;
    public float time;
    public int item;


	// Use this for initialization
	void Start () {
        /* waypoints.Add(new Waypoint(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0));
        waypoints.Add(new Waypoint(new Vector3(0, 0, 30), new Vector3(0, 0, 0), 300));
        waypoints.Add(new Waypoint(new Vector3(0, 0, 70), new Vector3(-1, 0, 0), 600));
        waypoints.Add(new Waypoint(new Vector3(0, 0, 71), new Vector3(-3, 0, 0), 601));
        waypoints.Add(new Waypoint(new Vector3(0, 0, 72), new Vector3(-7, 0, 0), 602));
        waypoints.Add(new Waypoint(new Vector3(0, 0, 73), new Vector3(-12, 0, 0), 603));
        waypoints.Add(new Waypoint(new Vector3(0, 0, 74), new Vector3(-15, 0, 0), 604));
        waypoints.Add(new Waypoint(new Vector3(0, 20, 130), new Vector3(0, 0, 0), 800));*/

        engine = new FlightEngine();

        waypoints = engine.CalculateWaypoints(3000, 0.02f);

        airplane = GetComponentInChildren<Airplane>();
        path = GetComponentInChildren<Path>();
        updatePath();
        resetSimulator();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void updatePath() {
        path.updateWaypoints(waypoints.ToArray());
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
        airplane.reposition(newPosition, newRotation);

        time++;
    }
}
