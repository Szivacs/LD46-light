using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class Orb : MonoBehaviour
{
    public Path path;
    public float speed = 0.2f;
    public bool follow = true;
    [Space(10)]
    public Light2D pointLight;
    public float health = 100.0f;
    public float hitDamage = 10.0f;
    public float hitForce = 10.0f;
    [Space(10)]
    public Transform orb;
    public GameObject deathEffect;
    public Animator blackScreen;
    public Animator helpText;
    public Animator credits;
    public AudioClip hitSound;
    public Generator generator;

    float timePerSegment;
    float time = 0.0f;
    int index = 0;
    float hitTimer = 0;
    float orbOffset;
    CircleCollider2D coll;

    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        helpText.SetTrigger("FadeIn");
        StartCoroutine(FadeOutHelpText());
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer -= Time.deltaTime;
        if (follow && index < path.points.Count - 1)
        {
            time += Time.deltaTime * speed;
            speed += Time.deltaTime * 0.003f;
            transform.position = path.Evaluate(index, index + 1, time);
            if (time > 1.0f)
            {
                time = 0.0f;
                index++;
                StartCoroutine(generator.GenerateSegment(index+1));
            }


            orbOffset += Time.deltaTime;
            float x = Mathf.PerlinNoise(orbOffset, 0) * 2 - 1;
            float y = Mathf.PerlinNoise(0, orbOffset) * 2 - 1;
            orb.localPosition = new Vector3(x, y, 0);
            coll.offset = orb.localPosition;
        }
    }

    IEnumerator FadeOutHelpText()
    {
        yield return new WaitForSeconds(10.0f);
        helpText.SetTrigger("FadeOut");
    }

    IEnumerator Die()
    {
        follow = false;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        orb.gameObject.SetActive(false);
        pointLight.intensity = 1.5f;
        yield return new WaitForSeconds(2.0f);
        blackScreen.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(9.0f);
        blackScreen.SetTrigger("FadeIn");
        credits.SetTrigger("Credits");
        yield return new WaitForSeconds(21.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hitTimer > 0.0f || follow == false) return;
        health -= hitDamage;
        hitTimer = 1.0f;
        Rigidbody2D crb = collision.gameObject.GetComponent<Rigidbody2D>();
        crb.AddForce(((Vector3)collision.contacts[0].point - transform.position) * hitForce, ForceMode2D.Impulse);
        crb.AddTorque(hitForce * 10.0f);
        Camera.main.transform.SendMessage("Shake");
        pointLight.intensity -= hitDamage / 100.0f;
        speed *= 0.75f;
        AudioSource.PlayClipAtPoint(hitSound, transform.position);

        if(health <= 0.0f)
        {
            StartCoroutine(Die());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //orb.gameObject.SetActive(false);
        //path.gameObject.SetActive(false);
        Camera.main.transform.SendMessage("FocusSun");
        collision.transform.SendMessage("Shine");
        StartCoroutine(EndGame());
    }
}
