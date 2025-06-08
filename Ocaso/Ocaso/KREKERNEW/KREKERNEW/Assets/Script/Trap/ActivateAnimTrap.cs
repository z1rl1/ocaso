using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimTrap : MonoBehaviour
{
    public List<GameObject> objectsToActivate; // ������ �������� � �����������
    public float deactivateTime = 2f; // �����, �� ������� ������� ����� ��������

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character")) // ���������, ��� ������ � ����� "Character"
        {
            ActivateAnimators();
            StartCoroutine(DeactivateTriggerAndAnimators());
        }
    }

    private void ActivateAnimators()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            Animator animator = obj.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true; // �������� ��������
            }
        }
    }

    private IEnumerator DeactivateTriggerAndAnimators()
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false; // ��������� �������

            // ���� �������� �����
            yield return new WaitForSeconds(deactivateTime);

            // ��������� ���������
            foreach (GameObject obj in objectsToActivate)
            {
                Animator animator = obj.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = false; // ��������� ��������
                }
            }

            // �������� ������� �����
            triggerCollider.enabled = true;
        }
    }
}