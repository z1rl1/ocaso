using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    public AudioClip[] impactSounds;
    private AudioSource audioSource;
    public float volume = 0.5f;
    private bool soundPlayed = false;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.pitch = 0.8f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            if (!soundPlayed)
            {
                audioSource.clip = impactSounds[Random.Range(0, impactSounds.Length)];
                audioSource.Play();
                soundPlayed = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            soundPlayed = false;
        }
    }
}