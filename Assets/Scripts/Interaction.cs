using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction {
    public double pitch;
    public double thrust;
    public float time;

    public Interaction(double pitch, double thrust, float time) {
        this.pitch = pitch;
        this.thrust = thrust;
        this.time = time;
    }
}