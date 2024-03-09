using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelController : MonoBehaviour
{
    public ParticleAccel accelerator;

    public void AddToAccel()
    {
        if(accelerator.powerMult <= 25)
        {
            accelerator.powerMult += 5;
        }
    }
}
