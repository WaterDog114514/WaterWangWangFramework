using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFactory<T> : Singleton<T> where T : class, new()
{
    public BaseFactory()
    {
        InitializeFactory();
    }

    /// <summary>
    /// ��ʼ��������������������������ˮ�ߵĹ���
    /// </summary>
    public abstract void InitializeFactory();

    /// <summary>
    /// �������õĲ�Ʒģ�壬��ʱ��ֱ���������Ƽ��ɣ������ٽ�����ˮ���ˡ�
    /// </summary>
    protected Dictionary<string, IFactoryProduct> productTempPlate;
    /// <summary>
    /// ��ˮ�ߣ�ר�������ɹ�����
    /// </summary>
    public IFactoryPipeline pipeline { get;protected set;}

    //����������ȥʵ�ְ�
    public void Inti_LoadDataInAndroid()
    {
        throw new System.NotImplementedException();
    }
    public void Inti_LoadDataInWindows()
    {
        throw new System.NotImplementedException();
    }
}
