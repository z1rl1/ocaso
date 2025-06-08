using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Transform objectToLift; // ������, ������� ����� ���������
    public float liftAmount = 20f; // ��������� ��������� ������
    public float liftSpeed = 20f;  // �������� �������� �������
    public float buttonPressAmount = 0.1f; // ��������� ������ ����� ����������
    public float buttonPressSpeed = 2f; // �������� ��������� ������

    public GameObject rotationObject1; // ������ ������ ��� ��������� ConstantRotation
    public GameObject rotationObject2; // ������ ������ ��� ��������� ConstantRotation

    private Vector3 initialPosition;
    private bool isPressed = false;
    private ConstantRotation rotationScript1;
    private ConstantRotation rotationScript2;
    private AudioSource audioSource1;
    private AudioSource audioSource2;

    void Start()
    {
        initialPosition = transform.position;

        // �������� ������� ConstantRotation � AudioSource �� ��������, ���� ��� �����������
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
            // ��������� ������
            if (objectToLift != null && objectToLift.position.y < initialPosition.y + liftAmount)
            {
                objectToLift.position = Vector3.MoveTowards(objectToLift.position, new Vector3(objectToLift.position.x, initialPosition.y + liftAmount, objectToLift.position.z), liftSpeed * Time.deltaTime);
            }

            // �������� ConstantRotation � AudioSource �� ��������
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
        

        // ������ ������� ���������� ��� �������
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
