using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WDFramework;


//Ϊ�˸���ܸ��ӷ���ĵ��ã�����һ�׹���Object��αObj��ʹ��
/// <summary>
/// �����������
/// </summary>
public abstract class Obj
{
    /// <summary>
    /// ��������ر�ʶ�����ݶ���ͨ��Type���֣���Ϸ����ͨ����������
    /// </summary>
    //ʹ�ô˱������Excel����ϸ��Ʒ��飬�����Լ�������͵ȵ�
    //������ͨ��ʹ��Ĭ�ϵĶ�������ã�Ϊ���ݳ�
    public string PoolIdentity;
    //��������ػص�
    public UnityAction EnterPoolCallback;
    public UnityAction QuitPoolCallback;
    // ����ʱ��ص�
    public UnityAction DeepDestroyCallback;
    /// <summary>
    /// ǳ���٣������Ž��������
    /// </summary>
    public void DestroyToPool()
    {
        ObjectManager.Instance.DestroyObj(this);
    }
    /// <summary>
    /// ������ٶ�����ȫ���ڴ����Ƴ�
    /// </summary>
    public void DeepDestroy()
    {

        ObjectManager.Instance.DeepDestroyObj(this);

    }

}
