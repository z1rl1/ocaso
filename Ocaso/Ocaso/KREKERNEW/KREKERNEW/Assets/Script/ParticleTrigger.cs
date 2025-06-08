using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float disableTime = 2f; // время отключения триггера в секундах
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character") && !isTriggered)
        {
            particleSystem.Play();
            isTriggered = true;
            StartCoroutine(DisableTrigger());
        }
    }

    private IEnumerator DisableTrigger()
    {
        yield return new WaitForSeconds(disableTime);
        isTriggered = false;
    }
}
