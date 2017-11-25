using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlightEngineRunge {
    public static double g = 9.81; // gravity

    // Fl
    private static Vector2 CalcDrag(Aircraft aircraft, Vector2 windVelocity, Vector2 aircraftWindVelocity, double cw, double roh)
    {
        Vector2 windVelocityTotal = windVelocity + aircraftWindVelocity;

        float help = (float)(cw * (roh / 2.0) * aircraft.wingArea * windVelocityTotal.magnitude);
        return new Vector2(help * windVelocityTotal.x, help * windVelocityTotal.y);
    }

    // Fa
    private static Vector2 CalcLift(Aircraft aircraft, Vector2 windVelocity, Vector2 aircraftWindVelocity, double ca, double roh)
    {
        Vector2 windVelocityTotal = windVelocity + aircraftWindVelocity;

        float help = (float)(ca * (roh / 2.0) * aircraft.wingArea * windVelocityTotal.magnitude);
        return new Vector2(help * windVelocityTotal.y, -help * windVelocityTotal.x);
    }

    // Fg
    private static Vector2 CalcGravity(double mass)
    {
        return new Vector2(0, (float)(-mass * g));
    }

    private static double DegreeToRadians(double pitch) {
        return Mathf.PI * 2 * (pitch / 360);
    }

    // Fs
    private static Vector2 CalcThrust(Aircraft aircraft, double pitch, double thrustFactor)
    {
        double totalThrust = aircraft.engines * aircraft.maxThrust * thrustFactor;
        return new Vector2((float)(Mathf.Cos((float)DegreeToRadians(pitch)) * totalThrust),
                           (float)(Mathf.Sin((float)DegreeToRadians(pitch)) * totalThrust));
    }

    // gamma
    private static double CalcSlope(Vector2 trajectory)
    {
        return Vector2.Angle(new Vector2(0, 0), trajectory);
    }

    // alpha
    private static double CalcAngleOfAttack(double pitch, Vector2 trajectory)
    {
        return CalcSlope(trajectory) - pitch;
    }

    /**
     * fills the array with interactions between the setted ones for easier processing
     * TODO "smooth" out the interactions in between to have softer transitions of pitch
     */
    private static List<Interaction> InterpolateInteractions(List<Interaction> interactions, int steps) {
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

    public static Vector2 calculateResultingForce(Aircraft aircraft, Interaction interaction, Vector3 position, Vector3 velocity, Vector2 windVelocity, double ca, double cw, double roh, float deltaTime)
    {
        var aircraftWindVelocity = new Vector2(-velocity.z, -velocity.y);
        Vector2 drag = CalcDrag(aircraft, windVelocity, aircraftWindVelocity, cw, roh);
        Vector2 lift = CalcLift(aircraft, windVelocity, aircraftWindVelocity, ca, roh);
        Vector2 gravity = CalcGravity(aircraft.mass);
        Vector2 thrust = CalcThrust(aircraft, interaction.pitch, interaction.thrust);
        Vector2 resultingforce = drag + lift + gravity + thrust;
        // prevent sinking
        if (resultingforce.y < 0 && position.y <= 0)
        {
            resultingforce.y = 0;
        }
        return resultingforce;
    }


    public static Vector3 calculateVelocity(Aircraft aircraft, Interaction interaction, Vector3 position, Vector3 velocity, Vector2 windVelocity, double ca, double cw, double roh, float deltaTime) {
        Vector2 resultingforce = calculateResultingForce(aircraft, interaction, position, velocity, windVelocity, ca, cw, roh, deltaTime);
        //Debug.Log(((new Vector3(0f, resultingforce.y, resultingforce.x)) * ((float)(1 / aircraft.mass) * deltaTime)));
        return velocity + (((new Vector3(0f, resultingforce.y, resultingforce.x)) * deltaTime) * (float)(1 / aircraft.mass));
    }

    public static double calculateCW(Vector2 resultingforce, Interaction interaction, Aircraft aircraft) {
        double angleOfAttack = CalcAngleOfAttack(interaction.pitch, resultingforce);
        return aircraft.CalcCW(angleOfAttack);
    }

    public static double calculateCA(Vector2 resultingforce, Interaction interaction, Aircraft aircraft)
    {
        double angleOfAttack = CalcAngleOfAttack(interaction.pitch, resultingforce);
        return aircraft.CalcCA(angleOfAttack);
    }


    public static List<Waypoint> CalculateWaypoints(Aircraft aircraft, List<Interaction> interactions, Vector2 windVelocity, float deltaTime, int steps)
    {
        List<Waypoint> waypoints = new List<Waypoint>();
        Vector2 resultingforce = new Vector2(0f, 0f);
        Vector3 velocity = new Vector3(0f, 0f, 0f);
        Vector3 position = new Vector3(0f, 0f, 0f);
        Vector3 rotation = new Vector3(0f, 0f, 0f);
        double pitch = 0;
        bool grounded = true;
        // double thrustFactor = 0;
        double angleOfAttack = 0;
        double cw = aircraft.cW0;
        double ca = aircraft.cA0;
        //TODO Luftdichte berechnen abhängig Temperatur und Höhe und Luftfeuchtigkeit
        double roh = 1.2;

        // interpolate instructions
        List<Interaction> interpolatedInteractions = InterpolateInteractions(interactions, steps);

        // add default position and rotation to start
        waypoints.Add(new Waypoint(position, rotation, velocity, 0));

        for (int i = 1; i < steps; i++)
        {
            Interaction interaction = interpolatedInteractions[i];
            // Vector2 resultingForce = 

            // Vector3 velocity_k4 = calculateVelocity(aircraft, interpolatedInteractions[i], position, velocity, windVelocity, ca, cw, roh, deltaTime);

            // velocity = velocity_k4;


            // position = position + velocity * deltaTime;
            Vector2 acceleration_k1 = resultingforce;
            Vector3 velocity_k1 = velocity;

            Vector2 acceleration_k2 = calculateResultingForce(aircraft, interaction, position, velocity_k1, windVelocity, ca, cw, roh, deltaTime / 2);
            ca = calculateCA(acceleration_k2, interaction, aircraft);
            cw = calculateCW(acceleration_k2, interaction, aircraft);
            Vector3 velocity_k2 = calculateVelocity(aircraft, interaction, position, velocity_k1, windVelocity, ca, cw, roh, deltaTime / 2);

            //Debug.Log(acceleration_k2);
            //Debug.Log(velocity_k2);

            Vector2 acceleration_k3 = calculateResultingForce(aircraft, interaction, position, velocity_k2, windVelocity, ca, cw, roh, deltaTime / 2);
            ca = calculateCA(acceleration_k3, interaction, aircraft);
            cw = calculateCW(acceleration_k3, interaction, aircraft);
            Vector3 velocity_k3 = calculateVelocity(aircraft, interaction, position, velocity_k2, windVelocity, ca, cw, roh, deltaTime / 2);

            Vector2 acceleration_k4 = calculateResultingForce(aircraft, interaction, position, velocity_k3, windVelocity, ca, cw, roh, deltaTime / 2);
            ca = calculateCA(acceleration_k3, interaction, aircraft);
            cw = calculateCW(acceleration_k3, interaction, aircraft);
            Vector3 velocity_k4 = calculateVelocity(aircraft, interaction, position, velocity_k3, windVelocity, ca, cw, roh, deltaTime);

            resultingforce = deltaTime / 6 * (acceleration_k1 + 2 * acceleration_k2 + 2 * acceleration_k3 + acceleration_k4);
            velocity = deltaTime / 6 * (velocity_k1 + 2 * velocity_k2 + 2 * velocity_k3 + velocity_k4);
            Debug.Log(velocity);
            position = position + velocity * deltaTime;


            rotation = new Vector3((float)-pitch, 0f, 0f);
            // Vector2 resultingforce = calculateResultingForce(aircraft, interpolatedInteractions[i], velocity, windVelocity, ca, cw, roh, deltaTime);
            // angleOfAttack = CalcAngleOfAttack(pitch, resultingforce);
            // cw = aircraft.CalcCW(angleOfAttack);
            // ca = aircraft.CalcCA(angleOfAttack);


            waypoints.Add(new Waypoint(position,
                                       rotation,
                                       velocity,
                                       new Vector3(0f, 0f, 0f),
                                       new Vector3(0f, 0f, 0f),
                                       new Vector3(0f, 0f, 0f),
                                       new Vector3(0f, 0f, 0f),
                                       i));
        }
        Debug.Log("Waypoints calculated");
        return waypoints;
    }
}
