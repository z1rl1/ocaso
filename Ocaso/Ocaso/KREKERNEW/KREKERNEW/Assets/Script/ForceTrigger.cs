using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTrigger : MonoBehaviour
{
    public float minFlyUpForce = 5f; // ����������� ���� ������ �����
    public float maxFlyUpForce = 10f; // ������������ ���� ������ �����
    public float minFlyRightForce = 3f; // ����������� ���� ������ ������
    public float maxFlyRightForce = 15f; // ������������ ���� ������ ������
    public float minGravityScale = 1f; // ����������� gravityScale
    public float maxGravityScale = 3f; // ������������ gravityScale

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character")) // ���������, ��� ��� �����
        {
            Hero player = collision.GetComponent<Hero>();
            if (player != null)
            {
                player.flyUpForce = Random.Range(minFlyUpForce, maxFlyUpForce);
                player.flyRightForce = Random.Range(minFlyRightForce, maxFlyRightForce);

                // �������� gravityScale
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.gravityScale = Random.Range(minGravityScale, maxGravityScale);
                }
            }
        }
    }
}
