using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRotationSaw : MonoBehaviour
{
    public float dropAmount = 0.5f; // Сколько кнопка опустится вниз при касании
    public Transform objectToRotate; // Объект, который нужно вращать
    public float rotationAmount = 45f; // Сколько градусов нужно добавить или убрать от ротации
    private Vector3 originalPosition;
    private bool isPressed = false;
    private bool hasRotated = false; // Флаг, чтобы следить за тем, была ли уже выполнена ротация
    private AudioSource audioSource; // Для воспроизведения звука
    public float rotationDuration = 1f; // Общая продолжительность вращения
    public float accelerationDuration = 0.2f; // Время, за которое объект набирает скорость
    public float decelerationDuration = 0.2f; // Время, за которое объект замедляется


    private void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasRotated && collision.gameObject.CompareTag("Character"))
        {
            hasRotated = true; // Устанавливаем флаг, что кнопка уже была активирована
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

        // Вращаем объект
        if (objectToRotate != null)
        {
            StartCoroutine(RotateObject());
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

    private IEnumerator RotateObject()
    {
        float startRotation = objectToRotate.eulerAngles.z;
        float endRotation = startRotation + rotationAmount;
        float elapsedTime = 0f;

        // Время на достижение полной скорости
        float accelerationTime = Mathf.Min(accelerationDuration, rotationDuration);
        float decelerationTime = Mathf.Max(0, rotationDuration - accelerationTime);

        // Период времени, когда объект ускоряется
        while (elapsedTime < accelerationTime)
        {
            float t = elapsedTime / accelerationTime;
            float speed = Mathf.SmoothStep(0, 1, t); // Ускорение
            float newRotationZ = Mathf.Lerp(startRotation, endRotation, speed);
            objectToRotate.eulerAngles = new Vector3(0, 0, newRotationZ);
            elapsedTime += Time.deltaTime;
            yield return null;
        }




    }
}
