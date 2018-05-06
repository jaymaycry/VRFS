using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction {
    public bool edit;
    public double pitch;
    public double thrust;
    public int time;
    public int smoothSteps;

    public Interaction(double pitch, double thrust, int time, int smoothSteps, bool edit)
    {
        this.pitch = pitch;
        this.thrust = thrust;
        this.time = time;
        this.edit = edit;
        this.smoothSteps = smoothSteps;
    }

    public Interaction(double pitch, double thrust, int time): this(pitch, thrust, time, 50, false)
    {
    }

    public Interaction(double pitch, double thrust, int time, bool edit): this(pitch, thrust, time, 50, edit)
    {
    }

    public Interaction Clone()
    {
        return (Interaction)this.MemberwiseClone();
    }
}