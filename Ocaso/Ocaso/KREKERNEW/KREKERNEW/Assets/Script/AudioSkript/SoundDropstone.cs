using UnityEngine;

public class SoundDropstone : MonoBehaviour
{
    public AudioClip[] impactSounds;
    private AudioSource[] audioSources;
    public float volume = 0.5f;
    public float pitch = 1f;
    private bool soundPlayed = false;

    private void Start()
    {
        // Инициализируем массив audioSources с помощью создания новых AudioSource.
        audioSources = new AudioSource[impactSounds.Length];
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].volume = volume; // Устанавливаем громкость 0.5 для каждого AudioSource
            audioSources[i].pitch = pitch;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!soundPlayed && !collision.gameObject.CompareTag("NoSound") && !collision.gameObject.CompareTag("Character"))
        {
            AudioSource availableSource = GetAvailableAudioSource();
            if (availableSource != null)
            {
                availableSource.clip = impactSounds[Random.Range(0, impactSounds.Length)];
                availableSource.volume = volume;
                availableSource.pitch = pitch;
                availableSource.Play();
                soundPlayed = true;
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
}