using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnlyIfClose : MonoBehaviour
{
    public float dist;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        sr.enabled = Vector2.Distance(Camera.main.transform.position, transform.position) < dist;
    }
}
