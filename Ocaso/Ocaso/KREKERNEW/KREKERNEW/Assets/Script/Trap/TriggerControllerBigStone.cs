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
        // �������� Rigidbody2D ���������� �����
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
        // ���������, ��� � ������� ����� ��������
        if (other.CompareTag("Character"))
        {
            StartRockMovement();
            PlaySpawnSound();
        }
    }

    private void StartRockMovement()
    {
        // �������� �������� �����
        rockRb.simulated = true;

        // ��������� �������� ��� ������������ �����
        StartCoroutine(ResetRock());
    }

    private IEnumerator ResetRock()
    {
        // ���� ��������� �����
        yield return new WaitForSeconds(resetTime);

        // ��������� ��������� Rigidbody2D
        rockRb.simulated = false;

        // ���������� ������ � ��������� ���������
        rock.transform.position = startPosition.position;

    }
}
