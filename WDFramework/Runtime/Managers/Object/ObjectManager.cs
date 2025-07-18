using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDFramework
{
    //Get和Create的区别：
    //Get会优先从对象池中取，如果对象池没有会自动Create一个，等后续销毁会进对象池；
    //Create会直接新建一个对象，但不会优先考虑对象池，推荐使用Get

    /// <summary>
    /// 所有游戏对象管理器，记录着所有实例化后的游戏对象，仅仅包括GameObject和非Mono的数据对象
    /// </summary>
    public class ObjectManager : Singleton<ObjectManager>
    {
        // private Dictionary<int, Obj> dicObj = new Dictionary<int, Obj>();
        public PoolManager poolManager;
        public ObjectManager()
        {
            InstantiateModule();
        }
        public void InstantiateModule()
        {
            poolManager = new PoolManager();
        }
        //获取数据对象从对象池之中
        public T GetDataObj<T>() where T : DataObj
        {
            return GetDataObj(typeof(T)) as T;
        }
        //这样写很安全，不会得不到，就算定容没有空闲对象也不会发生意外
        private DataObj GetDataObj(Type type)
        {
            var dataObj = poolManager.ExitObj<DataObj>(type.Name);
            //从对象池取到就直接返回吧
            if (dataObj != null) return dataObj;
            //没有对象池/取不到 那就直接创建一个新对象，等到它会自己返回对象池的
            return CreateDataObject(type);
        }
        /// <summary>
        /// 从对象池中取到GameObj，取不到会返回null
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public GameObj GetGameObjFromPool(string Identity)
        {
            return poolManager.ExitObj<GameObj>(Identity);
        }
        /// <summary>
        /// 通过预设体新实例化创建一个GameObj
        /// </summary>
        /// <returns></returns>
        public GameObj CreateGameObj(GameObject Prefab, string identity = null)
        {
            GameObject Instance = GameObject.Instantiate(Prefab);
            GameObj obj = new GameObj(Instance);
            //设置对象池的Identity标识为名字
            if (string.IsNullOrEmpty(identity))
                obj.PoolIdentity = Prefab.name;
            //通过Identity设置
            else obj.PoolIdentity = identity;
            //设置对象名字为标识
            Instance.name = obj.PoolIdentity;

            // 第一次创建该类型物体，先记录到使用
             poolManager.FirstCreateRecord(identity, obj);
            return obj;
        }
        /// <summary>
        /// 创建一般数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateDataObject<T>() where T : DataObj, new()
        {
            return CreateDataObject(typeof(T)) as T;
        }
        public DataObj CreateDataObject(Type type)
        {
            if (!type.IsSubclassOf(typeof(DataObj)))
            {
                Debug.LogError($"实例化错误！{type.Name}不继承自DataObj类");
                return null;
            }
            DataObj obj = Activator.CreateInstance(type) as DataObj;
            //给予其标识，为它的类型名
            obj.PoolIdentity = type.Name;
            return obj;
        }
        /// <summary>
        /// 浅销毁，将其放入对象池而已
        /// </summary>
        /// <param name="obj"></param>
        public void DestroyObj(Obj obj)
        {
            poolManager.EnterObj(obj);
        }
        /// <summary>
        /// 深度销毁，将其完全从内存中移除
        /// </summary>
        public void DeepDestroyObj(Obj obj)
        {
            if (obj is GameObj)
            {
                GameObject.Destroy((obj as GameObj).Instance);
            }
        }
    }
}
//通过预设体形式创建，暂时用不到哈
//创建预设体对象
//
//
//
//
//
//
//
//
//
//
//通过预设体创建对象，暂时不用
//public GameObj CreateGameObject(PrefabInfo info)
//{
//    //原始创建
//    GameObject gameObj = UnityEngine.Object.Instantiate(info.res);
//    GameObj obj = new GameObj(gameObj);
//    //给予id
//    GiveObjID(obj);
//    //只有对象池约束才要
//    if (info is PoolPrefabInfo)
//    {
//        gameObj.name = (info as PoolPrefabInfo).identity;
//        obj.PoolGroup = (info as PoolPrefabInfo).PoolGroup;
//    }
//    gameObj.AddComponent<GameObjectInstance>().Inti(obj);
//    return obj;
//}

//从对象池中获得物体
//public GameObj GetGameObjFromPool(PrefabInfo info)
//{
//    return poolManager.GetGameObj(info);
//}
//public GameObj GetGameObjFromPool(int id)
//{
//    return poolManager.GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromID(id));
//}
//public GameObj GetGameObjFromPool(string PrefabName)
//{
//    return poolManager.GetGameObj(PrefabLoaderManager.Instance.GetPrefabInfoFromName(PrefabName));
//}