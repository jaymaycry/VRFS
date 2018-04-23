using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlightEngine
{
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

    private static double DegreeToRadians(double pitch)
    {
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
        return Vector2.Angle(new Vector2(1, 0), trajectory);
    }

    // alpha
    private static double CalcAngleOfAttack(double pitch, Vector2 trajectory)
    {
        return pitch - CalcSlope(trajectory);
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
            next = interactions[j];
            j++;
        }

        for (int i = 0; i < steps; i++)
        {
            // update current
            if (i >= next.time)
            {
                current = next;

                if (j < interactions.Count)
                    next = interactions[j];
                
                j++;
            }

            interpolated.Add(current);
        }

        return interpolated;
    }

    public static double Roh(double height, double temperature)
    {
        height = height + 153.85 * (temperature - 20); //t = 20°C ist der normalfall, bei abweichung berechnen wir die höhe relativ zur gegebenen Temperatur
        return 1.247015 * Mathd.Exp(-0.000104 * height);
    }

    public static List<Waypoint> CalculateWaypoints(Aircraft aircraft, List<Interaction> interactions, Vector2 windVelocity, double metersAboveSeaLevel, double temperature, float deltaTime, int steps)
    {
        List<Waypoint> waypoints = new List<Waypoint>();

        Vector3 velocity = new Vector3(0f, 0f, 0f);
        Vector3 position = new Vector3(0f, 0f, 0f);
        Vector3 rotation = new Vector3(0f, 0f, 0f);
        double pitch = 0;
        bool grounded = true;
        double thrustFactor = 0;
        double angleOfAttack = 0;
        double cw = 0;
        double ca = 0;

        // interpolate instructions
        List<Interaction> interpolatedInteractions = InterpolateInteractions(interactions, steps);

        // add default position and rotation to start
        waypoints.Add(new Waypoint(position, rotation, velocity, 0));

        for (int i = 1; i < steps; i++)
        {
            pitch = interpolatedInteractions[i].pitch;
            thrustFactor = interpolatedInteractions[i].thrust;

            Vector2 aircraftWindVelocity = new Vector2(-velocity.z, -velocity.y);

            Vector2 drag = CalcDrag(aircraft, windVelocity, aircraftWindVelocity, cw, Roh(position.y + metersAboveSeaLevel, temperature));
            Vector2 lift = CalcLift(aircraft, windVelocity, aircraftWindVelocity, ca, Roh(position.y + metersAboveSeaLevel, temperature));
            Vector2 gravity = CalcGravity(aircraft.mass);
            Vector2 thrust = CalcThrust(aircraft, pitch, thrustFactor);

            Vector2 resultingforce = drag + lift + gravity + thrust;

            // check if ground hit
            grounded = (position.y <= 0);
    
            // prevent sinking
            if (resultingforce.y < 0 && grounded)
            {
                resultingforce.y = 0;
            }

            angleOfAttack = CalcAngleOfAttack(pitch, new Vector2(velocity.z, velocity.y));
            // todo fix this....
            cw = aircraft.CalcCW(angleOfAttack);
            ca = aircraft.CalcCA(angleOfAttack);
            // Debug.Log("pitch: " + pitch + " resultingforce: " + resultingforce + " = angle of attack: " + angleOfAttack);
            // Debug.Log("i:" + i + " ca:" + ca + " cw:" + cw + " pitch: " + pitch + " angle of attack:" + angleOfAttack + " force:" + resultingforce + " drag:" + drag + " lift:" + lift + " gravity:" + gravity + " thrust:" + thrust + " velocity:" + velocity);

            velocity = velocity + (new Vector3(0f, resultingforce.y, resultingforce.x) * (float)(1 / aircraft.mass) * deltaTime);

            position = position + velocity * deltaTime;

            rotation = new Vector3((float)-pitch, 0f, 0f);


            waypoints.Add(new Waypoint(position,
                                       rotation,
                                       velocity,
                                       new Vector3(0f, drag.y, drag.x),
                                       new Vector3(0f, lift.y, lift.x),
                                       new Vector3(0f, gravity.y, gravity.x),
                                       new Vector3(0f, thrust.y, thrust.x),
                                       i));
        }
        Debug.Log("Waypoints calculated");
        return waypoints;
    }
}