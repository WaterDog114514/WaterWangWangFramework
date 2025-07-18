using System;
using UnityEngine;

/// <summary>
/// ��Դ��Ϣ��
/// </summary>
public class Res
{
    public Res(Type assetType)
    {
        AssetType = assetType;
    }
    //���ü���
    public int refCount { get; private set; }
    //��Դ
    public UnityEngine.Object Asset;
    private Type AssetType = null;
    public T GetAsset<T>() where T : UnityEngine.Object
    {
        if (Asset == null)
        {
            Debug.LogError("��ȡ��Դʧ�ܣ����������ڽ����첽�����У�����ͨ���첽��ȡ");
            return default(T);
        }
        //ɵ��˾����룬������Ҫ��������
        if (AssetType != typeof(T))
        {
            Debug.LogError($"��ȡ��Դʧ��,����ԴΪ{AssetType.Name}���ͣ�����ͨ��{typeof(T).Name}��ȡ");
            return default(T);
        }
        return Asset as T;
    }
    public void AddrefCount()
    {
        ++refCount;
    }
    public void SubrefCount()
    {
        --refCount;
        if (refCount < 0)
            Debug.LogError("���ü���С��0�ˣ�����ʹ�ú�ж���Ƿ����ִ��");
    }
}
