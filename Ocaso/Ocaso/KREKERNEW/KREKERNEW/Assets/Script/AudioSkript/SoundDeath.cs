using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDeath : MonoBehaviour
{
    public static SoundDeath Instance;

    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);  // Если менеджер уже существует, уничтожаем его
    //    }
    //}

    public void PlaySound(AudioClip clip)
    {
        // Воспроизведение звука
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}

