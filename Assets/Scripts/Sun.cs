using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public GameObject shineEffect;
    public Animation anim;

    public void Shine()
    {
        Instantiate(shineEffect, transform.position + Vector3.forward, Quaternion.identity, transform);
        //anim.Play("sun_shine");
    }
}
