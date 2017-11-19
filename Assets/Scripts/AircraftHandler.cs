using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftHandler : MonoBehaviour {
    Aircraft aircraft;

    public GameObject smallAircraft;

    void Start() {
        smallAircraft = GameObject.Find("Small Passenger Plane");
        aircraft = new Aircraft("A380", 0.492, 0.01523, 846, 492000, 311000, 4, smallAircraft);
    }

    // returns the active aircraft
    // TODO add other aircrafts as well
    public Aircraft GetAircraft() {
        return aircraft;
    }

    public void Reposition(Vector3 position, Vector3 rotation) {
        position.z = position.z / 2f;
        transform.localPosition = position;
        transform.rotation = Quaternion.Euler(rotation);
    }
}