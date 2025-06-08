using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpeedTrigger : MonoBehaviour
{
    public float newCameraSpeed = 10f; // Установите желаемую скорость камеры
    private CameraController cameraController; // Ссылка на контроллер камеры

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>(); // Найдите контроллер камеры
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            if (cameraController != null)
            {
                cameraController.SetCameraSpeed(newCameraSpeed); // Установите новую скорость
            }
        }
    }
}
