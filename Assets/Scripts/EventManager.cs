using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ChangeAction();
    public static event ChangeAction OnChange;

    public static void Play()
    {
        Debug.Log("Play triggered!");
        if (OnChange != null)
            OnChange();
    }

    public static void Pause()
    {
        Debug.Log("Pause triggered");
        if (OnChange != null)
            OnChange();
    }

    public static void Rewind()
    {
        Debug.Log("Rewind triggered");
        if (OnChange != null)
            OnChange();
    }

    public static void Forward()
    {
        Debug.Log("Forward triggered");
        if (OnChange != null)
            OnChange();
    }
}