using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControllerBigStone : MonoBehaviour
{
    public GameObject rock;
    public Transform startPosition;
    public float resetTime = 3f;
    public AudioClip spawnSound;
    public AudioSource audioSource; 
    private Rigidbody2D rockRb;

    private void Start()
    {
        // Получаем Rigidbody2D компонента камня
        rockRb = rock.GetComponent<Rigidbody2D>();
    }
    private void PlaySpawnSound()
    {
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что в триггер вошел персонаж
        if (other.CompareTag("Character"))
        {
            StartRockMovement();
            PlaySpawnSound();
        }
    }

    private void StartRockMovement()
    {
        // Включаем движение камня
        rockRb.simulated = true;

        // Запускаем корутину для перезагрузки камня
        StartCoroutine(ResetRock());
    }

    private IEnumerator ResetRock()
    {
        // Ждем указанное время
        yield return new WaitForSeconds(resetTime);

        // Отключаем симуляцию Rigidbody2D
        rockRb.simulated = false;

        // Перемещаем камень в начальное положение
        rock.transform.position = startPosition.position;

    }
}
