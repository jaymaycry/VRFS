using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandler : MonoBehaviour {
    LineRenderer lineRenderer;
    List<Waypoint> waypoints;
    List<Interaction> interactions;

	// Use this for initialization
	void Start () {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
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
        lineRenderer.positionCount = waypointsArray.Length;
        for (int i = 0; i < waypointsArray.Length; i++)
        {
            Vector3 position = waypointsArray[i].position;
            position.z = position.z / 2f;
            lineRenderer.SetPosition(i, position);
        }
    }

    protected void RenderInteractions() {
        // TODO: write renderer for interactions as spheres
    }
}
