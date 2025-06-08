using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab; // Префаб птицы
    public Transform birdSpawnPoint; // Точка спавна
    public float birdSpeed = 5f; // Скорость полёта птицы

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("dieBird"))
        {
            SpawnBird();
        }
    }

    void SpawnBird()
    {
        // Создаем птицу в позиции спавна с углом -45 градусов
        GameObject bird = Instantiate(birdPrefab, birdSpawnPoint.position, Quaternion.Euler(0, 0, -45));

        // Запускаем корутину для движения птицы
        StartCoroutine(MoveBird(bird));
    }

    IEnumerator MoveBird(GameObject bird)
    {
        while (bird.transform.position.y < 10) // Задаем границу движения по оси Y
        {
            bird.transform.Translate(new Vector3(1, 1, 0) * birdSpeed * Time.deltaTime); // Движение под углом
            yield return null;
        }

        Destroy(bird); // Уничтожаем птицу после выхода за границы экрана
    }
}
