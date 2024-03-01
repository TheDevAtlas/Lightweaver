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

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Initialize the line renderer points
        lineRenderer.positionCount = subdivisions + 1;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        startPoint = transform.position;
        //velocity = transform.forward * shootSpeed;
        timeSinceShot = 0f;
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
    }

    public void UpdateLinePositions()
    {
        Vector3 simulatedPosition = startPoint;
        Vector3 simulatedVelocity = velocity;

        // Distance between points
        float segmentLength = beamLength / subdivisions;

        for (int i = 0; i <= subdivisions; i++)
        {
            simulatedPosition.y = 0.75f;
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
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.tag == "BlackHole")
            {
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.tag == "Prism")
            {
                hit.collider.gameObject.GetComponent<Prism>().SpawnNewBeams(velocity, blackHoles);
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.tag == "Mirror")
            {
                // Calculate reflection direction
                Vector3 incomingDirection = velocity.normalized;
                Vector3 reflectDirection = Vector3.Reflect(incomingDirection, hit.normal);

                // Update velocity to the new direction
                velocity = reflectDirection.normalized * shootSpeed;

                // Since we're reflecting the beam, we should also update its start point to the hit point to avoid immediate recollision
                //startPoint = hit.point;
                UpdateLinePositions();
            }
            else if (hit.collider.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
        }
    }

}
