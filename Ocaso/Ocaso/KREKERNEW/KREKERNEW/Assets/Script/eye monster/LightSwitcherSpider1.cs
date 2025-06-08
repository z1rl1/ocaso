using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwitcherSpider1 : MonoBehaviour
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
            lightComponent.intensity = 0.39f;
            yield return new WaitForSeconds(8f);

            lightComponent.intensity = 0f;
            yield return new WaitForSeconds(0.1f);

            lightComponent.intensity = 0.39f;
            yield return new WaitForSeconds(3f);

            lightComponent.intensity = 0f;
            yield return new WaitForSeconds(0.1f);

        }
    }
}
