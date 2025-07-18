using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WDFramework;

/// <summary>
/// ��Ϸ����  �Ͷ���ؽ��ܹ�����Ҳ�������Դ���ܹ���
/// </summary>
public class GameObj : Obj
{
    public GameObject Instance => _instance;
    /// <summary>
    /// ʵ����Ϸ����
    /// </summary>
    private GameObject _instance;
    public GameObj(GameObject Instance, UnityAction IntiCallback = null)
    {
        if (Instance == null)
            Debug.LogError("ʵ��������ʱ�����Ϊnull��");
        //��ֵ����ʵ��
        _instance = Instance;


        QuitPoolCallback += () =>
        {
            //  IntiCallback?.Invoke();
            Debug.Log("��ȥ��ʼ��");
            Instance.SetActive(true);
        };

        //������ʼ��
        EnterPoolCallback += () =>
        {
            Debug.Log("��ȥ��ʼ��");
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
