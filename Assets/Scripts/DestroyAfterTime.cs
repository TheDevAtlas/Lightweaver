using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float TimeToDestroy;

    private void Start()
    {
        Invoke("DestroyAfter", TimeToDestroy);
    }

    public void DestroyAfter()
    {
        Destroy(gameObject);
    }
}
