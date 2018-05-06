using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Audio;

public class AircraftSound : MonoBehaviour {
    public AudioClip engineSound;
    AudioSource source;
    [Range(0f, 3f)]
    public float pitchMax;
    [Range(0f, 3f)]
    public float pitchMin;
    [Range(0f, 1f)]
    public float volume;

    void Awake () {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = engineSound;
        source.volume = volume;
        source.pitch = pitchMin;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 1f;
    }

    public void Play()
    {
        Debug.Log("Play Sound");
        source.Play();
    }

    public void Stop()
    {
        Debug.Log("Stop Sound");
        source.Stop();
    }

    public void SetThrust(double thrust)
    {
        source.pitch = (pitchMax - pitchMin) * (float)thrust + pitchMin;
    }
}
