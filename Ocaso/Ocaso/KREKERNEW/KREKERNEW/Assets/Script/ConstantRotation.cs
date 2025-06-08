using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;  // —корость вращени€
    public float pushStrength = 5f;     // —ила, с которой шестерн€ откидывает персонажа

    private Rigidbody2D rb;
    private bool isPlayerNear = false;  // ‘лаг, показывающий, что персонаж р€дом с шестерней
    private Rigidbody2D playerRb;       // Rigidbody персонажа

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // ”станавливаем тип тела как Kinematic, чтобы не реагировать на коллизии
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
            rb.drag = 0;
            rb.angularDrag = 0;
        }
    }

    void Update()
    {
        // ѕосто€нно вращаем шестеренку вокруг оси Z
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        // ≈сли персонаж р€дом с шестерней, передаем ему силу
        if (isPlayerNear && playerRb != null)
        {
            // ѕолучаем направление силы на основе угла вращени€ шестеренки
            Vector2 forceDirection = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad));

        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // ѕровер€ем, что это персонаж
        if (other.gameObject.CompareTag("Character"))
        {
            playerRb = other.gameObject.GetComponent<Rigidbody2D>();
            isPlayerNear = true;
        }
    }


    void OnCollisionExit2D(Collision2D other)
    {
        //  огда персонаж выходит из области действи€ шестерни
        if (other.gameObject.CompareTag("Character"))
        {
            isPlayerNear = false;
            playerRb = null;
        }
    }
}