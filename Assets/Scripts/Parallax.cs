using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector2 diff = Camera.main.transform.position * transform.position.z;
        transform.position = new Vector3(startPos.x + diff.x, startPos.y + diff.y, startPos.z);
    }
}
