using System.Collections;
using UnityEngine;

public class FadeInImage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeSpeed = 1f / 8f; // Скорость изменения альфа-канала

    private float currentAlpha = 0f;
    private bool isFading = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(StartFadeIn());
    }

    IEnumerator StartFadeIn()
    {
        yield return new WaitForSeconds(13.5f);

        isFading = true;
    }

    void Update()
    {
        if (isFading)
        {
            currentAlpha += fadeSpeed * Time.deltaTime;
            currentAlpha = Mathf.Clamp01(currentAlpha);

            Color newColor = spriteRenderer.color;
            newColor.a = currentAlpha;

            spriteRenderer.color = newColor;

            if (currentAlpha >= 1f)
            {
                isFading = false;
            }
        }
    }
}