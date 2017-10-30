using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint
{
    public Vector3 position;
    public Vector3 rotation;
    public float time;

    public Waypoint(Vector3 position, Vector3 rotation, float time) {
        this.position = position;
        this.rotation = rotation;
        this.time = time;
    }
}