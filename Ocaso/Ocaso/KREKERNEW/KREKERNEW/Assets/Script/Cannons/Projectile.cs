using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 10f; // Время жизни снаряда
    public float damage = 10f;    // Сила воздействия на цепи

    private void Start()
    {
        Destroy(gameObject, lifetime); // Удаляем снаряд через некоторое время
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, столкнулся ли снаряд с цепью
        if (collision.gameObject.CompareTag("Chain"))
        {
            // Получаем HingeJoint2D (или другой компонент, связанный с цепями)
            HingeJoint2D hinge = collision.gameObject.GetComponent<HingeJoint2D>();
            if (hinge != null)
            {
                // Отключаем или разрушаем цепь
                hinge.enabled = false; // Отключаем HingeJoint2D
                // или можно использовать Destroy(collision.gameObject) если хотите полностью удалить объект
            }
        }

    }
}
