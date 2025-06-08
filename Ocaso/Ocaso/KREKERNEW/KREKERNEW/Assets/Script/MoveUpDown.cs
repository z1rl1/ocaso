using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float speed = 5f; // Скорость движения объекта
    public float delay = 3f; // Задержка между движениями в секундах

    private bool movingUp = true; // Флаг, указывающий, двигается ли объект вверх
    private float timer = 0f; // Таймер для отслеживания задержки

    void Update()
    {
        if (movingUp)
        {
            // Движение вверх
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            // Если объект достиг высоты 5, переключаем направление движения и сбрасываем таймер
            if (transform.position.y >= 38f)
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
        if (transform.position.y <= 32f)
        {
            movingUp = true;
            timer = 0f;
        }
    }
}
