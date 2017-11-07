using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightSimulator_Euler : MonoBehaviour
{

    public Rigidbody plane;
    //Flugzeugspezifisch
    public double cw = 1;
    public double ca = 1;

    //Tragfläche
    public double A = 1;
    //Windgeschwindigkeit
    public Vector2 v_wind0 = new Vector2(0, 0);
    //Masse
    public double m = 1;
    //Schubkraft
    public double fs = 1;


    //Umweltspezifisch
    public double g = 9.81;

    //TODO Luftdichte berechnen abhängig Temperatur und Höhe und Luftfeuchtigkeit
    double roh = 1;


    //zeitlich ändernde Parameter
    private Vector2 v_Flugzeug = new Vector2(0, 0);
    private float pitch = Mathf.PI / 18;




    // Wiederstandskraft
    Vector2 Fw(Vector2 v_wind)
    {
        Vector2 v_windtotal = v_wind0 + v_wind;
        /*Debug.Log("v_windtotal");
        Debug.Log(v_windtotal);
        Debug.Log("v_wind0");
        Debug.Log(v_wind0);
        */
        float help = (float)(cw * (roh / 2.0) * A * v_windtotal.magnitude);
        return new Vector2(help * v_windtotal.x, help * v_windtotal.y);
    }

    // Auftriebskraft
    Vector2 Fa(Vector2 v_wind)
    {
        Vector2 v = v_wind0 + v_wind;

        float help = (float)(cw * (roh / 2.0) * A * v.magnitude);
        return new Vector2(help * v.y, -help * v.x);
    }

    // Schwerkraft
    Vector2 Fg()
    {
        return new Vector2(0, (float)(-m * g));
    }

    //Schubkraft abgängig vom Pitch beta
    Vector2 Fs(float beta)
    {
        return new Vector2((float)(Mathf.Cos(beta) * fs), (float)(Mathf.Sin(beta) * fs));
    }

    //effektiver Flugwinkel
    double gamma()
    {
        Vector2 fa = Fa(v_Flugzeug);
        return Mathf.Atan(-fa.x / fa.y);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO check dimensions
        var windspeed = new Vector2(-plane.velocity.z, -plane.velocity.y);

        Debug.Log("windspeed");
        Debug.Log(windspeed);
        Debug.Log("Fw(windspeed)");
        Debug.Log(Fw(windspeed));

        Debug.Log("Auftrieb");
        Debug.Log(Fa(windspeed));
        Debug.Log("Fg");
        Debug.Log(Fg());
        Debug.Log("Fs");
        Debug.Log(Fs(pitch));
        Debug.Log("Fres");
        Vector2 resultingforce = Fw(windspeed) + Fa(windspeed) + Fg() + Fs(pitch);
        Debug.Log(resultingforce);

        if (resultingforce.y < 0) { resultingforce.y = 0; }
        plane.AddForce(0.0f, resultingforce.y, resultingforce.x);
    }
}

