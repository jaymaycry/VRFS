using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PolarCurveUI : MonoBehaviour {
    Simulation sim;
    Aircraft aircraft;

    GameObject m5g;
    GameObject p0g;
    GameObject p5g;
    GameObject p10g;
    GameObject p15g;

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
        m5g = GameObject.Find("UI/PolarCurve/Panel/Curve/-5g");
        p0g = GameObject.Find("UI/PolarCurve/Panel/Curve/0g");
        p5g = GameObject.Find("UI/PolarCurve/Panel/Curve/5g");
        p10g = GameObject.Find("UI/PolarCurve/Panel/Curve/10g");
        p15g = GameObject.Find("UI/PolarCurve/Panel/Curve/15g");

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

        m5g.transform.localPosition = new Vector3((float)aircraft.CalcCW(-5d), (float)aircraft.CalcCA(-5d), 0f);
        p0g.transform.localPosition = new Vector3((float)aircraft.CalcCW(0d), (float)aircraft.CalcCA(0d), 0f);
        p5g.transform.localPosition = new Vector3((float)aircraft.CalcCW(5d), (float)aircraft.CalcCA(5d), 0f);
        p10g.transform.localPosition = new Vector3((float)aircraft.CalcCW(10d), (float)aircraft.CalcCA(10d), 0f);
        p15g.transform.localPosition = new Vector3((float)aircraft.CalcCW(15d), (float)aircraft.CalcCA(15d), 0f);
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
        EventManager.ClosePolarCurveUI();
    }

    // todo all the slider functions that trigger update

}
