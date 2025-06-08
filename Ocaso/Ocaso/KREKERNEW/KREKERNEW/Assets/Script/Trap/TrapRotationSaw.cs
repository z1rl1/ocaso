using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRotationSaw : MonoBehaviour
{
    public float dropAmount = 0.5f; // ������� ������ ��������� ���� ��� �������
    public Transform objectToRotate; // ������, ������� ����� �������
    public float rotationAmount = 45f; // ������� �������� ����� �������� ��� ������ �� �������
    private Vector3 originalPosition;
    private bool isPressed = false;
    private bool hasRotated = false; // ����, ����� ������� �� ���, ���� �� ��� ��������� �������
    private AudioSource audioSource; // ��� ��������������� �����
    public float rotationDuration = 1f; // ����� ����������������� ��������
    public float accelerationDuration = 0.2f; // �����, �� ������� ������ �������� ��������
    public float decelerationDuration = 0.2f; // �����, �� ������� ������ �����������


    private void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasRotated && collision.gameObject.CompareTag("Character"))
        {
            hasRotated = true; // ������������� ����, ��� ������ ��� ���� ������������
            StartCoroutine(HandleButtonPress());
        }
    }

    private IEnumerator HandleButtonPress()
    {
        // ������������� ����
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // �������� ������
        Vector3 pressedPosition = originalPosition - new Vector3(0, dropAmount, 0);
        float elapsedTime = 0f;
        float duration = 0.2f; // ����� ��������

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, pressedPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = pressedPosition;

        // ������� ������
        if (objectToRotate != null)
        {
            StartCoroutine(RotateObject());
        }

        // ���������� ������ �� �������� ���������
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(pressedPosition, originalPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }

    private IEnumerator RotateObject()
    {
        float startRotation = objectToRotate.eulerAngles.z;
        float endRotation = startRotation + rotationAmount;
        float elapsedTime = 0f;

        // ����� �� ���������� ������ ��������
        float accelerationTime = Mathf.Min(accelerationDuration, rotationDuration);
        float decelerationTime = Mathf.Max(0, rotationDuration - accelerationTime);

        // ������ �������, ����� ������ ����������
        while (elapsedTime < accelerationTime)
        {
            float t = elapsedTime / accelerationTime;
            float speed = Mathf.SmoothStep(0, 1, t); // ���������
            float newRotationZ = Mathf.Lerp(startRotation, endRotation, speed);
            objectToRotate.eulerAngles = new Vector3(0, 0, newRotationZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }




    }
}
