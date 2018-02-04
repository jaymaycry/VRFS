using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationHandler : MonoBehaviour {
    List<Simulation> sims;
    public static Simulation activeSim;
    public static float scale = 0.01f;
    public static int length = 6000;
    public static float deltaTime = 0.02f;

    private void Awake()
    {
        sims = new List<Simulation>();
        EventManager.OnSetScale += SetScale;
    }

    // Use this for initialization
    void Start () {
        // create default values for first simulation
        Aircraft aircraft = new Aircraft("b787", 0.11, 0.03, 0.2, 0.6, 2, 325, 200000, 324000, 2);

        List<Interaction> interactions = new List<Interaction>();
        interactions.Add(new Interaction(0, 1, 1));
        interactions.Add(new Interaction(7, 1, 1500));

        Vector2 windVelocity = new Vector2(-0.5f, 0f);

        SetScale(scale);

        Simulation sim = ((GameObject)Instantiate(Resources.Load("Simulation"), new Vector3(0f,0f,0f), Quaternion.Euler(0f, 0f, 0f), this.transform)).GetComponent<Simulation>();
        sims.Add(sim);
        sim.SetActive();
        sim.Init(aircraft, interactions, windVelocity);

        this.Clone(sim);
	}

    void Clone(Simulation sim) {
        Aircraft aircraftClone = sim.aircraftHandler.GetAircraft().Clone();

        List<Interaction> interactionClones = new List<Interaction>();
        sim.pathHandler.GetInteractions().ForEach((interaction) => interactionClones.Add(interaction.Clone()));

        Vector2 windVelocity = new Vector2(-0.5f, 0f);

        Simulation clone = ((GameObject)Instantiate(Resources.Load("Simulation"), new Vector3(sims.Count * 1f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f), this.transform)).GetComponent<Simulation>();
        sims.Add(clone);
        clone.SetActive();
        clone.Init(aircraftClone, interactionClones, windVelocity);
    }

    protected void SetScale(float newScale)
    {
        scale = newScale;
        this.transform.localScale = new Vector3(scale, scale, scale);
        Debug.Log("Scale set");
        //sims.ForEach((sim) => sim.scale = newScale);
    }
}
