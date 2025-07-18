using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WDFramework
{
    //Get��Create������
    //Get�����ȴӶ������ȡ����������û�л��Զ�Createһ�����Ⱥ������ٻ������أ�
    //Create��ֱ���½�һ�����󣬵��������ȿ��Ƕ���أ��Ƽ�ʹ��Get

    /// <summary>
    /// ������Ϸ�������������¼������ʵ���������Ϸ���󣬽�������GameObject�ͷ�Mono�����ݶ���
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
        //��ȡ���ݶ���Ӷ����֮��
        public T GetDataObj<T>() where T : DataObj
        {
            return GetDataObj(typeof(T)) as T;
        }
        //����д�ܰ�ȫ������ò��������㶨��û�п��ж���Ҳ���ᷢ������
        private DataObj GetDataObj(Type type)
        {
            var dataObj = poolManager.ExitObj<DataObj>(type.Name);
            //�Ӷ����ȡ����ֱ�ӷ��ذ�
            if (dataObj != null) return dataObj;
            //û�ж����/ȡ���� �Ǿ�ֱ�Ӵ���һ���¶��󣬵ȵ������Լ����ض���ص�
            return CreateDataObject(type);
        }
        /// <summary>
        /// �Ӷ������ȡ��GameObj��ȡ�����᷵��null
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public GameObj GetGameObjFromPool(string Identity)
        {
            return poolManager.ExitObj<GameObj>(Identity);
        }
        /// <summary>
        /// ͨ��Ԥ������ʵ��������һ��GameObj
        /// </summary>
        /// <returns></returns>
        public GameObj CreateGameObj(GameObject Prefab, string identity = null)
        {
            GameObject Instance = GameObject.Instantiate(Prefab);
            GameObj obj = new GameObj(Instance);
            //���ö���ص�Identity��ʶΪ����
            if (string.IsNullOrEmpty(identity))
                obj.PoolIdentity = Prefab.name;
            //ͨ��Identity����
            else obj.PoolIdentity = identity;
            //���ö�������Ϊ��ʶ
            Instance.name = obj.PoolIdentity;

            // ��һ�δ������������壬�ȼ�¼��ʹ��
             poolManager.FirstCreateRecord(identity, obj);
            return obj;
        }
        /// <summary>
        /// ����һ�����ݶ���
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
                Debug.LogError($"ʵ��������{type.Name}���̳���DataObj��");
                return null;
            }
            DataObj obj = Activator.CreateInstance(type) as DataObj;
            //�������ʶ��Ϊ����������
            obj.PoolIdentity = type.Name;
            return obj;
        }
        /// <summary>
        /// ǳ���٣�����������ض���
        /// </summary>
        /// <param name="obj"></param>
        public void DestroyObj(Obj obj)
        {
            poolManager.EnterObj(obj);
        }
        /// <summary>
        /// ������٣�������ȫ���ڴ����Ƴ�
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
//ͨ��Ԥ������ʽ��������ʱ�ò�����
//����Ԥ�������
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
//ͨ��Ԥ���崴��������ʱ����
//public GameObj CreateGameObject(PrefabInfo info)
//{
//    //ԭʼ����
//    GameObject gameObj = UnityEngine.Object.Instantiate(info.res);
//    GameObj obj = new GameObj(gameObj);
//    //����id
//    GiveObjID(obj);
//    //ֻ�ж����Լ����Ҫ
//    if (info is PoolPrefabInfo)
//    {
//        gameObj.name = (info as PoolPrefabInfo).identity;
//        obj.PoolGroup = (info as PoolPrefabInfo).PoolGroup;
//    }
//    gameObj.AddComponent<GameObjectInstance>().Inti(obj);
//    return obj;
//}

//�Ӷ�����л������
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