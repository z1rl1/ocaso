using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTrigger : MonoBehaviour
{
    public float minFlyUpForce = 5f; // Минимальная сила полета вверх
    public float maxFlyUpForce = 10f; // Максимальная сила полета вверх
    public float minFlyRightForce = 3f; // Минимальная сила полета вправо
    public float maxFlyRightForce = 15f; // Максимальная сила полета вправо
    public float minGravityScale = 1f; // Минимальный gravityScale
    public float maxGravityScale = 3f; // Максимальный gravityScale

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character")) // Проверяем, что это игрок
        {
            Hero player = collision.GetComponent<Hero>();
            if (player != null)
            {
                player.flyUpForce = Random.Range(minFlyUpForce, maxFlyUpForce);
                player.flyRightForce = Random.Range(minFlyRightForce, maxFlyRightForce);

                // Изменяем gravityScale
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = Random.Range(minGravityScale, maxGravityScale);
                }
            }
        }
    }
}
