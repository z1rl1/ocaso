using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamDisable : MonoBehaviour
{
    public float shakeMagnitude = 1f;
    public float shakeDuration = 0.45f;
    public float disableDuration = 3f; // Время отключения тряски

    private CameraController cameraController;
    private bool isShakingEnabled = true; // Переменная для отслеживания состояния тряски

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character") && isShakingEnabled)
        {
            // Начинаем тряску камеры
            cameraController.shakeMagnitude = shakeMagnitude;
            cameraController.shakeDuration = shakeDuration;
            cameraController.ShakeCamera();

            // Отключаем тряску и запускаем корутину
            isShakingEnabled = false;
            StartCoroutine(EnableShakingAfterDelay(disableDuration));
        }
    }

    private IEnumerator EnableShakingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isShakingEnabled = true; // Включаем тряску обратно
    }
}
