using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint
{
    public static string separator = ";";
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 velocity;
    public int time;
    public Vector3 drag;
    public Vector3 lift;
    public Vector3 gravity;
    public Vector3 thrust;
    public double gamma;
    public double angleOfAttack;
    public double power;
    public double pitch;
    public double ca;
    public double cw;

    public Waypoint(
        Vector3 position,
        Vector3 rotation,
        Vector3 velocity,
        Vector3 drag,
        Vector3 lift,
        Vector3 gravity,
        Vector3 thrust,
        double gamma,
        double angleOfAttack,
        double power,
        double pitch,
        double ca,
        double cw,
        int time)
    {
        this.time = time;
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
        this.drag = drag;
        this.lift = lift;
        this.gravity = gravity;
        this.thrust = thrust;
        this.gamma = gamma;
        this.angleOfAttack = angleOfAttack;
        this.power = power;
        this.pitch = pitch;
        this.ca = ca;
        this.cw = cw;
    }

    public Waypoint(
        Vector3 position,
        Vector3 rotation,
        Vector3 velocity,
        int time
    ) : this(
        position,
        rotation,
        velocity,
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 0f, 0f),
        0d,
        0d,
        0d,
        0d,
        0d,
        0d,
        time
    )
    {
    }

    public Waypoint(
        Vector3 position,
        Vector3 rotation,
        Vector3 velocity,
        Vector3 drag,
        Vector3 lift,
        Vector3 gravity,
        Vector3 thrust,
        int time
    ) : this(
        position,
        rotation,
        velocity,
        drag,
        lift,
        gravity,
        thrust,
        0d,
        0d,
        0d,
        0d,
        0d,
        0d,
        time
    )
    {
    }

    public string printData() {
        string[] values = {
            Convert.ToString(SimulationHandler.deltaTime),
            Convert.ToString(this.position.z),
            Convert.ToString(this.position.y),
            Convert.ToString(this.velocity.z),
            Convert.ToString(this.velocity.y),
            Convert.ToString(this.drag.z),
            Convert.ToString(this.drag.y),
            Convert.ToString(this.lift.z),
            Convert.ToString(this.lift.y),
            Convert.ToString(this.gravity.z),
            Convert.ToString(this.gravity.y),
            Convert.ToString(this.thrust.z),
            Convert.ToString(this.thrust.y),
            Convert.ToString(this.pitch),
            Convert.ToString(this.gamma),
            Convert.ToString(this.angleOfAttack),
            Convert.ToString(this.power),
            Convert.ToString(this.ca),
            Convert.ToString(this.cw),
        };
        return string.Join(separator, values);
    }

    public static string printHeader() {
        string[] headers = {
            "time (" + Convert.ToString(SimulationHandler.deltaTime) + "s)",
            "position x (m)",
            "position y (m)",
            "velocity x (m/s)",
            "velocity y (m/s)",
            "drag x (N)",
            "drag y (N)",
            "lift x (N)",
            "lift y (N)",
            "gravity x (N)",
            "gravity y (N)",
            "thrust x (N)",
            "thrust y (N)",
            "pitch (deg)",
            "gamma (deg)",
            "angleOfAttack (deg)",
            "thrust (%)",
            "ca",
            "cw"
        };
        return string.Join(separator, headers);
    }

}