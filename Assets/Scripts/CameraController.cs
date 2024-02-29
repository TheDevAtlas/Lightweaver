using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Camera Snaps To 45 Degrees //
// Move Camera With Right Mouse Click //
// Smoothing //

// Stolen from MrPlague on YouTube //
// https://www.youtube.com/watch?v=f8foNx-Qge0 //

public class CameraController : MonoBehaviour
{
    public float targetAngle = 45f;
    public float currentAngle = 0f;
    public float mouseSensitivity = 2f;
    public float rotationSpeed = 5f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // If Right Click, Move Camera //
        // If Not, Snap To 45 Degrees  //
        if(Input.GetMouseButton(1))
        {
            targetAngle += mouseX * mouseSensitivity;
        }
        else
        {
            targetAngle = Mathf.Round(targetAngle / 45);
            targetAngle *= 45;
        }

        // Keep Angle Within 0 - 360 ///
        if(targetAngle < 0)
        {
            targetAngle += 360;
        }
        if(targetAngle > 360)
        {
            targetAngle -= 360;
        }

        // Set Angle Of Camera Pivot //
        currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(30, currentAngle, 0);
    }
}
