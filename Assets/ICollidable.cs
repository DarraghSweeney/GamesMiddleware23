using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollidable
{
    static Vector3 Perpendicular(Vector3 V, Vector3 N)
    {
        Vector3 Result = V - Parallel(V, N);
        return Result;
    }

    static Vector3 Parallel(Vector3 V, Vector3 N)
    {
        Vector3 Result = Vector3.Dot(V, N) * N;

        return Result;
    }

    static bool isColliding(SpherePhysics sphereScript1, SpherePhysics sphereScript2)
    {
        return Vector3.Distance(sphereScript1.newPosition,
                                sphereScript2.newPosition) <
                                sphereScript1.Radius + sphereScript2.Radius;
    }

    static Vector3 Rebound(Vector3 Velocity, Vector3 Normal, float Cior)
    {
       return Perpendicular(Velocity,Normal) - (Cior * Parallel(Velocity, Normal));
    }
}
