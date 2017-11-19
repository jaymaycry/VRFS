using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    public delegate void ChangeAction();
    public static event ChangeAction OnChange;

    public static void AircraftChanged() {
        Debug.Log("Aircraft changed!");
        if (OnChange != null)
            OnChange();
    }

    public static void InteractionsChanged() {
        Debug.Log("Interactions Changed");
        if (OnChange != null)
            OnChange();
    }
}
