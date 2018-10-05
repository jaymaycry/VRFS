using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class UIHandler : MonoBehaviour {
    public InteractionUI interactionUI;
    public PlayerUI playerUI;
    public AircraftUI aircraftUI;
    public PolarCurveUI polarcurveUI;
    public EnvironmentUI environmentUI;
	// Use this for initialization
	void Awake () {
        interactionUI = GameObject.Find("UI/Interaction").GetComponent<InteractionUI>();
        playerUI = GameObject.Find("UI/Player").GetComponent<PlayerUI>();
        aircraftUI = GameObject.Find("UI/Aircraft").GetComponent<AircraftUI>();
        polarcurveUI = GameObject.Find("UI/PolarCurve").GetComponent<PolarCurveUI>();
        environmentUI = GameObject.Find("UI/Environment").GetComponent<EnvironmentUI>();


        VRTK_ControllerEvents controllerEvents = transform.parent.GetComponent<VRTK_ControllerEvents>();

        controllerEvents.TriggerPressed += new ControllerInteractionEventHandler(TriggerPressed);
        controllerEvents.TriggerReleased += new ControllerInteractionEventHandler(TriggerReleased);
	}

    private void Start()
    {
        // hide uis
        interactionUI.Hide();
        playerUI.Hide();
        aircraftUI.Hide();
        polarcurveUI.Hide();
        environmentUI.Hide();

        // set rotation of uis to 45 degree in X axis to make it better usable
        transform.rotation = Quaternion.Euler(new Vector3(45f, 0f, 0f));
    }

    private void TriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("open player ui");
        EventManager.OpenPlayerUI();
    }

    private void TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("close player ui");
        EventManager.ClosePlayerUI();
    }
}