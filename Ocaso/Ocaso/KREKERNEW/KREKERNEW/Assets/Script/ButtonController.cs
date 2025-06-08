using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Transform objectToLift; // Объект, который нужно поднимать
    public float liftAmount = 20f; // Насколько поднимать объект
    public float liftSpeed = 20f;  // Скорость поднятия объекта
    public float buttonPressAmount = 0.1f; // Насколько кнопка будет опускаться
    public float buttonPressSpeed = 2f; // Скорость опускания кнопки

    public GameObject rotationObject1; // Первый объект для включения ConstantRotation
    public GameObject rotationObject2; // Второй объект для включения ConstantRotation

    private Vector3 initialPosition;
    private bool isPressed = false;
    private ConstantRotation rotationScript1;
    private ConstantRotation rotationScript2;
    private AudioSource audioSource1;
    private AudioSource audioSource2;

    void Start()
    {
        initialPosition = transform.position;

        // Получаем скрипты ConstantRotation и AudioSource из объектов, если они прикреплены
        if (rotationObject1 != null)
        {
            rotationScript1 = rotationObject1.GetComponent<ConstantRotation>();
            audioSource1 = rotationObject1.GetComponent<AudioSource>();
        }
        if (rotationObject2 != null)
        {
            rotationScript2 = rotationObject2.GetComponent<ConstantRotation>();
            audioSource2 = rotationObject2.GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (isPressed)
        {
            // Поднимаем объект
            if (objectToLift != null && objectToLift.position.y < initialPosition.y + liftAmount)
            {
                objectToLift.position = Vector3.MoveTowards(objectToLift.position, new Vector3(objectToLift.position.x, initialPosition.y + liftAmount, objectToLift.position.z), liftSpeed * Time.deltaTime);
            }

            // Включаем ConstantRotation и AudioSource на объектах
            if (rotationScript1 != null)
            {
                rotationScript1.enabled = true;
                if (audioSource1 != null && !audioSource1.isPlaying)
                    audioSource1.Play();
            }
            if (rotationScript2 != null)
            {
                rotationScript2.enabled = true;
                if (audioSource2 != null && !audioSource2.isPlaying)
                    audioSource2.Play();
            }
        }
        

        // Кнопка немного опускается при нажатии
        float buttonYPosition = isPressed ? initialPosition.y - buttonPressAmount : initialPosition.y;
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, buttonYPosition, buttonPressSpeed * Time.deltaTime), transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isPressed = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPressed = false;
        }
    }
}
