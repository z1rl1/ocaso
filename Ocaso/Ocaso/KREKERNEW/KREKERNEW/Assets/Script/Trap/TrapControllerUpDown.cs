using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapControllerUpDown : MonoBehaviour
{
    public GameObject trap1;
    public GameObject trap2;
    public GameObject triggerToActivate; // Новый триггер
    public float moveAmount = 1f; // Сколько опустится/поднимется ловушка
    public float moveSpeed = 2f; // Скорость движения
    public float returnDelay = 2f; // Задержка перед возвратом
    public float activateDelay = 1f; // Задержка перед активацией триггера
    public float deactivateDelay = 2f; // Задержка перед деактивацией триггера
    public GameObject effect1;
    public GameObject effect2;
    public float delayDrop = 0.5f;
    public AudioClip spawnSound;
    public AudioSource audioSource;
    private bool isTriggered = false;

    public float shakeMagnitude = 1f;
    public float shakeDuration = 0.45f;

    private CameraController cameraController;

    private void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(ActivateTraps());
        }
    }
    private void PlaySpawnSound()
    {
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }
    private IEnumerator ActivateTraps()
    {
        // Задержка перед активацией ловушек
        yield return new WaitForSeconds(delayDrop);
        Instantiate(effect1);
        Instantiate(effect2);

        // Опускаем Trap1
        Vector3 targetPosition1 = trap1.transform.position - new Vector3(0, moveAmount, 0);
        Vector3 targetPosition2 = trap2.transform.position + new Vector3(0, moveAmount, 0);
        float journeyLength1 = Vector3.Distance(trap1.transform.position, targetPosition1);
        float journeyLength2 = Vector3.Distance(trap2.transform.position, targetPosition2);
        float startTime = Time.time;
        PlaySpawnSound();
        if (isTriggered)
        {
            cameraController.shakeMagnitude = shakeMagnitude;
            cameraController.shakeDuration = shakeDuration;
            cameraController.ShakeCamera();
        }



        while (trap1.transform.position.y > targetPosition1.y || trap2.transform.position.y < targetPosition2.y)
        {
            float distCovered1 = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney1 = distCovered1 / journeyLength1;

            float distCovered2 = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney2 = distCovered2 / journeyLength2;

            trap1.transform.position = Vector3.Lerp(trap1.transform.position, targetPosition1, fractionOfJourney1);
            trap2.transform.position = Vector3.Lerp(trap2.transform.position, targetPosition2, fractionOfJourney2);

            yield return null;
        }





        yield return ActivateNewTrigger();
        // Ждем заданное время перед возвратом
        yield return new WaitForSeconds(returnDelay);
        yield return DisActivateNewTrigger();

        // Возвращаем ловушки обратно
        yield return MoveTraps(trap1, trap1.transform.position + new Vector3(0, moveAmount, 0));
        yield return MoveTraps(trap2, trap2.transform.position - new Vector3(0, moveAmount, 0));

        isTriggered = false; // Сбрасываем триггер
    }

    private IEnumerator MoveTraps(GameObject trap, Vector3 targetPosition)
    {
        float journeyLength = Vector3.Distance(trap.transform.position, targetPosition);
        float startTime = Time.time;

        while (trap.transform.position != targetPosition)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            trap.transform.position = Vector3.Lerp(trap.transform.position, targetPosition, fractionOfJourney);
            yield return null;
        }
    }

    private IEnumerator ActivateNewTrigger()
    {
        // Ждем n секунд перед активацией триггера
        yield return new WaitForSeconds(activateDelay);

        // Активируем триггер (например, включаем его)
        triggerToActivate.SetActive(true);
    }

    private IEnumerator DisActivateNewTrigger()
    {
        // Ждем m секунд перед деактивацией триггера
        yield return new WaitForSeconds(deactivateDelay);

        // Деактивируем триггер
        triggerToActivate.SetActive(false);
    }
}
