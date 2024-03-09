using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    Input input;

    public float rotateAngle;
    public float targetAngle;
    public float smoothSpeed;

    public Transform rotateTarget;

    public bool isPlayer;

    public GameObject ui;

    private void Awake()
    {
        input = new Input();

        input.Gameplay.Rotate.performed += ctx => ChangeAngle(rotateAngle * (int)ctx.ReadValue<float>());
    }

    private void Update()
    {
        if(isPlayer)
        {
            rotateTarget.rotation = Quaternion.Slerp(rotateTarget.rotation, Quaternion.Euler(0, targetAngle, 0), smoothSpeed * Time.deltaTime);

            if(ui)
            {
                ui.SetActive(true);
            }
        }
        else
        {
            if (ui)
            {
                ui.SetActive(false);
            }
        }
    }

    void ChangeAngle(float angle)
    {
        if(isPlayer)
        {
            targetAngle += angle;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayer = false;
        }
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
