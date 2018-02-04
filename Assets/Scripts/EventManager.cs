using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // simulation actions
    public delegate void ChangeAction(Simulation sim);
    public delegate void CreateInteractionAction(Simulation sim);
    public delegate void CloneSimulationAction(Simulation sim);
    public delegate void RemoveSimulationAction(Simulation sim);
    public delegate void SimulationsChangedAction(List<Simulation> sims);

    public static event ChangeAction OnChange;
    public static event CreateInteractionAction OnCreateInteraction;

    public static event CloneSimulationAction OnCloneSimulation;
    public static event RemoveSimulationAction OnRemoveSimulation;

    public static event SimulationsChangedAction OnSimulationsChanged;

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

    public delegate void OpenAircraftUIAction(Simulation sim, Aircraft aircraft);
    public delegate void CloseAircraftUIAction();

    public static event OpenAircraftUIAction OnOpenAircraftUI;
    public static event CloseAircraftUIAction OnCloseAircraftUI;

    public delegate void OpenPolarCurveUIAction(Simulation sim, Aircraft aircraft);
    public delegate void ClosePolarCurveUIAction();

    public static event OpenPolarCurveUIAction OnOpenPolarCurveUI;
    public static event ClosePolarCurveUIAction OnClosePolarCurveUI;

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

    public static void CloneSimulation(Simulation sim)
    {
        Debug.Log("simulation cloned");
        if (OnCloneSimulation != null)
            OnCloneSimulation(sim);
    }

    public static void RemoveSimulation(Simulation sim)
    {
        Debug.Log("simulation cloned");
        if (OnRemoveSimulation != null)
            OnRemoveSimulation(sim);
    }

    public static void SimulationsChanged(List<Simulation> sims)
    {
        Debug.Log("simulations changed");
        if (OnSimulationsChanged != null)
            OnSimulationsChanged(sims);
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

        CloseAircraftUI();
        ClosePolarCurveUI();

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

    public static void OpenAircraftUI(Simulation sim, Aircraft aircraft)
    {
        Debug.Log("open aircraft ui");

        CloseInteractionUI();
        ClosePolarCurveUI();

        if (OnOpenAircraftUI != null)
            OnOpenAircraftUI(sim, aircraft);
    }

    public static void CloseAircraftUI()
    {
        Debug.Log("close aircraft ui");

        if (OnCloseAircraftUI != null)
            OnCloseAircraftUI();
    }

    public static void OpenPolarCurveUI(Simulation sim, Aircraft aircraft)
    {
        Debug.Log("open polar curve ui");

        CloseInteractionUI();
        CloseAircraftUI();

        if (OnOpenPolarCurveUI != null)
            OnOpenPolarCurveUI(sim, aircraft);
    }

    public static void ClosePolarCurveUI()
    {
        Debug.Log("close polar curve ui");
        if (OnClosePolarCurveUI != null)
            OnClosePolarCurveUI();
    }
}