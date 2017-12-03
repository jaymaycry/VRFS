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
	protected override void Awake () {
        base.Awake();
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

    public override void StartUsing(VRTK_InteractUse usingObject)    {        base.StartUsing(usingObject);        base.ToggleHighlight(true);        ui.Init(this, sim);        ui.Show();    }    public override void StopUsing(VRTK_InteractUse usingObject)    {        base.StopUsing(usingObject);        base.ToggleHighlight(false);        ui.Hide();    }

    public double GetPitch()
    {
        return this.interaction.pitch;
    }

    public double GetThrust()
    {
        return this.interaction.thrust;
    }

    public int GetTime()
    {
        return this.interaction.time;
    }

    public void SetPitch(double pitch)
    {
        this.interaction.pitch = pitch;
        sim.InteractionsChanged();
    }

    public void SetThrust(double thrust)
    {
        this.interaction.thrust = thrust;
        sim.InteractionsChanged();
    }

    public void SetTime(int time)
    {
        this.interaction.time = time;
        sim.InteractionsChanged();
    }
}
