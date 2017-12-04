using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour {
    public InteractionUI interactionUI;
	public AircraftUI aircraftUI;

	// Use this for initialization
	void Awake () {
        interactionUI = GameObject.Find("UI/Interaction").GetComponent<InteractionUI>();
		aircraftUI = GameObject.Find ("UI/Aircraft").GetComponent<AircraftUI> ();
	}

    private void Start()
    {
        // hide uis
        interactionUI.Hide();
		aircraftUI.Hide ();

        // set rotation of uis to 45 degree in X axis to make it better usable
        transform.rotation = Quaternion.Euler(new Vector3(45f, 0f, 0f));
    }
}
