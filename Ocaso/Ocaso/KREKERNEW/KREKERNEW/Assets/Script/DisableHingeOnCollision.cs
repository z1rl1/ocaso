using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHingeOnCollision : MonoBehaviour
{
    // ������ �� Hinge Joint 2D ��������� ��� ������� ����� ����
    public HingeJoint2D[] hingeJoints;

    // ������ ���������, ��� ������� ����� ��������� Hinge Joint
    public Vector3 characterSize = new Vector3(0.2582829f, 0.2582829f, 0.2582829f);

    // ������� ����������, ����� ������ ������ � ������� � ������ �����������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ��������� ��� ����������
        if (collision.collider.CompareTag("Character"))
        {
            // ��������� ������ ���������
            if (Mathf.Approximately(collision.collider.transform.localScale.x, characterSize.x) &&
                Mathf.Approximately(collision.collider.transform.localScale.y, characterSize.y) &&
                Mathf.Approximately(collision.collider.transform.localScale.z, characterSize.z))
            {
                // ��������� Hinge Joint ��� ������� ����� ����
                foreach (var hingeJoint in hingeJoints)
                {
                    hingeJoint.enabled = false;
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character"))
        {
            // ��������� ������ ���������
            if (Mathf.Approximately(collision.transform.localScale.x, characterSize.x) &&
                Mathf.Approximately(collision.transform.localScale.y, characterSize.y) &&
                Mathf.Approximately(collision.transform.localScale.z, characterSize.z))
            {
                // ��������� Hinge Joint ��� ������� ����� ����
                foreach (var hingeJoint in hingeJoints)
                {
                    hingeJoint.enabled = false;
                }
            }
        }
    }


}