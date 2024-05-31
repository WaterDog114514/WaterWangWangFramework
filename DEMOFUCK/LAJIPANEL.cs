using System;
using UnityEngine;

public class MyMonoSingleton : Singleton_AutoMono<MyMonoSingleton>
{
    GameObj gameObj;
    private void Awake()
    {
       gameObj.SetPosition(new Vector3(11,45,14)).SetRotation(transform.rotation).SetParent(transform);
    }

}
class MyDataObj : DataObj
{

}