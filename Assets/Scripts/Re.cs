using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Re : MonoBehaviour
{
    [Header("核心Body设置（手动指定）")]
    public string coreBodyPath = "Body/Body"; 
    public string coreBodyName = "Body"; 

    [Header("其他引用")]
    public Transform startPoint;       
    public string customSceneName;     
    public string gameSceneName;       
    public GameObject gameOverPanel;   
    public GameObject goCustomPanel;   

    private GameObject playerObject;   
    public static bool isBackFromCustom = false;
    private bool isPositionSaved = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (FindObjectsOfType<Re>().Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName)
        {
            Debug.Log($"主场景加载完成，开始强制查找核心Body...");
            StartCoroutine(ForceFindCoreBody());
        }
    }

    IEnumerator ForceFindCoreBody()
    {
        float timeout = 5f;
        float timer = 0f;
        
        while (playerObject == null && timer < timeout)
        {
            playerObject = GameObject.Find(coreBodyPath);
            if (playerObject == null) playerObject = GameObject.Find(coreBodyName);
            if (playerObject == null)
            {
                PlayerControl[] controls = FindObjectsOfType<PlayerControl>();
                if (controls.Length > 0) playerObject = controls[0].gameObject;
            }

            if (playerObject != null)
            {
                Debug.Log($"成功找到核心Body！路径: {coreBodyPath}, 名称: {coreBodyName}");
                if (isBackFromCustom) StartCoroutine(LoadPositionAfterFrame());
                yield break;
            }

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Debug.LogError("==============================================");
        Debug.LogError("致命错误：无法找到核心Body！");
        Debug.LogError("请检查路径和名称配置");
        Debug.LogError("==============================================");
    }

    IEnumerator LoadPositionAfterFrame()
    {
        yield return null; 
        if (playerObject != null)
        {
            LoadPlayerPos();
            isBackFromCustom = false;
        }
        else Debug.LogError("玩家对象未赋值，无法加载位置！");
    }

    public void SavePlayerPos()
    {
        if (playerObject == null)
        {
            Debug.LogError("保存失败：玩家对象未赋值");
            return;
        }

        Vector3 currentPos = playerObject.transform.position;
        PlayerPosData data = new PlayerPosData
        {
            x = currentPos.x,
            y = currentPos.y,
            z = currentPos.z
        };

        string json = JsonUtility.ToJson(data);
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "player_pos.json");
        System.IO.File.WriteAllText(savePath, json);
        
        isPositionSaved = true;
        Debug.Log($"位置已保存到 {savePath}：{currentPos}");
    }

    private void LoadPlayerPos()
    {
        string savePath = System.IO.Path.Combine(Application.persistentDataPath, "player_pos.json");
        if (!System.IO.File.Exists(savePath))
        {
            Debug.LogError($"加载失败：位置文件不存在 {savePath}");
            return;
        }

        string json = System.IO.File.ReadAllText(savePath);
        PlayerPosData data = JsonUtility.FromJson<PlayerPosData>(json);
        Vector3 newPosition = new Vector3(data.x, data.y, data.z);

        playerObject.transform.position = newPosition;
        Debug.Log($"位置加载成功：{newPosition}");
    }

   

    public void ShowGameOverPanel()
    {
        gameOverPanel?.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowGoCustomPanel()
    {
        goCustomPanel?.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CancelGoCustom()
    {
        goCustomPanel?.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GotoCustomScene()
    {
        if (string.IsNullOrEmpty(customSceneName))
        {
            Debug.LogError("请设置自定义场景名称！");
            return;
        }

        SavePlayerPos(); 
        goCustomPanel?.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(customSceneName);
    }

  

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
}

[System.Serializable]
public class PlayerPosData
{
    public float x;
    public float y;
    public float z;
}
