using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction {
    public bool edit;
    public double pitch;
    public double thrust;
    public int time;

    public Interaction(double pitch, double thrust, int time, bool edit)
    {
        this.pitch = pitch;
        this.thrust = thrust;
        this.time = time;
        this.edit = edit;
    }

    public Interaction(double pitch, double thrust, int time): this(pitch, thrust, time, false)
    {
    }
}