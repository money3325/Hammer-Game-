using UnityEngine;

public class ReStart : MonoBehaviour
{
    public GameObject playerObject;
    public float floatForce = 5f;
    public bool isInSpace = false;
    public float normalGravityScale = 1f; // 新增：正常重力值

    void OnTriggerEnter2D(Collider2D other)
    {
        if (playerObject != null && other.gameObject == playerObject)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Debug.Log("触发太空浮力！关闭重力");
                rb.gravityScale = 0;
                isInSpace = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isInSpace && playerObject != null && other.gameObject == playerObject)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.up * floatForce, ForceMode2D.Force); 
            }
        }
    }

    public void ExitSpace()
    {
        ResetSpaceState(); // 调用统一的重置方法
    }

    // 新增：强制重置太空状态的方法
    public void ResetSpaceState()
    {
        isInSpace = false;
        if (playerObject != null)
        {
            Rigidbody2D rb = playerObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = normalGravityScale; // 使用预设的正常重力值
                rb.velocity = Vector2.zero; // 清除速度
                rb.angularVelocity = 0; // 清除旋转
            }
        }
        Debug.Log("太空状态已强制重置");
    }
}