﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft {
    public string name;
    public double cW0;
    public double cA0;
    // wing area in square meter
    public double wingArea;
    // mass in kg
    public double mass;
    // max thrust per engine
    public double maxThrust;
    // amount of engines
    public int engines;
    // linked model
    public GameObject model;

    public Aircraft(string name,
                    double cW0,
                    double cA0,
                    double wingArea,
                    double mass,
                    double maxThrust,
                    int engines,
                    GameObject model) {
        this.name = name;
        this.cW0 = cW0;
        this.cA0 = cA0;
        this.wingArea = wingArea;
        this.mass = mass;
        this.maxThrust = maxThrust;
        this.engines = engines;
        this.model = model;
    }

    public void ShowModel() {
        MeshRenderer render = model.GetComponentInChildren<MeshRenderer>();
        render.enabled = true;
    }

    public void HideModel() {
        MeshRenderer render = model.GetComponentInChildren<MeshRenderer>();
        render.enabled = false;
    }

    // TODO calcCA for other plane types - subclasses
    public double CalcCA(double angleOfAttack) {
        return 0.0549 * angleOfAttack + cA0;
    }

    // TODO calcCW for other plane types - subclasses
    public double CalcCW(double angleOfAttack) {
        return cW0 + 0.017 * Mathf.Pow((float)(CalcCA(angleOfAttack) - cA0), 2);
    }
}
