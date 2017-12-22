﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationHandler : MonoBehaviour {
    Simulation sim;

	// Use this for initialization
	void Awake () {
        Aircraft aircraft = new Aircraft("A380", 0.11, 0.05, 0.45, 0.6, 3, 846, 492000, 311000, 4);

        List<Interaction> interactions = new List<Interaction>();
        interactions.Add(new Interaction(0, 1, 1));
        interactions.Add(new Interaction(15, 1, 2000));

        float scale = 0.01f;
        float deltaTime = 0.02f;
        int length = 5000;
        Vector2 windVelocity = new Vector2(-0.5f, 0f);

        sim = GameObject.Find("Simulation").GetComponent<Simulation>();
        sim.SetActive();
        sim.Init(aircraft, interactions, scale, deltaTime, length, windVelocity);
	}
}
