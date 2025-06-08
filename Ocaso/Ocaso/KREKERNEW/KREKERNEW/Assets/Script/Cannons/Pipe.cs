using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform exitPoint; // ����� ������ �����
    public float suctionForce = 5f; // ���� �����������
    public float moveDuration = 2f; // �����, �� ������� �������� ������� ����� �����
    public float shootDelay = 1f; // �������� ����� ��������� ����� ������ �� �����
    public GameObject character; // ��������
    public Transform firePoint; // ����� ��������
    public float shootForce = 100f; // ���� ��������
    private CameraController cameraController;
    private Hero heroController;

    public float shakeMagnitude = 1f;
    public float shakeDuration = 0.45f;
    public AudioClip spawnSound;
    public AudioSource audioSource; 

    private void Start()
    {

        cameraController = FindObjectOfType<CameraController>();
        heroController = FindObjectOfType<Hero>();
    }

    private void PlaySound()
    {
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            heroController.enabled = false;
            StartCoroutine(SuctionAndMovePlayer(collision.transform));
        }
    }



    private IEnumerator SuctionAndMovePlayer(Transform player)
    {
        Debug.Log("���������� �����������...");

        // ������� ����������� � ������ �����
        Vector3 startPosition = player.position;
        Vector3 endPosition = transform.position; // ����� �����
        float elapsedTime = 0f;

        while (elapsedTime < suctionForce)
        {
            float t = elapsedTime / suctionForce;
            player.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = endPosition;

        Debug.Log("����������� ����� ����� ��������.");
        PlaySound();

        // ����������� ������ �����
        yield return StartCoroutine(MoveThroughPipe(player));

        Debug.Log("����������� ����� ����� ���������.");
        //cameraController.enabled = false;

        // �������� ����� ���������
        Debug.Log($"�������� ����� ���������: {shootDelay} ������.");
        yield return new WaitForSeconds(shootDelay);

        Debug.Log("�������!");

        // ������� ���������
        ShootPlayer();
    }

    private IEnumerator MoveThroughPipe(Transform player)
    {
        Vector3 startPosition = player.position;
        Vector3 endPosition = exitPoint.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            player.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = endPosition;
    }

    private void ShootPlayer()
    {
        if (character != null)
        {
            character.transform.position = firePoint.position;

            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(firePoint.right * shootForce, ForceMode2D.Impulse);
            }

            // �������� ������ ������
            cameraController.shakeMagnitude = shakeMagnitude;
            cameraController.shakeDuration = shakeDuration;
            cameraController.ShakeCamera();

            cameraController.enabled = true;
            heroController.enabled = true;
        }
        else
        {
            Debug.LogWarning("�������� �� ��������!");
        }
    }
}