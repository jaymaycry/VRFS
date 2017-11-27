using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractionMarker : VRTK_InteractableObject {
    InteractionUI ui;
    Interaction interaction;
    Simulation sim;

	// Use this for initialization
	protected void Awake () {
        ui = GameObject.Find("UI").GetComponent<UIHandler>().interactionUI;
    }

    // Update is called once per frame
    protected override void Update()
    {        base.Update();    }

    public void Init(Interaction interaction, Simulation sim)
    {
        this.interaction = interaction;
        this.sim = sim;
    }

    public override void StartUsing(VRTK_InteractUse usingObject)    {        base.StartUsing(usingObject);        base.ToggleHighlight(true);        ui.Init(interaction, sim);        ui.Show();    }    public override void StopUsing(VRTK_InteractUse usingObject)    {        base.StopUsing(usingObject);        base.ToggleHighlight(false);        ui.Hide();    }
}
