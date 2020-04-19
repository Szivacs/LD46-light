using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource source;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        source.volume += Time.deltaTime * 0.1f;
        if (source.volume > 0.5f)
            source.volume = 0.5f;

        source.pitch += Time.deltaTime * 0.5f;
        if (source.pitch > 1.0f)
            source.pitch = 1.0f;
    }

    public void HitPitch()
    {
        source.pitch = Random.Range(0.7f, 0.9f);
    }
}
