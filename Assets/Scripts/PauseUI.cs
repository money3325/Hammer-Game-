using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject pausePanel;
    public PlayerControl playerSaver;
    private Re reManager;
    void Start()
    {
        // 确保游戏开始时暂停界面是隐藏的
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // 确保游戏开始时时间是正常流动的
        Time.timeScale = 1;
        // 2. 找到全局的Re脚本（因为Re用了DontDestroyOnLoad，跨场景存在）
        reManager = FindObjectOfType<Re>();
        if (reManager == null)
        {
            Debug.LogError("未找到Re脚本！请确保主场景中挂载了Re脚本");
        }
    }
    public void OnPauseButtonClick()//停止按钮
    {
        PauseGame();
    }
    public void OnContinueButtonClick()//继续按钮
    {
        ResumeGame();
    }
    public void OnExitButtonClick()//继续按钮
    {
        ExitGame();
    }
    public void OnChangeSenceButtonClick()//切换场景按钮
    {
        if (reManager != null)
        {
            reManager.SavePlayerPos(); // 调用Re的方法保存当前位置
            Debug.Log("切换到自定义场景前，已保存玩家位置");
        }
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
        ResumeGame(); // 先恢复时间（避免自定义场景暂停）
        SceneManager.LoadScene("Custom");
    }
    private void ExitGame()
    {
        // 编辑器环境下仅显示日志，不实际退出
        #if UNITY_EDITOR
            Debug.Log("收到退出指令（编辑器模式下不执行实际退出）");
        #else
            // 打包后的程序会执行实际退出
            Application.Quit();
        #endif
    }
}
