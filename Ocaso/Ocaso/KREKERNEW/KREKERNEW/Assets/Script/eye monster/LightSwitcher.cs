using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class LightSwitcher : MonoBehaviour
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
            lightComponent.intensity = 90f;
            yield return new WaitForSeconds(5f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(0.2f);

            lightComponent.intensity = 90f;
            yield return new WaitForSeconds(2f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(0.2f);


            lightComponent.intensity = 90f;
            yield return new WaitForSeconds(5f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(0.1f);

            lightComponent.intensity = 90f;
            yield return new WaitForSeconds(0.1f);

            lightComponent.intensity = 1f;
            yield return new WaitForSeconds(0.2f);
        }
    }
}