using System;
using UnityEngine;

public class MyMonoSingleton : Singleton_AutoMono<MyMonoSingleton>
{
    private void Awake()
    {
        // 直接创建数据对象（通过泛型）
        MyDataObj dataObj = ObjectManager.Instance.CreateDataObject<MyDataObj>();

        // 直接创建数据对象（通过 Type）
        Type dataType = typeof(MyDataObj);
        MyDataObj dataObj2 = ObjectManager.Instance.CreateDataObject(dataType) as MyDataObj;
    }

}
class MyDataObj : DataObj
{

}