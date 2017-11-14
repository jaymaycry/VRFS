using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightEngine {
    public Aircraft aircraft;

    /**
     * ENVIRONMENT
     */
    public double g = 9.81; // gravity
    public Vector2 v_wind0 = new Vector2(-5, 0); // windspeed



    public FlightEngine(Aircraft aircraft) {
        this.aircraft = aircraft;
    }

    // Fl
    Vector2 calcDrag(Vector2 v_wind, double cw, double roh)
    {
        Vector2 v_windtotal = v_wind0 + v_wind;

        float help = (float)(cw * (roh / 2.0) * aircraft.wingArea * v_windtotal.magnitude);
        return new Vector2(help * v_windtotal.x, help * v_windtotal.y);
    }

    // Fa
    Vector2 calcLift(Vector2 v_wind, double ca, double roh)
    {
        Vector2 v = v_wind0 + v_wind;

        float help = (float)(ca * (roh / 2.0) * aircraft.wingArea * v.magnitude);
        return new Vector2(help * v.y, -help * v.x);
    }

    // Fg
    Vector2 calcGravity()
    {
        return new Vector2(0, (float)(-aircraft.mass * g));
    }

    double degreeToRadians(double pitch) {
        return Mathf.PI * 2 * (pitch / 360);
    }

    // Fs
    Vector2 calcThrust(double pitch, double thrustFactor)
    {
        double totalThrust = aircraft.engines * aircraft.maxThrust * thrustFactor;
        return new Vector2((float)(Mathf.Cos((float)degreeToRadians(pitch)) * totalThrust),
                           (float)(Mathf.Sin((float)degreeToRadians(pitch)) * totalThrust));
    }

    // gamma
    double calcSlope(Vector2 trajectory)
    {
        return Vector2.Angle(new Vector2(0, 0), trajectory);
    }

    // alpha
    double calcAngleOfAttack(double pitch, Vector2 trajectory)
    {
        return calcSlope(trajectory) - pitch;
    }

    /**
     * fills the array with interactions between the setted ones for easier processing
     * TODO "smooth" out the interactions in between to have softer transitions of pitch
     */
    List<Interaction> interpolateInteractions(List<Interaction> interactions, int steps) {
        List<Interaction> interpolated = new List<Interaction>();

        Interaction current = new Interaction(0, 0, 0);
        Interaction next = new Interaction(0, 0, steps-1);

        int j = 0;

        // set first interaction as next
        if (interactions.Count > 0) {
            next = interactions[0];
            j++;
        }

        for (int i = 0; i < steps; i++) {
            // update current
            if (i >= next.time) {
                current = next;
                j++;
            }

            if (j < interactions.Count && i < interactions[j].time) {
                next = interactions[j];
            }

            interpolated.Add(current);
        }

        return interpolated;
    }

    public List<Waypoint> CalculateWaypoints(int steps, float deltaTime, List<Interaction> interactions)
    {
        List<Waypoint> waypoints = new List<Waypoint>();
    
        Vector3 velocity = new Vector3(0f, 0f, 0f);
        Vector3 position = new Vector3(0f, 0f, 0f); 
        Vector3 rotation = new Vector3(0f, 0f, 0f);
        double pitch = 0;
        bool grounded = true;
        double thrustFactor = 0;
        double angleOfAttack = 0;
        double cw = aircraft.cW0;
        double ca = aircraft.cA0;
        //TODO Luftdichte berechnen abhängig Temperatur und Höhe und Luftfeuchtigkeit
        double roh = 1.2;

        // interpolate instructions
        List<Interaction> interpolatedInteractions = interpolateInteractions(interactions, steps);

        // add default position and rotation to start
        waypoints.Add(new Waypoint(position, rotation, 0));

        for (int i = 1; i < steps; i++) {
            pitch = interpolatedInteractions[i].pitch;
            thrustFactor = interpolatedInteractions[i].thrust;

            var windspeed = new Vector2(-velocity.z, -velocity.y);

            Vector2 drag = calcDrag(windspeed, cw, roh);
            Vector2 lift = calcLift(windspeed, ca, roh);
            Vector2 gravity = calcGravity();
            Vector2 thrust = calcThrust(pitch, thrustFactor);

            Vector2 resultingforce = drag + lift + gravity + thrust;


            // TODO clean this ugly hacks
            if (position.y > 0 && grounded) {
                grounded = false;
            } else {
                grounded = true;
            }
            // prevent sinking
            if (resultingforce.y < 0 && grounded) {
                resultingforce.y = 0;
            }

            /*Debug.Log("windspeed");
            Debug.Log(windspeed);
            Debug.Log("Fw(windspeed)");
            Debug.Log(Fw(windspeed));

            Debug.Log("Auftrieb");
            Debug.Log(calcFa(windspeed));
            Debug.Log("Fg");
            Debug.Log(calcFg());
            /*Debug.Log("Fs");
            Debug.Log(Fs(pitch));
            Debug.Log("Fres t=" + i);
            Debug.Log(resultingforce);

            Debug.Log("Position x t=" + i);
            Debug.Log(position);
            Debug.Log("Velocity x t=" + i);
            Debug.Log(velocity);*/

            angleOfAttack = calcAngleOfAttack(pitch, resultingforce);
            cw = aircraft.calcCW(angleOfAttack);
            ca = aircraft.calcCA(angleOfAttack);

            velocity = velocity + (new Vector3(0f, resultingforce.y, resultingforce.x) * (float)(1 / aircraft.mass) * deltaTime);

            position = position + velocity * deltaTime;
            rotation = new Vector3((float)-pitch, 0f, 0f);


            waypoints.Add(new Waypoint(position,
                                       rotation,
                                       new Vector3(0f, drag.y, drag.x),
                                       new Vector3(0f, lift.y, lift.x),
                                       new Vector3(0f, gravity.y, gravity.x),
                                       new Vector3(0f, thrust.y, thrust.x),
                                       i));
        }

        return waypoints;
    }
}
