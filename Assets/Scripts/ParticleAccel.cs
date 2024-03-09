using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAccel : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float baseOffset;

    public List<Transform> points = new List<Transform>();

    public float amplitude = 0.05f;
    public float frequency = 5f;

    private float timeOffset;

    public float powerMult = 1f;

    void Start()
    {
        lineRenderer.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i].position * baseOffset);
        }

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        timeOffset = Random.Range(0f, 1000f);
    }

    void FixedUpdate()
    {
        float time = Time.time + timeOffset;

        for (int i = 0; i < points.Count; i++)
        {
            float sineOffset = Mathf.Sin(time * frequency * powerMult + i) * amplitude;
            float currentOffset = baseOffset + sineOffset;
            Vector3 newPosition = points[i].position * currentOffset;

            lineRenderer.SetPosition(i, Vector3.Lerp(lineRenderer.GetPosition(i), newPosition, Time.deltaTime * 25f));
        }

        lineRenderer.startWidth = Mathf.Lerp(lineRenderer.startWidth, ((Mathf.Sin(time * frequency + -1f)+1f) * amplitude) * powerMult, Time.deltaTime * 25f);
        lineRenderer.endWidth = Mathf.Lerp(lineRenderer.endWidth, ((Mathf.Sin(time * frequency + 1f)+1f) * amplitude) * powerMult, Time.deltaTime * 25f);
    }
}