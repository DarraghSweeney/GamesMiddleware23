using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsManager : MonoBehaviour, ICollidable
{

    public List<SpherePhysics> Spheres;

    // Start is called before the first frame update
    void Start()
    {
        Spheres = FindObjectsOfType<SpherePhysics>().ToList();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < Spheres.Count; i++)
        {
            for (int j = i+1; j < Spheres.Count; j++)
            {
                if (ICollidable.isColliding(Spheres[i], Spheres[j]))
                {
                    SpherePhysics sph1 = Spheres[i];
                    SpherePhysics sph2 = Spheres[j];
                    float sumOfRadii = sph1.Radius + sph2.Radius;

                    sph1.ResolveCollisionWithSphere(sph1, sph2, sumOfRadii);
                }
            }
        }
    }
}
