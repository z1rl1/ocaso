using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ShakeCamAfterNSec : MonoBehaviour
{
    public float shakeMagnitude = 1f;
    public float shakeDuration = 0.45f;
    public List<float> shakeTimes;

    private CameraController cameraController;
    private AudioSource audioSource; // Источник звука

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ShakeAfterDelays());
    }

    private IEnumerator ShakeAfterDelays()
    {
        foreach (float delay in shakeTimes)
        {
            yield return new WaitForSeconds(delay);

            cameraController.shakeMagnitude = shakeMagnitude;
            cameraController.shakeDuration = shakeDuration;
            cameraController.ShakeCamera();
            audioSource.Play();
        }
    }
}
