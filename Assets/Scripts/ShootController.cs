using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    public Lightbeam lightBeamAsset; // Reference to the Lightbeam prefab

    public float shootSpeed = 10f;
    public float beamLength = 5f;

    private Lightbeam currentBeam; // To keep track of the instantiated light beam

    public List<BlackHole> blackHoles;

    private void Update()
    {
        // If the mouse button is pressed, shoot a Lightbeam
        if (Input.GetMouseButtonDown(0) && currentBeam == null) // Check if there's no current beam to avoid instantiating multiple beams at once
        {
            // Instantiate Lightbeam at the position and rotation of this controller
            Lightbeam newBeam = Instantiate(lightBeamAsset, transform.position, transform.rotation);

            // Set initial Lightbeam settings
            newBeam.shootSpeed = this.shootSpeed;
            newBeam.beamLength = 0f; // Start with a length of 0, which will be increased while the mouse button is held down

            newBeam.blackHoles = blackHoles;

            currentBeam = newBeam; // Keep a reference to the newly created beam
        }

        // If the mouse button is held down, add length to the Lightbeam
        if (Input.GetMouseButton(0) && currentBeam != null)
        {
            // Increase the length of the current beam up to the maximum beam length
            currentBeam.beamLength = Mathf.Min(currentBeam.beamLength + (shootSpeed * Time.deltaTime), beamLength);
            //currentBeam.UpdateLinePositions(); // You may need to make this method public in your Lightbeam script
        }

        // If the mouse button is released, clear the current beam to allow for a new one to be instantiated on the next click
        if (Input.GetMouseButtonUp(0) && currentBeam != null)
        {
            currentBeam = null; // Ready to shoot a new beam
        }
    }
}
