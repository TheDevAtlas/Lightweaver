using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public MeshRenderer rend;

    public Material[] colors;
    // 0 is default
    // 1 - red, 2 - green, 3 - blue
    // 4 - white

    public int state;
    public int targetState;

    private void Awake()
    {
        ChangeColor(state);
    }

    public void ChangeColor(int s)
    {
        print(s);
        state = s;
        rend.materials[1].color = colors[s].color;
        rend.materials[1].SetColor("_EmissionColor", colors[s].color * 3f);
        /*switch (s)
        {
            case 0:
                rend.materials[1] = defaultColor;
                break;
            case 1:
                rend.materials[1] = redColor;
                break;
            case 2:
                rend.materials[1] = greenColor;
                break;
            case 3:
                rend.materials[1] = blueColor;
                break;
            case 4:
                rend.materials[1] = whiteColor;
                break;
        }*/
    }
}
