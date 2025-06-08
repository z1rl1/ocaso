using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public CanvasGroup pausePanel; // ���������, ��� ����� CanvasGroup, � �� GameObject
    private bool isPaused = false;
    private bool isSound;

    void Start()
    {
        // ������������� ������ �����
        pausePanel.alpha = 0f; // ���������� ����� � 0, ����� ������ ������
        pausePanel.interactable = false; // ��������� ��������������
        pausePanel.blocksRaycasts = false; // ��������� ���� ���������

    }

    void Update()
    {
        // ������ ����, ��� �� ����� ����������� �����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        AudioListener.pause = isSound;
    }

    public void TogglePause()
    {
        isPaused = !isPaused; // ����������� ��������� �����

        
        if (isPaused)
        {
            ShowPausePanel();
        }
        else
        {
            HidePausePanel();
        }
    }

    public void ShowPausePanel()
    {
        isSound = true;
        pausePanel.alpha = 1f; // ���������� ����� � 1, ����� ������� ������ �������
        pausePanel.interactable = true; // ��������� ��������������
        pausePanel.blocksRaycasts = true; // ����������� ����
        Time.timeScale = 0f; // ���������� ���� (���� ��� ����������)

        Debug.Log("ShowPausePanel");
    }

    public void HidePausePanel()
    {
        Debug.Log("HidePausePanel ����������");

        isSound = false;
        pausePanel.alpha = 0f; // ���������� ����� � 0, ����� ������ ������
        pausePanel.interactable = false; // ��������� ��������������
        pausePanel.blocksRaycasts = false; // ��������� ���� ���������

        Debug.Log("�������� ������� ����: " + Time.timeScale);
        Time.timeScale = 1f; // ����������� ���� (���� ��� ����������)

        Debug.Log("����� �������������� ����, �����: " + Time.timeScale);
    }

}