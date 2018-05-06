using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 velocity;
    public int time;
    public Vector3 drag;
    public Vector3 lift;
    public Vector3 gravity;
    public Vector3 thrust;
    public Interaction interaction;

    public Waypoint(Vector3 position, Vector3 rotation, Vector3 velocity, Vector3 drag, Vector3 lift, Vector3 gravity, Vector3 thrust, Interaction interaction, int time) {
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
        this.drag = drag;
        this.lift = lift;
        this.gravity = gravity;
        this.thrust = thrust;
        this.time = time;
        this.interaction = interaction;
    }

    public Waypoint(Vector3 position, Vector3 rotation, Vector3 velocity, int time) : this(position, rotation, velocity, new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Interaction(0, 0, 0, false), time)
    {
    }

}