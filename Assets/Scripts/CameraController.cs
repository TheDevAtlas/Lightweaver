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

    public Transform target;

    public float smoothSpeed = 0.125f;

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void Update()
    {
        // Get Input //
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // If Right Mouse Button, Rotate Camera //
        if (Input.GetMouseButton(1))
        {
            targetAngle += mouseX * mouseSensitivity;
        }
        // If No Right Button, Snap Target Rotation //
        else
        {
            targetAngle = Mathf.Round(targetAngle / 45);
            targetAngle *= 45;
        }

        // Keep Angles Within 0-360 //
        if (targetAngle < 0)
        {
            targetAngle += 360;
        }
        if (targetAngle > 360)
        {
            targetAngle -= 360;
        }

        // Set The Angles //
        currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(35, currentAngle, 0);
    }
}
