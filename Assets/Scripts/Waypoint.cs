using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    public Vector3 position;
    public Vector3 rotation;
    public float time;
    public Vector3 drag;
    public Vector3 lift;
    public Vector3 gravity;
    public Vector3 thrust;

    public Waypoint(Vector3 position, Vector3 rotation, Vector3 drag, Vector3 lift, Vector3 gravity, Vector3 thrust, float time) {
        this.position = position;
        this.rotation = rotation;
        this.drag = drag;
        this.lift = lift;
        this.gravity = gravity;
        this.thrust = thrust;
        this.time = time;
    }

    public Waypoint(Vector3 position, Vector3 rotation, float time): this(position, rotation, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), time)
    {
    }

}