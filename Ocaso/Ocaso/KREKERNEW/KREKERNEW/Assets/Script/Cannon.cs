using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject character; // Ссылка на уже существующего персонажа
    public Transform firePoint; // Точка выстрела
    public float shootForce = 100f; // Сила выстрела
    public float timedelay = 5f;

    private void Start()
    {
        // Запускаем корутину, которая будет выстреливать через 5 секунд
        StartCoroutine(ShootAfterDelay(timedelay));
    }

    private IEnumerator ShootAfterDelay(float delay)
    {
        // Ждем заданное количество секунд
        yield return new WaitForSeconds(delay);

        // Убедитесь, что персонаж существует
        if (character != null)
        {
            // Перемещаем персонажа в точку выстрела
            character.transform.position = firePoint.position;

            // Получаем Rigidbody2D компонента персонажа для применения силы
            Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Применяем силу для выстрела вправо
                rb.AddForce(firePoint.right * shootForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("Персонаж не назначен!");
        }
    }
}