using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitcherSpider2 : MonoBehaviour
{
    private Light2D lightComponent;

    void Start()
    {
        lightComponent = GetComponent<Light2D>();
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            lightComponent.intensity = 0.31f;
            yield return new WaitForSeconds(5f);

            lightComponent.intensity = 0f;
            yield return new WaitForSeconds(0.2f);

            lightComponent.intensity = 0.31f;
            yield return new WaitForSeconds(6f);

            lightComponent.intensity = 0f;
            yield return new WaitForSeconds(0.1f);


        }
    }
}
