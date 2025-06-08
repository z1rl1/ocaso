using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisableSoundManager : MonoBehaviour
{
    public Button soundToggleButton; // ������ ������������ �����
    public TextMeshProUGUI soundToggleText; // ����� �� ������
    private bool isSoundOn; // ��������� �����

    void Start()
    {
        // ��������� ��������� ����� �� PlayerPrefs ��� ������
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        UpdateSoundState();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn; // ����������� ��������� �����
        UpdateSoundState();

        // ��������� ��������� ����� � PlayerPrefs
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateSoundState()
    {
        AudioListener.volume = isSoundOn ? 1 : 0;
        soundToggleText.text = isSoundOn ? "��������� ����" : "�������� ����";

        // ������� ���������� ����� � ������� ��� �������� ���������
        Debug.Log("Sound is " + (isSoundOn ? "on" : "off"));
    }
}


