using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingCircle : MonoBehaviour
{
    public float targetScale = 0.5f;  // Размер, до которого уменьшается круг
    public float shrinkSpeed = 2f;    // Скорость уменьшения
    public float growSpeed = 2f;      // Скорость увеличения
    public float shrinkWaitTime = 1f; // Время ожидания после уменьшения
    public float growWaitTime = 1f;   // Время ожидания после увеличения

    private Vector3 originalScale;
    private float timeCounter = 0f;
    private bool isShrinking = true;
    private bool isWaiting = false;
    private bool isPulsing = false; // Флаг для активации пульсации

    void Start()
    {
        // Запоминаем исходный размер объекта
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isPulsing)
        {
            if (isWaiting)
            {
                // Если находимся в режиме ожидания, обновляем таймер ожидания
                timeCounter += Time.deltaTime;

                // Проверяем, прошло ли достаточно времени
                if (isShrinking && timeCounter >= shrinkWaitTime)
                {
                    isWaiting = false;
                    timeCounter = 0f;
                    isShrinking = false; // Переходим к увеличению
                }
                else if (!isShrinking && timeCounter >= growWaitTime)
                {
                    isWaiting = false;
                    timeCounter = 0f;
                    isShrinking = true; // Переходим к уменьшению
                }
            }
            else
            {
                if (isShrinking)
                {
                    // Уменьшаем масштаб
                    float scale = Mathf.Lerp(originalScale.x, targetScale, timeCounter / shrinkSpeed);
                    transform.localScale = new Vector3(scale, scale, scale);

                    // Если достигли целевого размера, начинаем ожидание
                    if (timeCounter >= shrinkSpeed)
                    {
                        isWaiting = true;
                        timeCounter = 0f;
                    }
                }
                else
                {
                    // Увеличиваем масштаб обратно
                    float scale = Mathf.Lerp(targetScale, originalScale.x, timeCounter / growSpeed);
                    transform.localScale = new Vector3(scale, scale, scale);

                    // Если достигли исходного размера, начинаем ожидание
                    if (timeCounter >= growSpeed)
                    {
                        isWaiting = true;
                        timeCounter = 0f;
                    }
                }

                // Увеличиваем таймер
                timeCounter += Time.deltaTime;
            }
        }
    }

    public void StartPulsing()
    {
        isPulsing = true;
    }
}
