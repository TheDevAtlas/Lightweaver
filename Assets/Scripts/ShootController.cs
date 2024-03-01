using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    public Lightbeam lightBeamAsset; // Reference to the Lightbeam prefab

    public float shootSpeed = 10f;
    public float beamLength = 5f;

    public List<BlackHole> blackHoles;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Lightbeam newBeam = Instantiate(lightBeamAsset, transform.position, transform.rotation);

            newBeam.shootSpeed = this.shootSpeed;
            newBeam.beamLength = 1f;

            newBeam.blackHoles = blackHoles;
            newBeam.velocity = newBeam.transform.forward * shootSpeed;
        }
    }
}
