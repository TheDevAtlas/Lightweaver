using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;

public class Prism : MonoBehaviour
{
    public Color leftBeam;
    public Color middleBeam;
    public Color rightBeam;

    public float degrees;

    public Lightbeam lightBeamAsset;

    public float shootSpeed = 10f;

    // Spawn 3 New Beams Based On Input Velocity //
    public void SpawnNewBeams(Vector3 inputVelocity, List<BlackHole> holes)
    {

        // One beam x degrees to the left //
        CreateBeam(inputVelocity, leftBeam, -15f, holes);

        // One beam 0 degrees //
        CreateBeam(inputVelocity, middleBeam, 0f, holes);

        // One beam x degrees to the right //
        CreateBeam(inputVelocity, rightBeam, 15f, holes);
    }

    void CreateBeam(Vector3 velocity, Color c, float angle, List<BlackHole> holes)
    {
        Quaternion rotation = Quaternion.LookRotation(velocity);
        rotation *= Quaternion.Euler(new Vector3(0, angle, 0));
        // Instantiate Lightbeam at the position and rotation of this controller
        Lightbeam newBeam = Instantiate(lightBeamAsset, transform.position, rotation);

        // Set initial Lightbeam settings
        newBeam.shootSpeed = 10f;
        newBeam.beamLength = 1f;
        newBeam.velocity = newBeam.transform.forward * shootSpeed;

        newBeam.lineRenderer.startColor = c;
        newBeam.lineRenderer.endColor = c;
        newBeam.lineRenderer.material.color = c;
        newBeam.lineRenderer.material.SetColor("_EmissionColor", c * 3f);

        newBeam.blackHoles = holes;
    }
}
