using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolarCurveUI : MonoBehaviour {
    Simulation sim;
    Aircraft aircraft;

    GameObject m30g;
    GameObject m15g;
    GameObject m10g;
    GameObject m5g;
    GameObject p0g;
    GameObject p5g;
    GameObject p10g;
    GameObject p15g;
    GameObject p30g;

    Text cA0Value;
    Text cA1Value;
    Text cW0Value;
    Text cW1Value;
    Text cW2Value;

    Slider cA0Slider;
    Slider cA1Slider;
    Slider cW0Slider;
    Slider cW1Slider;
    Slider cW2Slider;

	// Use this for initialization
	void Awake () {
        m30g = GameObject.Find("UI/PolarCurve/Panel/Curve/-30g");
        m15g = GameObject.Find("UI/PolarCurve/Panel/Curve/-15g");
        m10g = GameObject.Find("UI/PolarCurve/Panel/Curve/-10g");
        m5g = GameObject.Find("UI/PolarCurve/Panel/Curve/-5g");
        p0g = GameObject.Find("UI/PolarCurve/Panel/Curve/0g");
        p5g = GameObject.Find("UI/PolarCurve/Panel/Curve/5g");
        p10g = GameObject.Find("UI/PolarCurve/Panel/Curve/10g");
        p15g = GameObject.Find("UI/PolarCurve/Panel/Curve/15g");
        p30g = GameObject.Find("UI/PolarCurve/Panel/Curve/30g");

        cA0Slider = GameObject.Find("UI/PolarCurve/Panel/cA0/Slider").GetComponent<Slider>();
        cA1Slider = GameObject.Find("UI/PolarCurve/Panel/cA1/Slider").GetComponent<Slider>();
        cW0Slider = GameObject.Find("UI/PolarCurve/Panel/cW0/Slider").GetComponent<Slider>();
        cW1Slider = GameObject.Find("UI/PolarCurve/Panel/cW1/Slider").GetComponent<Slider>();
        cW2Slider = GameObject.Find("UI/PolarCurve/Panel/cW2/Slider").GetComponent<Slider>();

        cA0Value = GameObject.Find("UI/PolarCurve/Panel/cA0/Value").GetComponent<Text>();
        cA1Value = GameObject.Find("UI/PolarCurve/Panel/cA1/Value").GetComponent<Text>();
        cW0Value = GameObject.Find("UI/PolarCurve/Panel/cW0/Value").GetComponent<Text>();
        cW1Value = GameObject.Find("UI/PolarCurve/Panel/cW1/Value").GetComponent<Text>();
        cW2Value = GameObject.Find("UI/PolarCurve/Panel/cW2/Value").GetComponent<Text>();

        EventManager.OnOpenPolarCurveUI += Open;
        EventManager.OnClosePolarCurveUI += Close;
	}

    protected void Init()
    {
        Debug.Log("polar curve ui opened - cA0: " + aircraft.cA0.ToString() + " cA1: " + aircraft.cA1.ToString() + " cW0: " + aircraft.cW0.ToString() + " cW1: " + aircraft.cW1.ToString() + " cW2: " + aircraft.cW2.ToString());
        cA0Slider.value = (float)aircraft.cA0;
        cA1Slider.value = (float)aircraft.cA1;
        cW0Slider.value = (float)aircraft.cW0;
        cW1Slider.value = (float)aircraft.cW1;
        cW2Slider.value = (float)aircraft.cW2;

        UpdateValues();
    }

    protected void UpdateValues()
    {
        cA0Value.text = aircraft.cA0.ToString();
        cA1Value.text = aircraft.cA1.ToString();
        cW0Value.text = aircraft.cW0.ToString();
        cW1Value.text = aircraft.cW1.ToString();
        cW2Value.text = aircraft.cW2.ToString();

        m30g.transform.localPosition    = new Vector3((float)aircraft.CalcCWDegree(-30d), (float)aircraft.CalcCADegree(-30d),   0f);
        m15g.transform.localPosition    = new Vector3((float)aircraft.CalcCWDegree(-15d), (float)aircraft.CalcCADegree(-15d),   0f);
        m10g.transform.localPosition    = new Vector3((float)aircraft.CalcCWDegree(-10d), (float)aircraft.CalcCADegree(-10d),   0f);
        m5g.transform.localPosition     = new Vector3((float)aircraft.CalcCWDegree(-5d),  (float)aircraft.CalcCADegree(-5d),    0f);
        p0g.transform.localPosition     = new Vector3((float)aircraft.CalcCWDegree(0d),   (float)aircraft.CalcCADegree(0d),     0f);
        p5g.transform.localPosition     = new Vector3((float)aircraft.CalcCWDegree(5d),   (float)aircraft.CalcCADegree(5d),     0f);
        p10g.transform.localPosition    = new Vector3((float)aircraft.CalcCWDegree(10d),  (float)aircraft.CalcCADegree(10d),    0f);
        p15g.transform.localPosition    = new Vector3((float)aircraft.CalcCWDegree(15d),  (float)aircraft.CalcCADegree(15d),    0f);
        p30g.transform.localPosition    = new Vector3((float)aircraft.CalcCWDegree(30d),  (float)aircraft.CalcCADegree(30d),    0f);
    }

    public void Open(Simulation sim, Aircraft aircraft)
    {
        this.aircraft = aircraft;
        this.sim = sim;
        Show();
    }

    public void Close()
    {
        Hide();
    }

    public void Show()
    {
        Debug.Log("show polar curve ui");
        Init();
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        Debug.Log("hide polar curve ui");
        this.gameObject.SetActive(false);
    }

    public void TriggerClose()
    {
        Debug.Log("close button pressed");
        EventManager.ClosePolarCurveUI();
    }

    public void CA0Changed(float newCA0)
    {
        Debug.Log("cA0 slider changed");
        Debug.Log(newCA0);

        aircraft.cA0 = (double)newCA0;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void CA1Changed(float newCA1)
    {
        Debug.Log("cA1 slider changed");
        Debug.Log(newCA1);

        aircraft.cA1 = (double)newCA1;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void CW0Changed(float newCW0)
    {
        Debug.Log("cW0 slider changed");
        Debug.Log(newCW0);

        aircraft.cW0 = (double)newCW0;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void CW1Changed(float newCW1)
    {
        Debug.Log("cW1 slider changed");
        Debug.Log(newCW1);

        aircraft.cW1 = (double)newCW1;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

    public void CW2Changed(float newCW2)
    {
        Debug.Log("cW2 slider changed");
        Debug.Log(newCW2);

        aircraft.cW2 = (double)newCW2;
        UpdateValues();
        EventManager.AircraftChanged(sim);
    }

}
