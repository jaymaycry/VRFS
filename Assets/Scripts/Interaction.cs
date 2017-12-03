﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction {
    public GameObject marker;
    public double pitch;
    public double thrust;
    public int time;

    public Interaction(double pitch, double thrust, int time) {
        this.pitch = pitch;
        this.thrust = thrust;
        this.time = time;
    }
}