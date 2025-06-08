using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBall : MonoBehaviour
{
    public float spring = 10f; // Сила пружины
    public float damping = 5f; // Дампинг пружины
    public float distance = 1f; // Расстояние, на котором пружина удерживает шар от персонажа

    private GameObject character; // Ссылка на персонажа
    private SpringJoint2D springJoint; // Ссылка на SpringJoint2D

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            character = collision.gameObject;
            AttachToCharacter();
        }
        if (collision.gameObject.CompareTag("DestroyStickyBall"))
        {
            Destroy(gameObject);
        }
    }

    private void AttachToCharacter()
    {
        // Убедитесь, что у шара нет SpringJoint2D, чтобы избежать дублирования
        springJoint = gameObject.AddComponent<SpringJoint2D>();

        // Настройка SpringJoint2D
        springJoint.connectedBody = character.GetComponent<Rigidbody2D>();
        springJoint.autoConfigureDistance = false; // Настройка расстояния вручную
        springJoint.distance = distance; // Установка расстояния
        springJoint.dampingRatio = damping; // Настройка коэффициента демпфирования
        springJoint.frequency = spring; // Установка частоты пружины

        // Убедитесь, что у шара есть Rigidbody2D для взаимодействия с пружиной
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }

        // Отключаем коллайдер шара от персонажа, чтобы избежать пересечения
        Collider2D characterCollider = character.GetComponent<Collider2D>();
        Collider2D ballCollider = GetComponent<Collider2D>();

        if (characterCollider != null && ballCollider != null)
        {
            Physics2D.IgnoreCollision(characterCollider, ballCollider, true);
        }
    }
}
