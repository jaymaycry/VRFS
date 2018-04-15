using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneryHandler : MonoBehaviour {

    public Text simName;
    Simulation parentSim;

	// Use this for initialization
	void Start () {
        parentSim = this.transform.parent.GetComponent<Simulation>();
        simName.text = parentSim.name;


        EventManager.OnSimulationsChanged += SimsChanged;
    }

    public void SimsChanged(List<Simulation> sims)
    {
        simName.text = parentSim.name;
    }
}
