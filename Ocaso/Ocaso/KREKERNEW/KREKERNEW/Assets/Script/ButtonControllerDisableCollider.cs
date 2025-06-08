using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControllerDisableCollider : MonoBehaviour
{
    public Rigidbody2D objectDrop; // Объект, который нужно поднимать
    public float buttonPressAmount = 0.1f; // Насколько кнопка будет опускаться
    public float buttonPressSpeed = 2f; // Скорость опускания кнопки
    private bool isPressed = false;
    private AudioSource audioSource1;
    public float mass = 1.0f;
    public float gravity = 1.0f;
    private Vector3 initialPosition;
    public AudioClip spawnSound;
    public AudioSource audioSource;

    public float shakeMagnitude = 1f;
    public float shakeDuration = 0.45f;
    private CameraController cameraController;


    void Start()
    {
        initialPosition = transform.position;
        cameraController = Camera.main.GetComponent<CameraController>();

    }
    void Update()
    {
        // Кнопка немного опускается при нажатии
        float buttonYPosition = isPressed ? initialPosition.y - buttonPressAmount : initialPosition.y;
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, buttonYPosition, buttonPressSpeed * Time.deltaTime), transform.position.z);
    }
    private void PlaySpawnSound()
    {
        if (spawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (objectDrop != null)
            {
                objectDrop.bodyType = RigidbodyType2D.Dynamic;
                objectDrop.mass = mass; // Установка массы
                objectDrop.gravityScale = gravity;
                isPressed = true;

                PlaySpawnSound();

            }

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character") && !isPressed)
        {
            // Начинаем тряску камеры
            cameraController.shakeMagnitude = shakeMagnitude;
            cameraController.shakeDuration = shakeDuration;
            cameraController.ShakeCamera();

        }
    }
}
