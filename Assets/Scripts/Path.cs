using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour {

    LineRenderer lineRenderer;
    Waypoint[] waypoints;
	// Use this for initialization
	void Start () {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
	}
	
    public void updateWaypoints(Waypoint[] newWaypoints) {
        waypoints = newWaypoints;
        lineRenderer.positionCount = waypoints.Length;
        for (int i = 0; i < waypoints.Length; i++) {
            lineRenderer.SetPosition(i, waypoints[i].position);
        }
    }
}
