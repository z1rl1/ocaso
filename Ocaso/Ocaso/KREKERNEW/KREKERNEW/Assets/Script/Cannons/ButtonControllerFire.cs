using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerFire : MonoBehaviour
{
    public float dropAmount = 0.5f; // ������� ������ ��������� ���� ��� �������
    public CannonControllerlvl5 cannonToFire; // ������ �� �����, ������� ����� ��������
    private Vector3 originalPosition;
    private bool isPressed = false;
    private bool hasFired = false; // ����, ����� ������� �� ���, ���� �� ��� ��������
    private AudioSource audioSource; // ��� ��������������� �����

    private void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasFired && collision.gameObject.CompareTag("Character"))
        {
            hasFired = true; // ������������� ����, ��� ������ ��� ���� ������������
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

        // ��������� ��������
        if (cannonToFire != null)
        {
            cannonToFire.Fire();
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
}
