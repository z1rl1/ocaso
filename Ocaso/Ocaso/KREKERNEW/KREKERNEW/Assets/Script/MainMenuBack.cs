using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MainMenuBack : MonoBehaviour
{

    public CanvasGroup pausePanel; // ���������, ��� ����� CanvasGroup, � �� GameObject

    void Start()
    {
        // ������������� ������ �����
        pausePanel.alpha = 0f; // ���������� ����� � 0, ����� ������ ������
        pausePanel.interactable = false; // ��������� ��������������
        pausePanel.blocksRaycasts = false; // ��������� ���� ���������

    }

    public void GoToLevel0()
    {
        // ������� �� ����� ������ 0
        SceneManager.LoadScene("lvl0");
        Time.timeScale = 1f;
        YandexGame.FullscreenShow();
        
    }

}
