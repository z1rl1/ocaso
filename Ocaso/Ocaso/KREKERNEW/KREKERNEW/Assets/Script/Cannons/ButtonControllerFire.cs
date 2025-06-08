using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerFire : MonoBehaviour
{
    public float dropAmount = 0.5f; // Сколько кнопка опустится вниз при касании
    public CannonControllerlvl5 cannonToFire; // Ссылка на пушку, которая будет стрелять
    private Vector3 originalPosition;
    private bool isPressed = false;
    private bool hasFired = false; // Флаг, чтобы следить за тем, была ли уже стрельба
    private AudioSource audioSource; // Для воспроизведения звука

    private void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasFired && collision.gameObject.CompareTag("Character"))
        {
            hasFired = true; // Устанавливаем флаг, что кнопка уже была активирована
            StartCoroutine(HandleButtonPress());
        }
    }

    private IEnumerator HandleButtonPress()
    {
        // Воспроизводим звук
        if (audioSource != null)
        {
            audioSource.Play();
        }
        // Опускаем кнопку
        Vector3 pressedPosition = originalPosition - new Vector3(0, dropAmount, 0);
        float elapsedTime = 0f;
        float duration = 0.2f; // Время анимации

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(originalPosition, pressedPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = pressedPosition;

        // Запускаем стрельбу
        if (cannonToFire != null)
        {
            cannonToFire.Fire();
        }

        // Возвращаем кнопку на исходное положение
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
