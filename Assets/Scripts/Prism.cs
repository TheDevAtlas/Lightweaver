using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : MonoBehaviour
{
    public Color leftBeam;
    public Color middleBeam;
    public Color rightBeam;

    public float degrees;

    public Lightbeam lightBeamAsset;

    // Spawn 3 New Beams Based On Input Velocity //
    public void SpawnNewBeams(Vector3 inputVelocity)
    {
        Vector3 position;
        Vector3 velocity;

        // One beam x degrees to the left //
        position = Vector2.zero;
        velocity = new Vector3(10, 0, 0);
        CreateBeam(position, velocity, leftBeam);

        // One beam 0 degrees //
        position = Vector2.zero;
        velocity = new Vector3(0, 0, 10);
        CreateBeam(position, velocity, middleBeam);

        // One beam x degrees to the right //
        position = Vector2.zero;
        velocity = new Vector3(-10, 0, 0);
        CreateBeam(position, velocity, rightBeam);
    }

    void CreateBeam(Vector3 position, Vector3 rotation, Color c)
    {
        // Instantiate Lightbeam at the position and rotation of this controller
        Lightbeam newBeam = Instantiate(lightBeamAsset, position, Quaternion.Euler(rotation));

        // Set initial Lightbeam settings
        newBeam.shootSpeed = 10f;
        newBeam.beamLength = 10f;

        newBeam.lineRenderer.startColor = c;
        newBeam.lineRenderer.endColor = c;
    }
}
