using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationHandler : MonoBehaviour {
    Simulation sim;

	// Use this for initialization
	void Awake () {
        Aircraft aircraft = new Aircraft("b787", 0.11, 0.03, 0.2, 0.6, 2, 325, 200000, 324000, 2);

        List<Interaction> interactions = new List<Interaction>();
        interactions.Add(new Interaction(0, 1, 1));
        interactions.Add(new Interaction(7, 1, 1500));

        float scale = 0.01f;
        float deltaTime = 0.02f;
        int length = 6000;
        Vector2 windVelocity = new Vector2(-0.5f, 0f);

        sim = GameObject.Find("Simulation").GetComponent<Simulation>();
        sim.SetActive();
        sim.Init(aircraft, interactions, scale, deltaTime, length, windVelocity);
	}
}
