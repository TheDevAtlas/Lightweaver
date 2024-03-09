using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera cameraToLookAt;

    private void Start()
    {
        cameraToLookAt = Camera.main;
    }

    void Update()
    {
        if (cameraToLookAt != null)
        {
            transform.LookAt(cameraToLookAt.transform);
        }
    }
}
