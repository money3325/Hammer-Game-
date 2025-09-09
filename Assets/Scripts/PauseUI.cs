using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject pausePanel;
    void Start()
    {
        // 确保游戏开始时暂停界面是隐藏的
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // 确保游戏开始时时间是正常流动的
        Time.timeScale = 1;
    }
    public void OnPauseButtonClick()//停止按钮
    {
        PauseGame();
    }
    public void OnContinueButtonClick()//继续按钮
    {
        ResumeGame();
    }
    public void OnChangeSenceButtonClick()//切换场景按钮
    {
        ChangeSence();  
    }
    private void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        Time.timeScale = 0;
    }
    private void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        Time.timeScale = 1;
    }
    private void ChangeSence()
    {
        SceneManager.LoadScene("Custom");
    }
}
