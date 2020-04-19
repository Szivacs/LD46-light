using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUEShifter : MonoBehaviour
{
    public SpriteRenderer sr;
    public Gradient hue;
    public float speed;
    float offset;

    void Update()
    {
        offset += Time.deltaTime * speed;

        sr.color = hue.Evaluate(Mathf.PerlinNoise(offset, 0));
    }
}
