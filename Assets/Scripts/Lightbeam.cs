using NUnit.Framework.Internal.Execution;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Updates The Lightbeams Line Renderer And What It Does //

public class Lightbeam : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int subdivisions;
    public float shootSpeed;
    public float beamLength;

    public Vector3 velocity;
    private Vector3 startPoint;
    private float timeSinceShot;

    public List<BlackHole> blackHoles;

    public float totalLife;
    public float lifeLeft;
    public float maxSize;

    public int colorID;

    public GameObject explosionEffect;
    public GameObject reflectEffect;
    public CameraShake camshake;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        camshake = Camera.main.GetComponent<CameraShake>();

        // Initialize the line renderer points
        lineRenderer.positionCount = subdivisions + 1;

        for(int i = 0; i < subdivisions+1; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(0, -5, 0));
        }
        

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        startPoint = transform.position;
        //velocity = transform.forward * shootSpeed;
        timeSinceShot = 0f;

        lifeLeft = totalLife;
    }

    private void Update()
    {
        timeSinceShot += Time.deltaTime;

        startPoint += velocity * Time.deltaTime;

        // Update the velocity based on black holes
        foreach (BlackHole blackHole in blackHoles)
        {
            float distance = Vector3.Distance(startPoint, blackHole.transform.position);
            if (distance < blackHole.radius) // Assume effectRadius defines "too close"
            {
                Vector3 directionToBlackHole = (blackHole.transform.position - startPoint).normalized;
                velocity += directionToBlackHole * blackHole.gravity * Time.deltaTime;
            }
        }

        UpdateLinePositions();

        CheckCollider();

        lifeLeft -= Time.deltaTime;

        float size = Mathf.Lerp(0f, maxSize, lifeLeft / totalLife);

        lineRenderer.startWidth = size;
        lineRenderer.endWidth = size;

        if(lifeLeft < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateLinePositions()
    {
        Vector3 simulatedPosition = startPoint;
        Vector3 simulatedVelocity = velocity;

        // Distance between points
        float segmentLength = beamLength / subdivisions;

        for (int i = 0; i <= subdivisions; i++)
        {
            simulatedPosition.y = -0.25f;
            lineRenderer.SetPosition(i, simulatedPosition);

            // Simulate the effect of black holes on this segment
            foreach (BlackHole blackHole in blackHoles)
            {
                float distance = Vector3.Distance(simulatedPosition, blackHole.transform.position);
                if (distance < blackHole.radius)
                {
                    Vector3 directionToBlackHole = (blackHole.transform.position - simulatedPosition).normalized;
                    simulatedVelocity += directionToBlackHole * blackHole.gravity * (segmentLength / shootSpeed);
                }
            }

            simulatedPosition += simulatedVelocity.normalized * segmentLength;
        }
    }

    public void CheckCollider()
    {
        Vector3 direction = velocity.normalized;

        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, beamLength))
        {
            if (hit.collider.gameObject.tag == "Target")
            {
                /*hit.collider.gameObject.GetComponent<MeshRenderer>().materials[1].color = lineRenderer.startColor;
                hit.collider.gameObject.GetComponent<MeshRenderer>().materials[1].SetColor("_EmissionColor", lineRenderer.startColor * 3f);*/
                hit.collider.gameObject.GetComponent<Target>().ChangeColor(colorID);
                Destroy(gameObject);
                Instantiate(explosionEffect, hit.point, Quaternion.identity);
                camshake.StartShake();
            }
            else if (hit.collider.gameObject.tag == "BlackHole")
            {
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.tag == "Prism")
            {
                hit.collider.gameObject.GetComponent<Prism>().SpawnNewBeams(velocity, blackHoles);
                Destroy(gameObject);
                Instantiate(reflectEffect, hit.point, Quaternion.identity);
            }
            else if (hit.collider.gameObject.tag == "Mirror")
            {
                // Calculate reflection direction
                Vector3 incomingDirection = velocity.normalized;
                Vector3 reflectDirection = Vector3.Reflect(incomingDirection, hit.normal);

                // Update velocity to the new direction
                velocity = reflectDirection.normalized * shootSpeed;

                startPoint = hit.collider.gameObject.transform.position;

                // Since we're reflecting the beam, we should also update its start point to the hit point to avoid immediate recollision
                //startPoint = hit.point;
                UpdateLinePositions();
                Instantiate(reflectEffect, hit.point, Quaternion.identity);
            }
            else if (hit.collider.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
                Instantiate(explosionEffect, hit.point, Quaternion.identity);
                camshake.StartShake();
            }
            else if(hit.collider.gameObject.tag == "Accel")
            {
                Destroy(gameObject);
                Instantiate(reflectEffect, hit.point, Quaternion.identity);
                hit.collider.gameObject.GetComponent<AccelController>().AddToAccel();
            }
        }
    }
}
