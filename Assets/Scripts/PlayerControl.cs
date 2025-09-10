using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour, IPlayerBody
{
    public Transform hammerHead;
    public Transform body;
    public Transform head;
    public float maxHeadRotation = 60.0f; // 头部最大旋转角度
    public float maxRange = 2.0f;
    private Rigidbody2D bodyRb;

    // 引用Re管理器（全局位置管理）
    private Re reManager;

    void Start()
    {
        // 忽视身体和锤子的碰撞
        Physics2D.IgnoreCollision(body.GetComponent<Collider2D>(), hammerHead.GetComponent<Collider2D>());

        // 找到全局的Re管理器
        reManager = FindObjectOfType<Re>();

        // 如果是从自定义场景返回，Re会处理位置加载，这里不需要额外操作
    }

    void FixedUpdate()
    {
        // 摄像机中心和鼠标在屏幕的位置
        float depth = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, depth);
        Vector3 mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth);

        // 屏幕坐标系转世界坐标系
        center = Camera.main.ScreenToWorldPoint(center);
        mouse = Camera.main.ScreenToWorldPoint(mouse);

        // 限制鼠标移动范围
        Vector3 mouseVec = Vector3.ClampMagnitude(mouse - center, maxRange);

        // 移动锤子
        hammerHead.GetComponent<Rigidbody2D>().MovePosition(body.position + mouseVec);

        Vector3 newHammerPos = body.position + mouseVec;
        Vector3 hammerMoveVec = newHammerPos - hammerHead.position;
        newHammerPos = hammerHead.position + hammerMoveVec * 0.2f;

        hammerHead.GetComponent<Rigidbody2D>().MovePosition(newHammerPos);
        hammerHead.rotation = Quaternion.FromToRotation(Vector3.right, newHammerPos - body.position);

        // 碰撞检测与力的应用
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = LayerMask.GetMask("Default");
        Collider2D[] results = new Collider2D[5];
        if (hammerHead.GetComponent<Rigidbody2D>().OverlapCollider(contactFilter, results) > 0)
        {
            Vector3 targetBodyPos = hammerHead.position - mouseVec;
            Vector3 force = (targetBodyPos - body.position) * 80.0f;
            Debug.DrawLine(body.position, body.position + force);
            body.GetComponent<Rigidbody2D>().AddForce(force);
        }

        // 头部旋转逻辑
        Vector3 worldMouseDir = (mouse - body.position).normalized;

        if (worldMouseDir.x < 0) // 鼠标在身体左侧
        {
            head.localScale = new Vector3(1, 1, 1);
            float targetAngle = Mathf.Atan2(worldMouseDir.y, -worldMouseDir.x) * Mathf.Rad2Deg;
            targetAngle = Mathf.Clamp(targetAngle, -maxHeadRotation, maxHeadRotation);
            head.localRotation = Quaternion.Euler(0, 0, targetAngle);
        }
        else // 鼠标在身体右侧
        {
            float targetAngle = Mathf.Atan2(worldMouseDir.y, worldMouseDir.x) * Mathf.Rad2Deg;
            targetAngle = Mathf.Clamp(targetAngle, -maxHeadRotation, maxHeadRotation);
            head.rotation = Quaternion.Euler(0, 0, targetAngle);
            head.localScale = new Vector3(-1, 1, 1);
        }
    }

    // 供Re脚本调用的获取当前位置方法
    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    // 供Re脚本调用的设置位置方法
    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    void Awake()
    {
        // 提前获取身体的刚体组件（避免重复获取）
        if (body != null)
            bodyRb = body.GetComponent<Rigidbody2D>();
    }
    public void ResetPhysics()
    {
        if (bodyRb != null)
        {
            bodyRb.velocity = Vector2.zero;
            bodyRb.angularVelocity = 0f;
            // 恢复初始重力（使用你设置的4）
            bodyRb.gravityScale = 4f;
        }

    }
}
