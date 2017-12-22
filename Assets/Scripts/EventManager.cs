using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // simulation actions
    public delegate void ChangeAction(Simulation sim);
    public delegate void CreateInteractionAction(Simulation sim);

    public static event ChangeAction OnChange;
    public static event CreateInteractionAction OnCreateInteraction;


    // player actions
    public delegate void PlayAction();
    public delegate void PauseAction();
    public delegate void SetTimeAction(int time);
    public delegate void SetScaleAction(float scale);

    public static event PlayAction OnPlay;
    public static event PauseAction OnPause;
    public static event SetTimeAction OnSetTime;
    public static event SetScaleAction OnSetScale;

    // gui actions
    public delegate void OpenInteractionUIAction(Simulation sim, Interaction interaction);
    public delegate void CloseInteractionUIAction();

    public static event OpenInteractionUIAction OnOpenInteractionUI;
    public static event CloseInteractionUIAction OnCloseInteractionUI;

    public delegate void OpenPlayerUIAction();
    public delegate void ClosePlayerUIAction();

    public static event OpenPlayerUIAction OnOpenPlayerUI;
    public static event ClosePlayerUIAction OnClosePlayerUI;

    public static void AircraftChanged(Simulation sim)
    {
        Debug.Log("aircraft changed");
        if (OnChange != null)
            OnChange(sim);
    }

    public static void InteractionChanged(Simulation sim)
    {
        Debug.Log("interaction changed");
        if (OnChange != null)
            OnChange(sim);
    }

    public static void CreateInteraction(Simulation sim)
    {
        Debug.Log("create interaction");
        if (OnCreateInteraction != null)
            OnCreateInteraction(sim);
    }

    public static void Play()
    {
        Debug.Log("play triggered!");
        if (OnPlay != null)
            OnPlay();
    }

    public static void Pause()
    {
        Debug.Log("pause triggered");
        if (OnPause != null)
            OnPause();
    }

    public static void SetTime(int time)
    {
        Debug.Log("time changed");
        if (OnSetTime != null)
            OnSetTime(time);
    }

    public static void SetScale(float scale)
    {
        Debug.Log("scale changed");
        if (OnSetScale != null)
            OnSetScale(scale);
    }

    public static void OpenInteractionUI(Simulation sim, Interaction interaction)
    {
        Debug.Log("open interaction ui");
        Debug.Log(sim);
        Debug.Log(interaction);

        if (OnOpenInteractionUI != null)
            OnOpenInteractionUI(sim, interaction);
    }

    public static void CloseInteractionUI()
    {
        Debug.Log("close interaction ui");

        if (OnCloseInteractionUI != null)
            OnCloseInteractionUI();
    }

    public static void OpenPlayerUI()
    {
        Debug.Log("open player ui");

        if (OnOpenPlayerUI != null)
            OnOpenPlayerUI();
    }

    public static void ClosePlayerUI()
    {
        Debug.Log("close player ui");

        if (OnClosePlayerUI != null)
            OnClosePlayerUI();
    }
}