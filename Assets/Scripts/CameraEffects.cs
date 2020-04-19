using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public float amount = 1.0f;
    public float duration = 0.2f;
    public float zoom = 9.0f;
    public Transform sun;
    public AnimationCurve focusCurve;

    Camera cam;
    float originalZoom;
    Vector3 originalPos;
    float timer = 0;
    bool focusing = false;
    float focusTimer = 0;

    void Start()
    {
        originalPos = transform.localPosition;
        originalZoom = Camera.main.orthographicSize;
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (focusing)
        {
            transform.SetParent(null);
            focusTimer += Time.deltaTime * 0.05f;
            if (focusTimer > 1.0f) focusTimer = 1.0f;
            transform.position = Vector3.Lerp(transform.position, new Vector3(sun.position.x, sun.position.y, transform.position.z), focusCurve.Evaluate(focusTimer));
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 180.0f, focusCurve.Evaluate(focusTimer));
        }
        else
        {
            timer -= Time.deltaTime;

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalZoom, Time.deltaTime * 0.5f);

            if (timer > 0)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos + (Vector3)Random.insideUnitCircle * amount, Time.deltaTime * 20);
            }
            else
            {
                transform.localPosition = originalPos;
            }
        }
    }

    public void Shake()
    {
        if (timer > duration) return;
        timer = duration;
        originalPos = transform.localPosition;
        cam.orthographicSize = zoom;
        FindObjectOfType<Music>().HitPitch();
    }

    public void FocusSun()
    {
        focusing = true;
    }
}
