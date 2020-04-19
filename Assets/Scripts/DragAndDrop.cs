using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public float force = 100.0f;
    Rigidbody2D rb;
    bool dragging = false;

    Vector3 originalPos;
    Vector3 originalMousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (dragging == false) return;

        Vector3 mousePositionOffset = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)) - originalMousePos;
        rb.velocity = (originalPos + mousePositionOffset - transform.position) * force * Time.fixedDeltaTime;
    }

    private void OnMouseDown()
    {
        dragging = true;
        originalPos = transform.position;
        originalMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
}
