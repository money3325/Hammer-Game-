using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReStart : MonoBehaviour
{
    public GameObject playerObject;
    public float floatForce = 5f; // 向上漂浮的力


    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerObject != null && other.gameObject == playerObject)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("触发太空浮力！关闭重力");
                rb.gravityScale = 0; // 2D中关闭重力的正确方式
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (playerObject != null && other.gameObject == playerObject)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 使用2D物理支持的力模式
                rb.AddForce(Vector2.up * floatForce, ForceMode2D.Force); 
                // 或根据需求使用 ForceMode2D.Impulse（瞬间冲力）
            }
        }
    }
}
