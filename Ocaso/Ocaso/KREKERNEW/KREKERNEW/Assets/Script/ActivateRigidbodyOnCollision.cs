using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRigidbodyOnCollision : MonoBehaviour
{
    public float mass = 1.0f; // Публичное поле для массы
    public float gravity = 1.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.mass = mass; // Установка массы
                rb.gravityScale = gravity;
            }
        }
    }
}