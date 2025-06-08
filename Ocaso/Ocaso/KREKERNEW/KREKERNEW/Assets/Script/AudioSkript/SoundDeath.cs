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
    //        Destroy(gameObject);  // ���� �������� ��� ����������, ���������� ���
    //    }
    //}

    public void PlaySound(AudioClip clip)
    {
        // ��������������� �����
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }
}

