using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlightEngineRunge
{
    public static double g = 9.81; // gravity

    // Fl
    private static Vector2d CalcDrag(Aircraft aircraft, Vector2d windVelocity, Vector2d aircraftWindVelocity, double cw, double roh)
    {
        Vector2d windVelocityTotal = windVelocity + aircraftWindVelocity;

        float help = (float)(cw * (roh / 2.0) * aircraft.wingArea * windVelocityTotal.magnitude);
        return new Vector2d(help * windVelocityTotal.x, help * windVelocityTotal.y);
    }

    // Fa
    private static Vector2d CalcLift(Aircraft aircraft, Vector2d windVelocity, Vector2d aircraftWindVelocity, double ca, double roh)
    {
        Vector2d windVelocityTotal = windVelocity + aircraftWindVelocity;

        float help = (float)(ca * (roh / 2.0) * aircraft.wingArea * windVelocityTotal.magnitude);
        return new Vector2d(help * windVelocityTotal.y, -help * windVelocityTotal.x);
    }

    // Fg
    private static Vector2d CalcGravity(double mass)
    {
        return new Vector2d(0, (float)(-mass * g));
    }

    private static double DegreeToRadians(double pitch)
    {
        return Mathf.PI * 2 * (pitch / 360);
    }

    // Fs
    private static Vector2d CalcThrust(Aircraft aircraft, double pitch, double thrustFactor)
    {
        double totalThrust = aircraft.engines * aircraft.maxThrust * thrustFactor;
        return new Vector2d((float)(Mathf.Cos((float)DegreeToRadians(pitch)) * totalThrust),
                           (float)(Mathf.Sin((float)DegreeToRadians(pitch)) * totalThrust));
    }

    // gamma
    private static double CalcSlope(Vector2d trajectory)
    {
        return Vector2d.Angle(new Vector2d(0, 0), trajectory);
    }

    // alpha
    private static double CalcAngleOfAttack(double pitch, Vector2d trajectory)
    {
        return CalcSlope(trajectory) - pitch;
    }

    /**
     * fills the array with interactions between the setted ones for easier processing
     * TODO "smooth" out the interactions in between to have softer transitions of pitch
     */
    private static List<Interaction> InterpolateInteractions(List<Interaction> interactions, int steps)
    {
        List<Interaction> interpolated = new List<Interaction>();

        Interaction current = new Interaction(0, 0, 0);
        Interaction next = new Interaction(0, 0, steps - 1);

        int j = 0;

        // set first interaction as next
        if (interactions.Count > 0)
        {
            next = interactions[0];
            j++;
        }

        for (int i = 0; i < steps; i++)
        {
            // update current
            if (i >= next.time)
            {
                current = next;
                j++;
            }

            if (j < interactions.Count && i < interactions[j].time)
            {
                next = interactions[j];
            }

            interpolated.Add(current);
        }

        return interpolated;
    }

    public static Vector2d calculateResultingForce(Aircraft aircraft, Interaction interaction, Vector3d position, Vector3d velocity, Vector2d windVelocity, double ca, double cw, double roh, float deltaTime)
    {
        var aircraftWindVelocity = new Vector2d(-velocity.z, -velocity.y);
        Vector2d drag = CalcDrag(aircraft, windVelocity, aircraftWindVelocity, cw, roh);
        Vector2d lift = CalcLift(aircraft, windVelocity, aircraftWindVelocity, ca, roh);
        Vector2d gravity = CalcGravity(aircraft.mass);
        Vector2d thrust = CalcThrust(aircraft, interaction.pitch, interaction.thrust);
        Vector2d resultingforce = drag + lift + gravity + thrust;
        // prevent sinking
        if (resultingforce.y < 0 && position.y <= 0)
        {
            resultingforce.y = 0;
        }
        return resultingforce;
    }

    public static Vector3d calculateVelocity(Aircraft aircraft, Vector3d velocity, Vector2d acceleration, float deltaTime)
    {
        return velocity + (((new Vector3d(0f, acceleration.y, acceleration.x)) * deltaTime));
    }

    /*
    public static Vector3d calculateVelocity(Aircraft aircraft, Interaction interaction, Vector3d position, Vector3d velocity, Vector2d windVelocity, double ca, double cw, double roh, float deltaTime)
    {
        Vector2d resultingforce = calculateResultingForce(aircraft, interaction, position, velocity, windVelocity, ca, cw, roh, deltaTime);
        //Debug.Log(((new Vector3d(0f, resultingforce.y, resultingforce.x)) * ((float)(1 / aircraft.mass) * deltaTime)));
        return velocity + (((new Vector3d(0f, resultingforce.y, resultingforce.x)) * deltaTime) * (float)(1 / aircraft.mass));
    }
    */

    public static double calculateCW(Vector2d resultingforce, Interaction interaction, Aircraft aircraft)
    {
        double angleOfAttack = CalcAngleOfAttack(interaction.pitch, resultingforce);
        return aircraft.CalcCW(angleOfAttack);
    }

    public static double calculateCA(Vector2d resultingforce, Interaction interaction, Aircraft aircraft)
    {
        double angleOfAttack = CalcAngleOfAttack(interaction.pitch, resultingforce);
        return aircraft.CalcCA(angleOfAttack);
    }


    public static List<Waypoint> CalculateWaypoints(Aircraft aircraft, List<Interaction> interactions, Vector2d windVelocity, float deltaTime, int steps)
    {
        List<Waypoint> waypoints = new List<Waypoint>();
        Vector2d resultingforce = new Vector2d(0d, 0d);
        Vector3d velocity = new Vector3d(0d, 0d, 0d);
        Vector3d position = new Vector3d(0d, 0d, 0d);
        Vector3d rotation = new Vector3d(0d, 0d, 0d);
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

        Vector3 positionF = new Vector3((float)position.x, (float)position.y, (float)position.z);
        Vector3 rotationF = new Vector3((float)rotation.x, (float)rotation.y, (float)rotation.z);
        Vector3 velocityF = new Vector3((float)velocity.x, (float)velocity.y, (float)velocity.z);
        // add default position and rotation to start
        waypoints.Add(new Waypoint(positionF, rotationF, velocityF, 0));

        for (int i = 1; i < steps; i++)
        {
            Interaction interaction = interpolatedInteractions[i];
            // Vector2d resultingForce = 


            // position = position + velocity * deltaTime;
            Vector2d acceleration_k1 = resultingforce * (1/aircraft.mass);
            Vector3d velocity_k1 = velocity;

            /* a_k1 = F * 1/m
             * v_k1 = v
             * 
             * a_k2 = F(v_k1) *1/m
             * v_k2 = v(v_k1, a_k2, deltaT/2)
             * 
             * a_k3 = F(v_k2) *1/m
             * v_k3 = v(v_k2, a_k3, deltaT/2)
             * 
             * a_k4 = F(v_k3) *1/m
             * v_k4 = v(v_k3, a_k4, deltaT)
             * 
             * f = f + deltaT/6 * ( a_k1 + 2 * a_k2 + 2 * a_k3 + a_k4)
             * a = f*1/m
             * 
             * v = a*deltaT
             * (x,y) = v * deltaT
             */


            Vector2d acceleration_k2 = calculateResultingForce(aircraft, interaction, position, velocity_k1, windVelocity, ca, cw, roh, deltaTime / 2) * (1/aircraft.mass);
            ca = calculateCA(acceleration_k2, interaction, aircraft);
            cw = calculateCW(acceleration_k2, interaction, aircraft);
            Vector3d velocity_k2 = calculateVelocity(aircraft, velocity_k1, acceleration_k2, deltaTime /2);
            // Vector3d velocity_k2 = calculateVelocity(aircraft, interaction, position, velocity_k1, windVelocity, ca, cw, roh, deltaTime / 2);

            //Debug.Log(acceleration_k2);
            //Debug.Log(velocity_k2);

            Vector2d acceleration_k3 = calculateResultingForce(aircraft, interaction, position, velocity_k2, windVelocity, ca, cw, roh, deltaTime / 2) * (1 / aircraft.mass);
            ca = calculateCA(acceleration_k3, interaction, aircraft);
            cw = calculateCW(acceleration_k3, interaction, aircraft);
            Vector3d velocity_k3 = calculateVelocity(aircraft, velocity_k2, acceleration_k3, deltaTime /2);

            Vector2d acceleration_k4 = calculateResultingForce(aircraft, interaction, position, velocity_k3, windVelocity, ca, cw, roh, deltaTime / 2) * (1 / aircraft.mass);
            ca = calculateCA(acceleration_k3, interaction, aircraft);
            cw = calculateCW(acceleration_k3, interaction, aircraft);
            Vector3d velocity_k4 = calculateVelocity(aircraft, velocity_k3, acceleration_k4, deltaTime); ;

            resultingforce = resultingforce + deltaTime / 6 * (acceleration_k1 + 2 * acceleration_k2 + 2 * acceleration_k3 + acceleration_k4);
            //velocity = velocity + deltaTime / 6 * (velocity_k1 + 2 * velocity_k2 + 2 * velocity_k3 + velocity_k4);
            Vector2d acceleration = resultingforce * 1 / aircraft.mass;
            velocity = acceleration * deltaTime;
            Debug.Log(velocity);
            position = position + velocity * deltaTime;


            rotation = new Vector3d((float)-pitch, 0f, 0f);
            // Vector2d resultingforce = calculateResultingForce(aircraft, interpolatedInteractions[i], velocity, windVelocity, ca, cw, roh, deltaTime);
            // angleOfAttack = CalcAngleOfAttack(pitch, resultingforce);
            // cw = aircraft.CalcCW(angleOfAttack);
            // ca = aircraft.CalcCA(angleOfAttack);

            positionF = new Vector3((float)position.x, (float)position.y, (float)position.z);
            rotationF = new Vector3((float)rotation.x, (float)rotation.y, (float)rotation.z);
            velocityF = new Vector3((float)velocity.x, (float)velocity.y, (float)velocity.z);


            waypoints.Add(new Waypoint(positionF,
                                       rotationF,
                                       velocityF,
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