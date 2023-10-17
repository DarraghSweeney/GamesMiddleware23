using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpheres : MonoBehaviour
{

    [SerializeField] private GameObject SmallBall;
    [SerializeField] private GameObject BigBall;
    private PhysicsManager physicsManager;
    float PlayerBallSpeed;
    // Start is called before the first frame update
    void Start()
    {
        physicsManager = FindAnyObjectByType<PhysicsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnSmallSphere();
        }

        if (Input.GetMouseButtonDown(1))
        {
            SpawnBigSphere();
        }
    }

    private void SpawnBigSphere()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 40f;
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(mousePosition);

        GameObject sphere = Instantiate(BigBall, spawnPos, Quaternion.identity);
        sphere.SetActive(true);

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 shootDirection = (spawnPos - cameraPos).normalized;

        SpherePhysics SphereScript = sphere.GetComponent<SpherePhysics>();

        physicsManager.Spheres.Add(SphereScript);
    }

    private void SpawnSmallSphere()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 40f;
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(mousePosition);

        GameObject sphere = Instantiate(SmallBall, spawnPos, Quaternion.identity);
        sphere.SetActive(true);

        Vector3 cameraPos = Camera.main.transform.position;

        SpherePhysics SphereScript = sphere.GetComponent<SpherePhysics>();

        physicsManager.Spheres.Add(SphereScript);


    }
}
