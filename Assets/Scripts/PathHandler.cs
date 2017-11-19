using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathHandler : MonoBehaviour {
    LineRenderer lineRenderer;
    Waypoint[] waypoints;

	// Use this for initialization
	void Start () {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
	}
	
    public void updateWaypoints(List<Waypoint> waypoints) {
        this.waypoints = waypoints.ToArray();
        lineRenderer.positionCount = this.waypoints.Length;
        for (int i = 0; i < this.waypoints.Length; i++) {
            Vector3 position = this.waypoints[i].position;
            position.z = position.z / 2f;
            lineRenderer.SetPosition(i, position);
        }
    }
}
