using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] impactSounds;
    public AudioClip[] metalSounds;
    public AudioClip[] stoneSounds;
    public AudioClip[] treeSounds;


    private bool hasPlayed = false;
    private AudioSource[] audioSources;
    public float volume = 1.0f; // Переменная для хранения громкости звука

    void Start()
    {
        audioSources = new AudioSource[1];
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    private void PlayImpactSound(Collision2D collision)
    {
        if (!hasPlayed)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
            {
                AudioSource availableSource = GetAvailableAudioSource();
                if (availableSource != null)
                {
                    availableSource.clip = impactSounds[Random.Range(0, impactSounds.Length)];
                    availableSource.volume = volume;
                    availableSource.Play();
                    hasPlayed = true;
                }
            }
            else if (collision.gameObject.CompareTag("Metal"))
            {
                AudioSource availableSource = GetAvailableAudioSource();
                if (availableSource != null)
                {
                    // Assign a different sound for the 'Metal' tag
                    availableSource.clip = metalSounds[Random.Range(0, metalSounds.Length)];
                    availableSource.volume = volume;
                    availableSource.Play();
                    hasPlayed = true;
                }
            }
            else if (collision.gameObject.CompareTag("Stone"))
            {
                AudioSource availableSource = GetAvailableAudioSource();
                if (availableSource != null)
                {
                    // Assign a different sound for the 'Stone' tag
                    availableSource.clip = stoneSounds[Random.Range(0, stoneSounds.Length)];
                    availableSource.volume = volume;
                    availableSource.Play();
                    hasPlayed = true;
                }
            }
            else if (collision.gameObject.CompareTag("Tree"))
            {
                AudioSource availableSource = GetAvailableAudioSource();
                if (availableSource != null)
                {
                    // Assign a different sound for the 'Tree' tag
                    availableSource.clip = treeSounds[Random.Range(0, treeSounds.Length)];
                    availableSource.volume = volume;
                    availableSource.Play();
                    hasPlayed = true;
                }
            }
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }
        return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasPlayed = false; // Сбрасываем флаг при нажатии на пробел
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayImpactSound(collision);
    }
}