using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public Animator blackScreen;

    private void Start()
    {
        StartCoroutine(DelayedLoad());
    }

    IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(15.0f);
        blackScreen.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }
}
