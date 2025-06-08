using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderHeadFollow : MonoBehaviour
{
    public Transform character; // Ссылка на объект персонажа
    public float smoothness = 5f; // Плавность следования головы за персонажем

    private void Update()
    {
        if (character != null)
        {
            // Получаем направление от головы паука к персонажу
            Vector2 direction = character.position - transform.position;

            // Вычисляем угол между головой паука и персонажем
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Создаем новый вектор поворота по оси Z
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Плавно поворачиваем голову паука в нужном направлении
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothness * Time.deltaTime);
        }
    }
}


