using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePhysics : MonoBehaviour, ICollidable
{
 
    private PlaneScript planeScript;
    internal Vector3 Velocity;
    private Vector3 Acceleration;
    private float GravityConstant = 9.81f;
    public float Radius { get { return transform.localScale.x / 2.0f;}
    private set { transform.localScale = 2 * value * Vector3.one; } }
private Vector3 SphereToPlane;
    private Vector3 PlaneNormal;
    private Vector3 PlanePerpendicular;
    private Vector3 PlaneParallel;
    internal Vector3 newPosition;
    internal Vector3 OldPosition;
    private float Distance;
    internal float Cior = 0.75f;
    private Vector3 DeltaS;
    public float mass = 1;
    Vector3 newVelocityAtTOI;

    void Start()
    {
        planeScript = FindAnyObjectByType<PlaneScript>();
        Acceleration = GravityConstant * Vector3.down;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      UpdatePosition();

        PlaneNormal = planeScript.transform.up.normalized;

        SphereToPlane = (planeScript.transform.position - newPosition);

        PlaneParallel = ICollidable.Parallel(SphereToPlane,PlaneNormal);

        PlanePerpendicular = ICollidable.Perpendicular(SphereToPlane,PlaneNormal);

        Distance = Vector3.Dot(SphereToPlane, planeScript.transform.up);

        float d0 = SphereToPlane.magnitude;
        float d1 = PlaneParallel.magnitude - Radius;

        if (d1 <= 0)
        {
            float t1 = (d1 / (d1 - d0)) * Time.deltaTime;
            Vector3 posAtTOI = OldPosition;
            Vector3 velocityAtTOI = Velocity - Acceleration * t1;
           
            newVelocityAtTOI = ICollidable.Rebound(velocityAtTOI, planeScript.Normal, Cior); 
                 
            Velocity = newVelocityAtTOI - Acceleration * t1;
            transform.position = posAtTOI + newVelocityAtTOI * t1;
        }

        else
        {           
            transform.position = newPosition;
            OldPosition = newPosition;
        }
    }
     internal Vector3 ResolveCollisionWith(SpherePhysics otherSphere)
    {
        transform.position = OldPosition;
        Vector3 normal = (transform.position - otherSphere.transform.position).normalized;
        Vector3 u1 = ICollidable.Parallel(Velocity, normal);
        Vector3 u2 = ICollidable.Parallel(otherSphere.Velocity, normal);
        Vector3 s1 = ICollidable.Perpendicular(Velocity, normal);
        Vector3 s2 = ICollidable.Perpendicular(otherSphere.Velocity, normal);

        float m1 = mass;
        float m2 = otherSphere.mass;

        Vector3 v1 = ((m1 - m2) / (m1 + m2)) * u1 + (2 * m2 / (m1 + m2)) * u2;

        Vector3 v2 = (2 * m1 / (m1 + m2)) * u1 + ((m2 - m1) / (m1 + m2)) * u2;

        Velocity = v1 + s1;
        UpdatePosition();
        return v2 + s2;
    }

    internal void UpdatePosition()
    {
        DeltaS = Velocity * Time.deltaTime;

        Velocity += Acceleration * Time.deltaTime;

        newPosition = transform.position + DeltaS;
    }
}
