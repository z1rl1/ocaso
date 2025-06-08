using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public GameObject birdPrefab; // ������ �����
    public Transform birdSpawnPoint; // ����� ������
    public float birdSpeed = 5f; // �������� ����� �����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("dieBird"))
        {
            SpawnBird();
        }
    }

    void SpawnBird()
    {
        // ������� ����� � ������� ������ � ����� -45 ��������
        GameObject bird = Instantiate(birdPrefab, birdSpawnPoint.position, Quaternion.Euler(0, 0, -45));

        // ��������� �������� ��� �������� �����
        StartCoroutine(MoveBird(bird));
    }

    IEnumerator MoveBird(GameObject bird)
    {
        while (bird.transform.position.y < 10) // ������ ������� �������� �� ��� Y
        {
            bird.transform.Translate(new Vector3(1, 1, 0) * birdSpeed * Time.deltaTime); // �������� ��� �����
            yield return null;
        }

        Destroy(bird); // ���������� ����� ����� ������ �� ������� ������
    }
}
