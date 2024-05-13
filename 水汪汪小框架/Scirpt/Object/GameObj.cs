using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 游戏对象  和对象池紧密关联，也与加载资源紧密关联
/// </summary>
public class GameObj : Obj
{

    /// <summary>
    /// 实际游戏对象
    /// </summary>
    private GameObject Instance;
    public GameObj(GameObject Instance,UnityAction IntiCallback = null)
    {
        if (Instance == null)
            Debug.LogError("实例化对象时候对象为null！");

        this.Instance = Instance;

        IntiCallback?.Invoke();
        //基本初始化
        EnterPoolCallback += () =>
        {
            Instance.SetActive(false);
        };
        QuitPoolCallback += () =>
        {
            Instance.SetActive(true);
            IntiCallback?.Invoke();
        };

        DestroyCallback += () =>
        {
            Object.Destroy(Instance);
        };

    }
    public Transform transform => Instance.transform;
    public string name => Instance.name;

    public override string PoolIdentity => name;

    public T GetComponent<T>() where T : Component
    {
        return Instance.GetComponent<T>();
    }
}
