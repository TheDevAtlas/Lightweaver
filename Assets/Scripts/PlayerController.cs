using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float rotateSmoothing = 0.125f;
    public Transform cameraTransform;

    [Header("Controller Settings")]
    public bool isGamepad;
    public Vector2 movement;
    public Vector2 aim;

    Input input;
    Rigidbody rb;

    void Awake()
    {
        // Reference Our Input Actions //
        input = new Input();

        // If Performed, Set To Value //
        input.Gameplay.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.Gameplay.Aim.performed += ctx => aim = ctx.ReadValue<Vector2>();

        // If No Input, Set To Zero //
        input.Gameplay.Movement.canceled += ctx => movement = Vector2.zero;
        input.Gameplay.Aim.canceled += ctx => aim = Vector2.zero;

        // Shoot Control //
        input.Gameplay.Shoot.performed += ctx => GetComponent<ShootController>().Shoot();

        // Get Rigidbody //
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        // Get Input //
        float horizontal = movement.x;
        float vertical = movement.y;

        // Calculate Movement Direction Based On Camera //
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Ensure Movement Is Strictly Horizontal //
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Create The Movement Direction Vector //
        Vector3 movementDirection = (forward * vertical + right * horizontal).normalized;

        // Apply The Movement //
        Vector3 move = movementDirection * speed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    void HandleRotation()
    {
        // Rotations Handled Differently Based On Rotation //
        if(isGamepad)
        {
            Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

            playerDirection = Quaternion.Euler(0, cameraTransform.localEulerAngles.y, 0) * playerDirection;

            if (playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newRotation = Quaternion.LookRotation(playerDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(aim);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if(groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    void LookAt(Vector3 lookPoint)
    {
        lookPoint.y = transform.transform.position.y;
        transform.LookAt(lookPoint);
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Controller") ? true : false;
    }
}
