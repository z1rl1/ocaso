using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingActivator : MonoBehaviour
{
    public GameObject[] pulsatingObjects; // Массив объектов для активации пульсации

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что объект с тегом "Character" вошел в триггер
        if (other.CompareTag("Character"))
        {
            // Проходим по всем назначенным объектам и активируем пульсацию
            foreach (GameObject obj in pulsatingObjects)
            {
                PulsingCircle pulsingCircle = obj.GetComponent<PulsingCircle>();
                if (pulsingCircle != null)
                {
                    pulsingCircle.StartPulsing();
                }
            }
        }
    }
}
