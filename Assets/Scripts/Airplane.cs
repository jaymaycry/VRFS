using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

    public GameObject planeCessna;

    void Start() {
        planeCessna = GameObject.Find("Small Passenger Plane");
    }
    public void reposition(Vector3 position, Vector3 rotation) {
        transform.localPosition = position;
        transform.rotation = Quaternion.Euler(rotation);
    } 
}