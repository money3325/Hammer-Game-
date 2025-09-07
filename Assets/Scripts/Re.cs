using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Re : MonoBehaviour
{
    public GameObject playerObject;    
    public GameObject panel;    // 面板对象
    public Transform startPoint;// 起始点位置

    // 2D 触发器回调，参数是 Collider2D
    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerObject != null && other.gameObject == playerObject)
        {
            Debug.Log("触发面板显示！");
            panel.SetActive(true);     // 显示面板
            Time.timeScale = 0f;       // 暂停游戏
        }
    }

    // 按钮调用的方法
    public void RestartGame()
    {
        if (playerObject == null)
        {
            return;
        }

        if (startPoint != null)
        {
            playerObject.transform.position = startPoint.position;
        }
        else
        {
            return;
        }
        // 获取 2D 刚体
        Rigidbody2D rb2D = playerObject.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            rb2D.velocity = Vector2.zero;
            rb2D.angularVelocity = 0f;
            // 2D 刚体没有 useGravity，重力由 Gravity Scale 控制，若要恢复重力，确保 Gravity Scale 不为 0
        }
        panel.SetActive(false);    // 隐藏面板
        Time.timeScale = 1f;       // 恢复游戏
    }
}
