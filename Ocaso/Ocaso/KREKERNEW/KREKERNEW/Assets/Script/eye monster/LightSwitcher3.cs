using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class LightSwitcher3 : MonoBehaviour
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
            lightComponent.intensity = 209.5f;
            yield return new WaitForSeconds(4f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(0.2f);

            lightComponent.intensity = 209.5f;
            yield return new WaitForSeconds(2f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(0.1f);


            lightComponent.intensity = 209.5f;
            yield return new WaitForSeconds(6f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(2f);

            lightComponent.intensity = 209.5f;
            yield return new WaitForSeconds(1f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(0.2f);
        }
    }
}