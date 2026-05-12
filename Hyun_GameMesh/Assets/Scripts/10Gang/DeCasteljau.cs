using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class DeCasteljau : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    List<Vector3> pointPositions = new List<Vector3>();

    float timeValue = 0f;

    private void Awake()
    {
        foreach (var pt in points)
        {
            if (pt != null) pointPositions.Add(pt.position);
        }
    }

    private void Update()
    {
        timeValue += Time.deltaTime / 2f;
        transform.position = De(pointPositions, timeValue);
    }

    Vector3 De(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;

            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
            {
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            }
            p = next;
        }
        return p[0];
    }
}

