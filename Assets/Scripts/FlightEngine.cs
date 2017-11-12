using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightEngine {
    //Flugzeugspezifisch
    public double cw = 0.02;
    public double ca = 1.041;
    //Tragfläche
    public double A = 846;
    //Masse
    public double m = 492000;
    //Schubkraft
    public double fs = 1244000;


    //Umweltspezifisch
    public double g = 9.81;
    //Windgeschwindigkeit
    public Vector2 v_wind0 = new Vector2(-5, 0);
    //TODO Luftdichte berechnen abhängig Temperatur und Höhe und Luftfeuchtigkeit
    double roh = 1.2;


    //zeitlich ändernde Parameter
    private Vector2 v_Flugzeug = new Vector2(0, 0);
    private float pitch = Mathf.PI / 18;


    // Wiederstandskraft
    Vector2 Fw(Vector2 v_wind)
    {
        Vector2 v_windtotal = v_wind0 + v_wind;

        float help = (float)(cw * (roh / 2.0) * A * v_windtotal.magnitude);
        return new Vector2(help * v_windtotal.x, help * v_windtotal.y);
    }

    // Auftriebskraft
    Vector2 Fa(Vector2 v_wind)
    {
        Vector2 v = v_wind0 + v_wind;

        float help = (float)(ca * (roh / 2.0) * A * v.magnitude);
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

    public List<Waypoint> CalculateWaypoints(int steps, float deltaTime)
    {
        List<Waypoint> waypoints = new List<Waypoint>();
    
        Vector3 velocity = new Vector3(0f, 0f, 0f);

        Vector3 position = new Vector3(0f, 0f, 0f); 
        Vector3 rotation = new Vector3(0f, 0f, 0f);

        waypoints.Add(new Waypoint(position, rotation, 0));

        for (int i = 1; i < steps; i++) {
            var windspeed = new Vector2(-velocity.z, -velocity.y);

            Vector2 resultingforce = Fw(windspeed) + Fa(windspeed) + Fg() + Fs(pitch);

            // prevent sinking
            if (resultingforce.y < 0) {
                resultingforce.y = 0;
            }

            /*Debug.Log("windspeed");
            Debug.Log(windspeed);
            Debug.Log("Fw(windspeed)");
            Debug.Log(Fw(windspeed));*/

            Debug.Log("Auftrieb");
            Debug.Log(Fa(windspeed));
            Debug.Log("Fg");
            Debug.Log(Fg());
            /*Debug.Log("Fs");
            Debug.Log(Fs(pitch));
            Debug.Log("Fres t=" + i);
            Debug.Log(resultingforce);*/



            velocity = velocity + (new Vector3(0f, resultingforce.y, resultingforce.x) * (float)(1/m) * deltaTime);

            position = position + velocity * deltaTime;
            Debug.Log("Position x t=" + i);
            Debug.Log(position);
            Debug.Log("Velocity x t=" + i);
            Debug.Log(velocity);
            waypoints.Add(new Waypoint(position, rotation, i));
        }

        return waypoints;
    }
}
