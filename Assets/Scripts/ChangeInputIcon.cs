using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInputIcon : MonoBehaviour
{
    public PlayerController controller;

    public Image uiImage;
    public Sprite gamepad;
    public Sprite keyboard;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(controller.isGamepad)
        {
            uiImage.sprite = gamepad;
        }
        else
        {
            uiImage.sprite = keyboard;
        }
    }
}
