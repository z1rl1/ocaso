using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpeedTrigger : MonoBehaviour
{
    public float newCameraSpeed = 10f; // ���������� �������� �������� ������
    private CameraController cameraController; // ������ �� ���������� ������

    private void Start()
    {
        cameraController = FindObjectOfType<CameraController>(); // ������� ���������� ������
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            if (cameraController != null)
            {
                cameraController.SetCameraSpeed(newCameraSpeed); // ���������� ����� ��������
            }
        }
    }
}
