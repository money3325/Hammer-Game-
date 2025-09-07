using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform hammerHead;
    public Transform body;
    public Transform head;
    public float maxHeadRotation = 60.0f; // 头部最大旋转角度
    public float maxRange = 2.0f;

    

    void Start()
    {
        //忽视身体和锤子的碰撞，碰撞用的是碰撞器，所以get的是这两个物件的碰撞器
        Physics2D.IgnoreCollision(body.GetComponent<Collider2D>(), hammerHead.GetComponent<Collider2D>());
    }
    void FixedUpdate()
    {
        //摄像机中心，和鼠标在屏幕的位置
        float depth = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, depth);
        Vector3 mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth);
        //屏幕坐标系变成世界坐标系
        center = Camera.main.ScreenToWorldPoint(center);
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        //将鼠标的移动范围限制
        Vector3 mouseVec = Vector3.ClampMagnitude(mouse - center, maxRange);
        //移动锤子，获取锤子的身体，移动，身体的位置到鼠标的位置，就是这能移动这些范围
        hammerHead.GetComponent<Rigidbody2D>().MovePosition(body.position + mouseVec);

        Vector3 newHammerPos = body.position + mouseVec;
        Vector3 hammerMoveVec = newHammerPos - hammerHead.position;
        newHammerPos = hammerHead.position + hammerMoveVec * 0.2f;

        hammerHead.GetComponent<Rigidbody2D>().MovePosition(newHammerPos);

        hammerHead.rotation = Quaternion.FromToRotation(Vector3.right, newHammerPos - body.position);

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
        Vector3 mouseDir = new Vector3(Input.mousePosition.x / Screen.width * 2.0f - 1.0f, Input.mousePosition.y / Screen.height * 2.0f - 1.0f,0);
        // 判断鼠标在屏幕左侧还是右侧
     Vector3 worldMouseDir = (mouse - body.position).normalized;
    
    if (worldMouseDir.x < 0) // 鼠标在身体左侧
    {
        // 确保头部朝向正确（不翻转）
        head.localScale = new Vector3(1, 1, 1);
        
        // 计算头部应该朝向的角度（基于鼠标方向）
        // 使用Atan2计算角度，但要根据图片初始朝向调整
        float targetAngle = Mathf.Atan2(worldMouseDir.y, -worldMouseDir.x) * Mathf.Rad2Deg;
        
        // 限制头部旋转角度
        targetAngle = Mathf.Clamp(targetAngle, -maxHeadRotation, maxHeadRotation);
        
        // 应用旋转到头部
        head.localRotation = Quaternion.Euler(0, 0, targetAngle);
    }
    else // 鼠标在身体右侧
    {
        // 计算头部应该朝向的角度（基于鼠标方向）
        float targetAngle = Mathf.Atan2(worldMouseDir.y, worldMouseDir.x) * Mathf.Rad2Deg;
        
        // 限制头部旋转角度
        targetAngle = Mathf.Clamp(targetAngle, -maxHeadRotation, maxHeadRotation);
        
        // 应用旋转到头部
        head.rotation = Quaternion.Euler(0, 0, targetAngle);
        
        // 翻转头部图片（使其面向右侧）
        head.localScale = new Vector3(-1, 1, 1);
    }
    
    }
}
