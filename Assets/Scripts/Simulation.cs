using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour {
    public static Simulation active;

    public List<Waypoint> waypoints;
    public AircraftHandler aircraftHandler;
    public PathHandler pathHandler;
    // TODO: windhandler
    public Vector2 windVelocity;
    public int time = 0;
    public int length = 5000;
    public float deltaTime = 0.02f;
    public bool play = false;
    public float scale = 0.01f;


    // Use this for initialization
    public void Awake() {
        aircraftHandler = GetComponentInChildren<AircraftHandler>();
        pathHandler = GetComponentInChildren<PathHandler>();
        waypoints = new List<Waypoint>();

        EventManager.OnChange += OnChange;
        EventManager.OnPlay += Play;
        EventManager.OnPause += Pause;
        EventManager.OnSetTime += SetTime;
        EventManager.OnSetScale += SetScale;
        EventManager.OnCreateInteraction += CreateInteraction;
    }

    public void Init(Aircraft aircraft, List<Interaction> interactions, float scale, float deltaTime, int length, Vector2 windVelocity)
    {
        aircraftHandler.SetAircraft(aircraft);
        pathHandler.SetInteractions(interactions);
        SetScale(scale);
        SetDeltaTime(deltaTime);
        SetLength(length);
        this.windVelocity = windVelocity;

        Recalculate();
    }

    protected void CreateInteraction(Simulation sim)
    {
        if (sim && sim == this)
        {
            Interaction interaction = new Interaction(0d, 0d, 0, true);
            pathHandler.AddInteraction(interaction);
            EventManager.OpenInteractionUI(this, interaction);
        }
    }

    protected void OnChange(Simulation sim)
    {
        if (sim == null || sim == this)
            Recalculate();
    }


    protected void SetScale(float newScale)
    {
        scale = newScale;
        this.transform.localScale = new Vector3(scale, scale, scale);
        Debug.Log("Scale set");
    }

    protected void Recalculate() {
        CalculateWaypoints();
        UpdatePathHandler();
        UpdateAircraftHandler();
    }

    protected void UpdatePathHandler() {
        pathHandler.SetWaypoints(waypoints);
    }

    protected void UpdateAircraftHandler() {
        aircraftHandler.SetWaypoints(waypoints);
    }

    protected void CalculateWaypoints() {
        // waypoints = FlightEngine.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), windVelocity, deltaTime, length);
        waypoints = FlightEngine.CalculateWaypoints(aircraftHandler.GetAircraft(), pathHandler.GetInteractions(), windVelocity, deltaTime, length);
    }

    protected void FixedUpdate() {
        if (time > waypoints.Count) {
            ResetSimulator();
        }

        aircraftHandler.UpdatePosition(time);

        if (play) {
            time++;
        }

    }

    protected void ResetSimulator() {
        time = 0;
        //play = false;
    }

    protected void Play() {
        this.play = true;
    }

    protected void Pause() {
        this.play = false;
    }

    protected void SetTime(int time) {
        this.time = time;
    }

    protected void SetLength(int length) {
        this.length = length;
        // Recalculate(); 
    }

    protected void SetDeltaTime(float deltaTime)
    {
        this.deltaTime = deltaTime;
    }

    public void SetActive()
    {
        Simulation.active = this;
    }
}
