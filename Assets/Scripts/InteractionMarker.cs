using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class InteractionMarker : VRTK_InteractableObject
{
    public Interaction interaction;
    public Simulation sim;
    public static InteractionMarker active;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        EventManager.OnCloseInteractionUI += Close;
    }

    protected override void Update()
    {
        base.Update();
        base.ToggleHighlight(interaction.edit);

    }

    public void Init(Simulation sim, Interaction interaction)
    {
        this.interaction = interaction;
        Debug.Log("interaction edit bool:");
        Debug.Log(interaction.edit);
        this.sim = sim;
    }

    public override void StartUsing(VRTK_InteractUse usingObject)    {
        Debug.Log("start using interaction marker");
        base.StartUsing(usingObject);

        if (InteractionMarker.active && InteractionMarker.active != this) {
            InteractionMarker.active.Close();
        } else {
            this.interaction.edit = true;
            InteractionMarker.active = this;

            EventManager.OpenInteractionUI(sim, interaction);
        }    }    public override void StopUsing(VRTK_InteractUse usingObject)    {
        Debug.Log("stop using interaction marker");        base.StopUsing(usingObject);
        this.interaction.edit = false;
        InteractionMarker.active = null;        EventManager.CloseInteractionUI();    }

    public void Close()
    {
        Debug.Log("close");
        this.interaction.edit = false;
        if (base.usingObject != null)
        {
            this.StopUsing(base.usingObject);
            Debug.Log("stop using");
        }
    }
}