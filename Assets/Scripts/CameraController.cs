using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float targetAngle = 45f;
    public float currentAngle = 0f;
    public float mouseSensitivity = 2f;
    public float joystickSensitivity = 2f;
    public float rotationSpeed = 5f;

    public Transform target;
    public PlayerController playerController;

    public float smoothSpeed = 0.125f;

    Input input;
    Vector2 aimController;
    Vector2 aimMouse;
    public bool camButton;

    private void Awake()
    {
        input = new Input();

        input.Gameplay.Aim.performed += ctx => aimController = ctx.ReadValue<Vector2>();
        input.Gameplay.AimDelta.performed += ctx => aimMouse = ctx.ReadValue<Vector2>();
        input.Gameplay.CameraButton.performed += ctx => camButton = true;
        input.Gameplay.CameraButton.canceled += ctx => camButton = false;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void Update()
    {
        // Get Input //
        float mouseX = 0f;
        float mouseY = 0f;

        if (playerController.isGamepad)
        {
            mouseX = aimController.x;
            mouseY = aimController.y;
        }
        else
        {
            mouseX = aimMouse.x;
            mouseY = aimMouse.y;
        }


        // If Right Mouse Button, Rotate Camera //
        if (camButton)
        {
            if (playerController.isGamepad)
            {
                targetAngle += mouseX * joystickSensitivity;
            }
            else
            {
                targetAngle += mouseX * mouseSensitivity;
            }
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

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }
}
