using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftHandler : MonoBehaviour {
    protected Aircraft aircraft;
    protected List<Waypoint> waypoints;
    protected int time;

    // aircraft models
    protected GameObject smallAircraft;

    public void Start() {
        smallAircraft = GameObject.Find("C");
        aircraft = new Aircraft("A380", 0.11, 0.05, 0.45, 0.6, 3, 846, 492000, 311000, 4, smallAircraft);
    }

    // returns the active aircraft
    // TODO add other aircrafts as well
    public Aircraft GetAircraft() {
        return aircraft;
    }

    public void SetWaypoints(List<Waypoint> waypoints) {
        this.waypoints = waypoints;
        UpdatePosition(time);
    }

    protected void Reposition(Vector3 position, Vector3 rotation) {
        position.z = position.z / 2f;
        transform.localPosition = position;
        transform.rotation = Quaternion.Euler(rotation);
    }

    public void UpdatePosition(int time) {
        this.time = time;
        Waypoint prev = new Waypoint(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), 0);
        Waypoint next = null;

        waypoints.ForEach(delegate(Waypoint waypoint) {
            if (waypoint.time <= time) {
                prev = waypoint;
            }
            else if (waypoint.time > time && next == null){
                next = waypoint;
            }
        });

        Vector3 newPosition = prev.position;

        if (prev != null && next != null) {
            float deltaTime = next.time - prev.time;
            Vector3 deltaPosition = next.position - prev.position;
            Vector3 segment = deltaPosition / deltaTime;

            newPosition = prev.position + ((time - prev.time) * segment);
        }

        Vector3 newRotation = prev.rotation;
        Reposition(newPosition, newRotation);
    }
}