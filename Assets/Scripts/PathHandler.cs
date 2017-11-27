using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandler : MonoBehaviour {
    Simulation sim;
    LineRenderer lineRenderer;
    List<Waypoint> waypoints;
    List<Interaction> interactions;

	// Use this for initialization
	public void Start () {
        lineRenderer = GameObject.Find("Path").GetComponent<LineRenderer>();
        sim = this.transform.parent.GetComponent<Simulation>();
        interactions = new List<Interaction>();

        // fake interactions
        AddInteraction(new Interaction(0, 1, 0));
        AddInteraction(new Interaction(20, 1, 1000));
	}
	
    public void SetWaypoints(List<Waypoint> waypoints) {
        this.waypoints = waypoints;
        RenderPath();
        RenderInteractions();
    }

    public List<Interaction> GetInteractions() {
        return this.interactions;
    }

    public void AddInteraction(Interaction interaction) {
        interactions.Add(interaction);
        interactions.Sort(delegate(Interaction x, Interaction y) {
            return x.time.CompareTo(y.time);
        });
        EventManager.InteractionsChanged();
    }

    public void RemoveInteraction(Interaction interaction) {
        interactions.Remove(interaction);
        EventManager.InteractionsChanged();
    }

    protected void RenderPath() {
        Waypoint[] waypointsArray = waypoints.ToArray();
        lineRenderer.startWidth = sim.scale;
        lineRenderer.endWidth = sim.scale;
        lineRenderer.positionCount = waypointsArray.Length;
        for (int i = 0; i < waypointsArray.Length; i++)
        {
            Vector3 position = waypointsArray[i].position;
            lineRenderer.SetPosition(i, position * sim.scale);
        }
    }

    protected void RenderInteractions() {
        Interaction[] interactionArray = interactions.ToArray();
        for(int i = 0; i < interactionArray.Length; i++) {
            Interaction interaction = interactionArray[i];
            Waypoint waypoint = waypoints.FindLast(wp => wp.time <= interaction.time);
            Vector3 position = new Vector3(0f, 0f, 0f);
            if (waypoint != null) {
                position = waypoint.position;
            }

            GameObject marker = interaction.marker;

            if (marker == null)
            {
                // instantiate new marker prefab
                marker = (GameObject)Instantiate(Resources.Load("InteractionMarker"));
                marker.transform.parent = this.transform;
                // set references
                interaction.marker = marker;
                marker.GetComponent<InteractionMarker>().Init(interactionArray[i], sim);
            }

            // set position and name
            marker.transform.position = position;
            marker.transform.localScale = new Vector3(3f, 3f, 3f);
            marker.transform.name = "Interaction_" + i;
        }
    }
}
