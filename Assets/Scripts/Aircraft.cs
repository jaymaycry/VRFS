using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircraft {
    public string name;
    public double cW0;
    public double cW1;
    public double cW2;
    public double cA0;
    public double cA1;
    // wing area in square meter
    public double wingArea;
    // mass in kg
    public double mass;
    // max thrust per engine
    public double maxThrust;
    // amount of engines
    public int engines;

    public Aircraft(string name,
                    double cW0,
                    double cW1,
                    double cW2,
                    double cA0,
                    double cA1,
                    double wingArea,
                    double mass,
                    double maxThrust,
                    int engines)
    {
        this.name = name;
        this.cW0 = cW0;
        this.cW1 = cW1;
        this.cW2 = cW2;
        this.cA0 = cA0;
        this.cA1 = cA1;
        this.wingArea = wingArea;
        this.mass = mass;
        this.maxThrust = maxThrust;
        this.engines = engines;
    }

    public double CalcCA(double angleOfAttack)
    {
        //return 0.0549 * angleOfAttack + cA0;
        return cA1 * angleOfAttack + cA0;
    }

    public double CalcCW(double angleOfAttack)
    {
        // return cW0 + 0.017 * Mathf.Pow((float)(CalcCA(angleOfAttack) - cA0), 2);
        return cW0 + cW1 * angleOfAttack + cW2 * Mathd.Pow(angleOfAttack, 2);
    }

    public double MockCA(double angleOfAttack)
    {
        return cA0;
    }

    public double MockCW(double angleOfAttack)
    {
        return cW0;
    }
}
