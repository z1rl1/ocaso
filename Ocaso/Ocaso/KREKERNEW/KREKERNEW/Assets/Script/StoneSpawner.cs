using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    public GameObject[] stonePrefabs; // Массив префабов камней
    public float delayBeforeDisablingColliders = 1f; // Время в секундах перед отключением коллайдеров
    public float timeBeforeDestroy = 3f; // Время в секундах до уничтожения камней
    public Transform spawnPoint; // Точка спауна
    public AudioClip spawnSound; // Звук, который будет проигрываться
    public AudioSource audioSource; // Компонент для воспроизведения звука
    private bool isTriggered = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character") && !isTriggered)
        {
            PlaySpawnSound(); // Проигрываем звук
            SpawnStones();
            isTriggered = true;
            StartCoroutine(DisableTrigger());
        }
    }

    private IEnumerator DisableTrigger()
    {
        yield return new WaitForSeconds(2f);
        isTriggered = false;
    }

    private void PlaySpawnSound()
    {
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }

    private void SpawnStones()
    {
        // Проверяем, что количество префабов и количество камней совпадает
        int numberOfStones = stonePrefabs.Length;

        for (int i = 0; i < numberOfStones; i++)
        {
            // Создаем камень в указанной точке спауна
            GameObject stone = Instantiate(stonePrefabs[i], spawnPoint.position, Quaternion.identity);

            // Задаем случайное начальное положение для камня
            stone.transform.position = new Vector2(spawnPoint.position.x + Random.Range(-1f, 1f), spawnPoint.position.y);

            // Запускаем корутину для управления камнями
            StartCoroutine(ManageStone(stone));
        }
    }

    private IEnumerator ManageStone(GameObject stone)
    {
        // Ожидаем перед отключением коллайдеров
        yield return new WaitForSeconds(delayBeforeDisablingColliders);

        // Отключаем коллайдеры
        Collider2D collider = stone.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        // Ожидаем перед уничтожением камней
        yield return new WaitForSeconds(timeBeforeDestroy - delayBeforeDisablingColliders);

        // Уничтожаем к��мень
        Destroy(stone);
    }
}

