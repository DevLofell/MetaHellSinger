using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{

    GameObject backImage;
    Image fadeImage;
    public float fadeDuration = 1.0f;
    private void Awake()
    {

    }

    void Start()
    {
        backImage = GameObject.Find("FadeImage");
        fadeImage = backImage.GetComponent<Image>();
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
        Destroy(gameObject, 1);
    }

    //public IEnumerator FadeOut()
    //{
    //    float elapsedTime = 0f;
    //    Color color = fadeImage.color;
    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
    //        fadeImage.color = color;
    //        yield return null;
    //    }
    //    fadeImage.color = new Color(color.r, color.g, color.b, 1.0f);
    //}
}
