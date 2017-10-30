using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour {

    public GameObject planeCessna;

    void Start() {
        planeCessna = GameObject.Find("plane_cessna");
    }
    public void reposition(Vector3 position, Vector3 rotation) {
        transform.position = position;
        transform.rotation = Quaternion.Euler(rotation);
    } 
}