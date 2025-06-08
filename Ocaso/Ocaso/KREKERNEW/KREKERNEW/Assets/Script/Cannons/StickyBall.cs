using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBall : MonoBehaviour
{
    public float spring = 10f; // ���� �������
    public float damping = 5f; // ������� �������
    public float distance = 1f; // ����������, �� ������� ������� ���������� ��� �� ���������

    private GameObject character; // ������ �� ���������
    private SpringJoint2D springJoint; // ������ �� SpringJoint2D

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
        // ���������, ��� � ���� ��� SpringJoint2D, ����� �������� ������������
        springJoint = gameObject.AddComponent<SpringJoint2D>();

        // ��������� SpringJoint2D
        springJoint.connectedBody = character.GetComponent<Rigidbody2D>();
        springJoint.autoConfigureDistance = false; // ��������� ���������� �������
        springJoint.distance = distance; // ��������� ����������
        springJoint.dampingRatio = damping; // ��������� ������������ �������������
        springJoint.frequency = spring; // ��������� ������� �������

        // ���������, ��� � ���� ���� Rigidbody2D ��� �������������� � ��������
        if (GetComponent<Rigidbody2D>() == null)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }

        // ��������� ��������� ���� �� ���������, ����� �������� �����������
        Collider2D characterCollider = character.GetComponent<Collider2D>();
        Collider2D ballCollider = GetComponent<Collider2D>();

        if (characterCollider != null && ballCollider != null)
        {
            Physics2D.IgnoreCollision(characterCollider, ballCollider, true);
        }
    }
}
