using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1.0f;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = fadeImage.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1.0f, fadeSpeed));
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0.0f, fadeSpeed));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float targetAlpha, float duration)
    {
        float time = 0.0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            canvasGroup.alpha = alpha;
            time += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}