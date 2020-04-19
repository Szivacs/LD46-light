using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    public Vector3 speed = Vector3.one;

    float offset;

    private void Start()
    {
        offset = Random.Range(0, 100f);
    }

    void Update()
    {
        offset += Time.deltaTime;

        Vector3 rot = new Vector3(
            Mathf.PerlinNoise(offset, 0) * speed.x,
            Mathf.PerlinNoise(-offset * 2, 0) * speed.y,
            Mathf.PerlinNoise(-offset * 0.5f, offset * 0.5f) * speed.z
        );

        transform.Rotate(rot);
    }
}
