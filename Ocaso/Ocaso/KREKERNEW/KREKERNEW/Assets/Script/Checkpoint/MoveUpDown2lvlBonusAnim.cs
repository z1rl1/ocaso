using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpDown2lvlBonusAnim : MonoBehaviour
{
    public float speed = 5f; // Скорость движения объекта
    public float delay = 3f; // Задержка между движениями в секундах

    public float dokuda = 0f;
    public float niz = 0f;

    private bool movingUp = true; // Флаг, указывающий, двигается ли объект вверх
    private float timer = 0f; // Таймер для отслеживания задержки

    void Update()
    {
        if (movingUp)
        {
            // Движение вверх
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            // Если объект достиг высоты 5, переключаем направление движения и сбрасываем таймер
            if (transform.position.y >= dokuda)
            {
                movingUp = false;
                timer = 0f;
            }
        }
        else
        {
            // Запускаем таймер
            timer += Time.deltaTime;

            // Если прошло время задержки, начинаем движение вниз
            if (timer >= delay)
            {
                MoveDown();
            }
        }
    }

    // Метод для движения вниз
    void MoveDown()
    {
        // Движение вниз
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Если объект достиг начальной позиции, снова переключаем направление движения и сбрасываем таймер
        if (transform.position.y <= niz)
        {
            movingUp = true;
            timer = 0f;
        }
    }
}
