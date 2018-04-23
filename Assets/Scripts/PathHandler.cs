using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandler : MonoBehaviour {
    Simulation sim;
    LineRenderer lineRenderer;
    public List<Waypoint> waypoints;
    public List<Interaction> interactions;
    List<GameObject> interactionMarkers;

	// Use this for initialization
	public void Awake ()
    {
        
        lineRenderer = this.transform.GetComponentInChildren<LineRenderer>();
        sim = this.transform.parent.GetComponent<Simulation>();
        interactions = new List<Interaction>();
        interactionMarkers = new List<GameObject>();
	}
	
    public void SetWaypoints(List<Waypoint> waypoints)
    {
        this.waypoints = waypoints;
        RenderPath();
        RenderInteractions();
    }

    public void SetInteractions(List<Interaction> interactions)
    {
        this.interactions = interactions;
        SortInteractions();
        foreach(Interaction interaction in this.interactions)
        {
            CreateInteractionMarker(sim, interaction);
        }
    }

    protected void CreateInteractionMarker(Simulation sim, Interaction interaction)
    {
        // instantiate new marker prefab
        GameObject marker = (GameObject)Instantiate(Resources.Load("InteractionMarker"));
        marker.transform.parent = this.transform;
        marker.GetComponent<InteractionMarker>().Init(sim, interaction);
        marker.transform.localPosition = new Vector3(0f, 0f, 0f);
        marker.transform.localScale = new Vector3(6f, 6f, 6f);
        this.interactionMarkers.Add(marker);
    }

    public List<Interaction> GetInteractions()
    {
        SortInteractions();
        return this.interactions;
    }

    public void AddInteraction(Interaction interaction)
    {
        interactions.Add(interaction);
        SortInteractions();
        CreateInteractionMarker(sim, interaction);

        EventManager.InteractionChanged(sim);
    }

    protected void SortInteractions()
    {
        interactions.Sort(delegate (Interaction x, Interaction y) {
            return x.time.CompareTo(y.time);
        });
    }

    public void RemoveInteraction(Interaction interaction) {
        interactions.Remove(interaction);
        GameObject marker = interactionMarkers.Find((obj) => obj.GetComponent<InteractionMarker>().interaction == interaction);
        if (marker)
            interactionMarkers.Remove(marker);
        EventManager.InteractionChanged(sim);
    }

    protected void RenderPath()
    {
        Waypoint[] waypointsArray = waypoints.ToArray();
        Material pathMaterial = new Material(Shader.Find("Standard"));
        pathMaterial.SetColor("_Color", sim.color);
        pathMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        pathMaterial.SetColor("_EmissionColor", sim.color);
        pathMaterial.EnableKeyword("_EMISSION");
        lineRenderer.material = pathMaterial;
        lineRenderer.startWidth = SimulationHandler.scale;
        lineRenderer.endWidth = SimulationHandler.scale;
        lineRenderer.positionCount = waypointsArray.Length;
        for (int i = 0; i < waypointsArray.Length; i++)
        {
            Vector3 position = waypointsArray[i].position;
            lineRenderer.SetPosition(i, position);
        }
    }

    protected void RenderInteractions()
    {
        foreach(GameObject marker in interactionMarkers)
        {
            Interaction interaction = marker.GetComponent<InteractionMarker>().interaction;
            Debug.Log(interaction.time);

            Waypoint waypoint = waypoints.FindLast(wp => wp.time <= interaction.time);

            if (waypoint != null) {
                marker.transform.localPosition = waypoint.position;
            } else {
                marker.transform.localPosition = new Vector3(0f, 0f, 0f);;
            }
        }
    }
}
