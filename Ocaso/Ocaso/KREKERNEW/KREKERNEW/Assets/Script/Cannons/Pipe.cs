using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform exitPoint; // Точка выхода трубы
    public float suctionForce = 5f; // Сила засасывания
    public float moveDuration = 2f; // Время, за которое персонаж пройдет через трубу
    public float shootDelay = 1f; // Задержка перед выстрелом после выхода из трубы
    public GameObject character; // Персонаж
    public Transform firePoint; // Точка выстрела
    public float shootForce = 100f; // Сила выстрела
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
        Debug.Log("Начинается засасывание...");

        // Плавное перемещение к центру трубы
        Vector3 startPosition = player.position;
        Vector3 endPosition = transform.position; // Центр трубы
        float elapsedTime = 0f;

        while (elapsedTime < suctionForce)
        {
            float t = elapsedTime / suctionForce;
            player.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.position = endPosition;

        Debug.Log("Перемещение через трубу началось.");
        PlaySound();

        // Перемещение внутри трубы
        yield return StartCoroutine(MoveThroughPipe(player));

        Debug.Log("Перемещение через трубу завершено.");
        //cameraController.enabled = false;

        // Задержка перед выстрелом
        Debug.Log($"Задержка перед выстрелом: {shootDelay} секунд.");
        yield return new WaitForSeconds(shootDelay);

        Debug.Log("Выстрел!");

        // Выстрел персонажа
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

            // Начинаем тряску камеры
            cameraController.shakeMagnitude = shakeMagnitude;
            cameraController.shakeDuration = shakeDuration;
            cameraController.ShakeCamera();

            cameraController.enabled = true;
            heroController.enabled = true;
        }
        else
        {
            Debug.LogWarning("Персонаж не назначен!");
        }
    }
}