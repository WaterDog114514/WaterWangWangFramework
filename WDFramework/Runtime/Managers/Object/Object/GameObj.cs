using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WDFramework;

/// <summary>
/// 游戏对象  和对象池紧密关联，也与加载资源紧密关联
/// </summary>
public class GameObj : Obj
{
    public GameObject Instance => _instance;
    /// <summary>
    /// 实际游戏对象
    /// </summary>
    private GameObject _instance;
    public GameObj(GameObject Instance, UnityAction IntiCallback = null)
    {
        if (Instance == null)
            Debug.LogError("实例化对象时候对象为null！");
        //赋值对象实例
        _instance = Instance;


        QuitPoolCallback += () =>
        {
            //  IntiCallback?.Invoke();
            Debug.Log("出去初始化");
            Instance.SetActive(true);
        };

        //基本初始化
        EnterPoolCallback += () =>
        {
            Debug.Log("进去初始化");
            Instance.SetActive(false);
        };
      
        DeepDestroyCallback += () =>
        {
            Object.Destroy(Instance);
        };

    }
    public Transform transform => Instance.transform;
    public string name => Instance.name;
    public T GetComponent<T>() where T : Component
    {
        return Instance.GetComponent<T>();
    }
}
