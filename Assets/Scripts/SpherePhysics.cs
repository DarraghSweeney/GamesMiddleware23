using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePhysics : MonoBehaviour, ICollidable
{
    private PhysicsManager physicsManager;
    private PlaneScript planeScript;
    private SoundManager soundManager;
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
    private float soundCoolDown = 2f;
    private float cooldownCounter = 0f;
    private int BounceCount = 0;
    private int BounceTotal = 5;

    void Start()
    {
        planeScript = FindAnyObjectByType<PlaneScript>();
        soundManager = FindAnyObjectByType<SoundManager>();
        physicsManager = FindAnyObjectByType<PhysicsManager>();
        Acceleration = GravityConstant * Vector3.down;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
      UpdatePosition();

        cooldownCounter++;

        PlaneNormal = planeScript.transform.up.normalized;

        SphereToPlane = ICollidable.distance(planeScript.transform.position, transform.position);

        PlaneParallel = ICollidable.Parallel(SphereToPlane,PlaneNormal);

        PlanePerpendicular = ICollidable.Perpendicular(SphereToPlane,PlaneNormal);

        Distance = Vector3.Dot(SphereToPlane, planeScript.transform.up);

        float d0 = SphereToPlane.magnitude;
        float d1 = PlaneParallel.magnitude - Radius;

        if (d1 <= 0)
        {
            float t1 = (d1 / (d1 - d0)) * Time.deltaTime;
            Vector3 posAtTOI = transform.position - DeltaS * t1;
            Vector3 velocityAtTOI = Velocity - Acceleration * t1;         
            newVelocityAtTOI = ICollidable.Rebound(velocityAtTOI, planeScript.Normal, Cior);           
            Velocity = newVelocityAtTOI - Acceleration * t1;
            transform.position = posAtTOI + newVelocityAtTOI * t1;

            if (soundManager && cooldownCounter > soundCoolDown)
            {
                soundManager.PlayRandomSound();
                cooldownCounter = 0f;
                BounceCount++;
            }

        }
        if(BounceCount > BounceTotal)
        {
            DestroyBall();
        }
    }

    private void DestroyBall()
    {
        physicsManager.Spheres.Remove(this);
        Destroy(gameObject);
        Destroy(this);
    }

    internal void ResolveCollisionWithSphere(SpherePhysics sph1, SpherePhysics sph2, float sumOfRadii)
    {
        float dPrev = Vector3.Distance(sph1.OldPosition, sph2.OldPosition) - sumOfRadii;
        float dCurrent = Vector3.Distance(sph1.transform.position, sph2.transform.position) - sumOfRadii;

        float tAtTOI = dCurrent * (Time.deltaTime / (dCurrent - dPrev));
        Vector3 s1PosTOI = sph1.transform.position - sph1.Velocity * tAtTOI;
        Vector3 s2PosTOI = sph2.transform.position - sph2.Velocity * tAtTOI;
        Vector3 s1VelTOI = sph1.Velocity - sph1.Acceleration * tAtTOI;
        Vector3 s2VelTOI = sph2.Velocity - sph2.Acceleration * tAtTOI;

        Vector3 normal = ICollidable.distance(s1PosTOI, s2PosTOI).normalized;
        Vector3 u1 = ICollidable.Parallel(s1VelTOI, normal);
        Vector3 u2 = ICollidable.Parallel(s2VelTOI, normal);
        Vector3 s1 = ICollidable.Perpendicular(s1VelTOI, normal);
        Vector3 s2 = ICollidable.Perpendicular(s2VelTOI, normal);

        float m1 = sph1.mass;
        float m2 = sph2.mass;

        Vector3 v1 = ((m1 - m2) / (m1 + m2)) * u1 + (2 * m2 / (m1 + m2)) * u2;
        Vector3 v2 = (2 * m1 / (m1 + m2)) * u1 + ((m2 - m1) / (m1 + m2)) * u2;

        v1 = (v1 * sph1.Cior) + (sph1.Acceleration * tAtTOI);
        v2 = (v2 * sph2.Cior) + (sph2.Acceleration * tAtTOI);

        sph1.Velocity = v1 + s1;
        sph2.Velocity = v2 + s2;

        sph1.transform.position = s1PosTOI + sph1.Velocity * tAtTOI;
        sph2.transform.position = s2PosTOI + sph2.Velocity * tAtTOI;
    }

    internal void UpdatePosition()
    {
        OldPosition = transform.position;

        DeltaS = Velocity * Time.deltaTime;

        Velocity += Acceleration * Time.deltaTime;

        transform.position += DeltaS;
    }
}
