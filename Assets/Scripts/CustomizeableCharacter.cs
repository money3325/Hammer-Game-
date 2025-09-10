
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CustomizeableCharacter : MonoBehaviour
{

    [SerializeField]
    private CustomizedCharacter _character;
    private Re reManager;
    void Start()
    {
        // 查找全局的Re脚本（通过DontDestroyOnLoad持久化的）
        reManager = FindObjectOfType<Re>();
    }
    public void StoreCustomizationInformation()
    {
        var elements = GetComponentsInChildren<CustomizableElement>();
        _character.Data.Clear();
        foreach (var element in elements)
        {
            _character.Data.Add(element.GetCustomizationDate());
        }
         //  告诉主场景需要加载位置
        if (reManager != null)
        {
            Re.isBackFromCustom = true;
            Debug.Log("已标记从自定义场景返回，准备恢复位置");
        }
        else
        {
            Debug.LogWarning("未找到Re脚本，可能无法恢复玩家位置");
        }
        SceneManager.LoadScene("SampleScene");
    }
}