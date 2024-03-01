using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves The Player Based On Camera Rotation //

public class MovementController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;
    public Transform cameraTransform;

    float horizontal;
    float vertical;

    void Start()
    {
        // Get Rigidbody //
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get Input //
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

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
        Vector3 movement = movementDirection * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotate The Player To Face The Direction Of Movement, Only If Moving //
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * 500f);
        }
    }
}
