using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationHandler : MonoBehaviour {
    public static List<Simulation> sims = new List<Simulation>();
    public static Simulation activeSim;
    public static float scale = 0.01f;
    public static int length = 6000;
    public static float deltaTime = 0.02f;
    public static int time = 0;
    public static bool play = false;

    public static Random random = new Random();
    public static List<Color> colors = new List<Color>();

    private void Awake()
    {
        colors.Add(new Color(1f, 0.6827586f, 0f));
        colors.Add(Color.blue);
        colors.Add(Color.green);
        colors.Add(Color.red);
        colors.Add(Color.magenta);

        EventManager.OnPlay += Play;
        EventManager.OnPause += Pause;
        EventManager.OnSetTime += SetTime;
        EventManager.OnSetScale += SetScale;
        EventManager.OnCloneSimulation += CloneSim;
        EventManager.OnRemoveSimulation += RemoveSim;
    }

    // Use this for initialization
    void Start () {
        // create default values for first simulation
        Aircraft aircraft = new Aircraft("b787", 0.11, 0.05, 0.45, 0.6, 3, 377, 200000, 340000, 2);
        List<Interaction> interactions = new List<Interaction>();
        interactions.Add(new Interaction(0, 1, 1));
        interactions.Add(new Interaction(7, 1, 1500));
        Vector2 windVelocity = new Vector2(0f, 0f);

        // scale the sim handler
        SetScale(scale);

        // create sim instance
        Simulation sim = ((GameObject)Instantiate(Resources.Load("Simulation"), new Vector3(0f,0f,0f), Quaternion.Euler(0f, 0f, 0f), this.transform)).GetComponent<Simulation>();
        sim.name = "Simulation 1";
        sims.Add(sim);
        sim.SetActive();
        sim.Init(aircraft, interactions, windVelocity, GetColor());
	}

    protected void FixedUpdate()
    {
        if (play)
        {
            time = ++time % length;
            sims.ForEach(sim => sim.UpdateSimulation());
        }
    }

    protected void SimsChanged()
    {
        for (int i = 0; i < sims.Count; i++)
        {
            sims[i].transform.localPosition = new Vector3(i * 100f, 0f, 0f);
            sims[i].transform.name = "Simulation " + (i + 1);
        }
        EventManager.SimulationsChanged(sims);
    }

    protected void CloneSim(Simulation sim)
    {
        Debug.Log("Clone " + sim.name);
        Aircraft aircraftClone = sim.aircraftHandler.GetAircraft().Clone();

        List<Interaction> interactionClones = new List<Interaction>();
        sim.pathHandler.GetInteractions().ForEach((interaction) => interactionClones.Add(interaction.Clone()));

        Vector2 windVelocity = new Vector2(-0.5f, 0f);

        Simulation clone = ((GameObject)Instantiate(Resources.Load("Simulation"), new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f), this.transform)).GetComponent<Simulation>();
        sims.Add(clone);
        clone.SetActive();
        clone.Init(aircraftClone, interactionClones, windVelocity, GetColor());

        SimsChanged();
    }

    protected void RemoveSim(Simulation sim)
    {
        sims.Remove(sim);
        sims[sims.Count - 1].SetActive();
        colors.Add(sim.color);
        Destroy(sim.gameObject);
        SimsChanged();
    }

    protected void SetTime(int newTime)
    {
        time = newTime;
    }

    protected void Play()
    {
        play = true;
    }

    protected void Pause()
    {
        play = false;
    }

    protected void SetScale(float newScale)
    {
        scale = newScale;
        this.transform.localScale = new Vector3(scale, scale, scale);
        Debug.Log("Scale set");
        //sims.ForEach((sim) => sim.scale = newScale);
    }

    protected Color GetColor()
    {
        if (colors.Count > 0)
        {
            Color newColor = colors[Random.Range(0, colors.Count - 1)];
            colors.Remove(newColor);
            return newColor;
        }
        return Color.grey;
    }
}
