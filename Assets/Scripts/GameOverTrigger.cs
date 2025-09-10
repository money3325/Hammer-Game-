using UnityEngine;
using UnityEngine.UI;

public class GameOverTrigger : MonoBehaviour
{
    
    public ReStart reStart; // 关联太空模式脚本
    public GameObject gameOverPanel; // 游戏结束面板
    public string playerPath; // 玩家核心Body的完整路径
    public Transform startPoint; // 重新开始的原点
    public Button restartButton; // 重新开始按钮

    private GameObject playerObject; // 玩家对象缓存
    private bool isPositionSaved = false; // 初始化为false，表示位置未保存
    void Start()
    {
        // 找到玩家对象
        playerObject = GameObject.Find(playerPath);
        
        // 绑定重新开始按钮事件
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("重新开始按钮未赋值！");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 检查触发对象是否是玩家
        if (playerObject != null && other.gameObject == playerObject)
        {
            Debug.Log("检测到玩家进入结束区域，触发游戏结束");
            
            // 退出太空模式
            if (reStart != null)
            {
                reStart.ExitSpace();
            }
            
            // 显示结束面板并暂停时间
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                Debug.LogError("gameOverPanel未赋值！");
            }
        }
    }

    // 重新开始游戏逻辑（从Re脚本迁移）
    public void RestartGame()
    {
        if (playerObject == null || startPoint == null)
        {
            Debug.LogError("玩家对象或起点未设置！");
            return;
        }

        // 1. 优先重置太空状态（核心修复）
        if (reStart != null)
        {
            reStart.ResetSpaceState(); // 调用新增的强制重置方法
        }
        else
        {
            Debug.LogError("ReStart组件未关联，无法重置太空状态！");
        }

        // 2. 移动到原点
        playerObject.transform.position = startPoint.position;

        // 3. 重置物理
        ResetPlayerPhysics();

        // 4. 隐藏面板和恢复时间
        gameOverPanel?.SetActive(false);
       
        Time.timeScale = 1f;

        Debug.Log("游戏已重新开始，所有状态已重置");
     PlayerControl playerControl = playerObject.GetComponent<PlayerControl>();
    if (playerControl != null)
    {
        playerControl.ResetPhysics();
    }
}
    // 重置玩家物理状态（从Re脚本迁移）
    private void ResetPlayerPhysics()
    {
        if (playerObject != null)
        {
            Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}

