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

    private Vector3 velocity;
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

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;

        startPoint = transform.position;
        velocity = transform.forward * shootSpeed;
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
                if (distance < blackHole.radius) // assuming blackHole.radius is a significant distance for gravity effect
                {
                    Vector3 directionToBlackHole = (blackHole.transform.position - simulatedPosition).normalized;
                    // Adjust the velocity based on the black hole's gravity
                    // Note: This simplistic approach assumes the effect is instantaneous and linear, which might not be physically accurate but should work for a game.
                    simulatedVelocity += directionToBlackHole * blackHole.gravity * (segmentLength / shootSpeed);
                }
            }

            // Update the simulated position for the next segment
            simulatedPosition += simulatedVelocity.normalized * segmentLength;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hit");

        // If Beam Hits Target //
        if(other.gameObject.tag == "Target")
        {
            // Set Target To True //

            // Instantiate Effect //

            // Create Sound //

            // Destroy Lightbeam //
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "BlackHole")
        {
            // Intantiate Effect //

            // Create Sound //

            // Destroy Lightbeam //
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Prism")
        {
            print("YESSSSS");
            // Trigger Split Of Light //
            other.gameObject.GetComponent<Prism>().SpawnNewBeams(velocity);
            // Intantiate Effect //

            // Create Sound //

            // Destroy Lightbeam //
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Mirror")
        {
            // Trigger Bounce Of Light //

            // Instantiate Effect //

            // Create Sound //

            // Destroy Lightbeam //
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "Wall")
        {
            // Instantiate Effect //

            // Create Sound //

            // Destroy Lightbeam //
            Destroy(gameObject);
        }
    }
}
