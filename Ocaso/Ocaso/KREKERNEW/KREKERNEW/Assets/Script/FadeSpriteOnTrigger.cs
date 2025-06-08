using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSpriteOnTrigger : MonoBehaviour
{
    public List<SpriteRenderer> spriteRenderers; // Список спрайтов для работы

    public float fadeDuration = 2.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            StartCoroutine(FadeOutSprites());
        }
    }

    private IEnumerator FadeOutSprites()
    {
        List<Color> initialColors = new List<Color>();
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            if (renderer != null)
            {
                initialColors.Add(renderer.color);
            }
        }

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float t = elapsedTime / fadeDuration;
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                if (renderer != null)
                {
                    Color initialColor = initialColors[spriteRenderers.IndexOf(renderer)];
                    renderer.color = Color.Lerp(initialColor, new Color(initialColor.r, initialColor.g, initialColor.b, 0f), t);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            if (renderer != null)
            {
                Color color = renderer.color;
                renderer.color = new Color(color.r, color.g, color.b, 0f);
            }
        }
    }
}
