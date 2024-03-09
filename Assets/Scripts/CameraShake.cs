using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float speed;
    public int elapsed = 0;
    public float x;
    public float y;
    public float z = -35f;

    public float startY;

    private void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, startY,transform.localPosition.z);
    }

    public void StartShake()
    {
        StartCoroutine(Shake(0.1f, 0.4f));
    }
    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 origionalPos = transform.localPosition;

        elapsed = 0;

        while(elapsed < 2)
        {
            x = Random.Range(-1,1f) * magnitude;
            y = Random.Range(-1,1f) * magnitude;

            //transform.localPosition = new Vector3(x, y, -35);
            elapsed++;
            yield return new WaitForSeconds(0.1f);
        }

        //transform.localPosition = new Vector3(0f,0f,-35f);
        x = 0;
        y = 0;
        z = -35f;
    }

    private void Update()
    {
        //transform.localPosition = new Vector3(x, y, -35);

        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(x, y, z), speed * Time.deltaTime);
    }
}
