using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftHandler : MonoBehaviour {
    Simulation sim;
    protected Aircraft aircraft;
    protected List<Waypoint> waypoints;
    protected int time;

    // aircraft models
    protected GameObject smallAircraft;

    public void Start() {
        sim = this.transform.parent.GetComponent<Simulation>();
        smallAircraft = GameObject.Find("AircraftHandler/Cessna");
        aircraft = new Aircraft("A380", 0.492, 0.01523, 846, 492000, 311000, 4);

        ShowModel(smallAircraft);
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

    public void SetCW(double cW0)
    {
        this.aircraft.cW0 = cW0;
        sim.AircraftChanged();
    }

    public void SetCA(double cA0)
    {
        this.aircraft.cA0 = cA0;
        sim.AircraftChanged();
    }

    public void SetMass(double mass)
    {
        this.aircraft.mass = mass;
        sim.AircraftChanged();
    }

    public void SetMaxThrust(double maxThrust)
    {
        this.aircraft.maxThrust = maxThrust;
        sim.AircraftChanged();
    }

    public void SetEngines(int engines)
    {
        this.aircraft.engines = engines;
        sim.AircraftChanged();
    }

    public void ShowModel(GameObject model)
    {
        MeshRenderer render = model.GetComponentInChildren<MeshRenderer>();
        render.enabled = true;
    }

    public void HideModel(GameObject model)
    {
        MeshRenderer render = model.GetComponentInChildren<MeshRenderer>();
        render.enabled = false;
    }
}