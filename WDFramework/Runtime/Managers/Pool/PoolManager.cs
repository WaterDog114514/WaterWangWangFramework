using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WDFramework
{


    /// <summary>
    /// 缓存池(对象池)模块 管理器
    /// </summary>
    public class PoolManager 
    {
        public PoolManager()
        {
            InstantiateModule();
        }
        /// <summary>
        /// 所有池子管理 池子dic(标识——池子）
        /// </summary>
        private Dictionary<string, Pool> dic_Pool = new Dictionary<string, Pool>();
        //框架设置
        private GameProjectSettingData SettingData;
        public void InstantiateModule()
        {
            //加载框架配置文件
            LoadSettingData();
            //开发中，如果开启可视化，则创建root
#if UNITY_EDITOR
            if (root == null)
            {
                root = new GameObject("PoolRoot").transform;
            }
#endif
        }
        //加载基本的配置文件
        private void LoadSettingData()
        {
            //初始化加载配置操作
            SettingData = SystemSettingLoader.Instance.LoadData<GameProjectSettingData>();
        }

#if UNITY_EDITOR
        //开发可视化专用
        public Dictionary<string, Pool> Dic_Pool => dic_Pool;
        /// <summary>
        /// 放抽屉的柜子
        /// </summary>
        public Transform root;
        //抽屉管理，只有当使用编辑器开发采用哦
        private Dictionary<string, Transform> dic_VolumeTransform = new Dictionary<string, Transform>();
#endif

        /// <summary>
        /// 将使用中的对象“销毁”，加入缓存池
        /// </summary>
        /// <param name="obj"></param>
        public void EnterObj(Obj obj)
        {
            //如果进池子时候，不存在池子就创建一个新池子
            if (!dic_Pool.ContainsKey(obj.PoolIdentity))
            {
                //没有就创建再放
                Pool pool = CreateNewPool(obj);
                //放取一波记录一下
                pool.Operation_EnterPool(obj);
                return;
            }
            //如果有池子那么直接放入
            dic_Pool[obj.PoolIdentity].Operation_EnterPool(obj);
        }
        //从池子中取东西
        public T ExitObj<T>(string identity) where T : Obj
        {
            //先检验是否有这个池子
            if (dic_Pool.ContainsKey(identity))
            {
                return dic_Pool[identity].Operation_QuitPool() as T;
            }
            //没有返回null
            return null;
        }
        /// <summary>
        /// 第一次创建物体时，使用池子记录
        /// </summary>
        public void FirstCreateRecord(string identity, Obj obj)
        {
            //第一次不存在，就先创建个池子
            if(!dic_Pool.ContainsKey(identity))CreateNewPool(obj);
            dic_Pool[identity].Operation_FirstCreateRecord(obj);
        }
        //新创建一个池子
        private Pool CreateNewPool(Obj obj, PoolSetting setting = null)
        {
            Pool pool = null;
            //对于对象池设置，如果没有设置组使用全局的默认预设，可以在框架设置中修改
            if (setting == null) setting = SettingData.defaultPoolSetting;
            //根据设置信息创建
            switch (setting.PoolType)
            {
                case Pool.E_PoolType.Circulate:
                    pool = new CircuPool(setting.MaxCount, obj.PoolIdentity);
                    break;
                case Pool.E_PoolType.Expansion:
                    pool = new ExtensionPool(setting.MaxCount, obj.PoolIdentity);
                    break;
                case Pool.E_PoolType.Fixed:
                    pool = new FixedPool(setting.MaxCount, obj.PoolIdentity);
                    break;

            }
            //添加
            dic_Pool.Add(obj.PoolIdentity, pool);
            return pool;
        }
        /// <summary>
        /// 过场景时候清除数据
        /// </summary>
        public void Clear()
        {
            dic_Pool.Clear();
        }
    }
    //池子预设，可以通过Excel进行分组加载，提前需要绑定Identity
    [Serializable]
    public class PoolSetting
    {
        public Pool.E_PoolType PoolType;
        public int MaxCount;
    }

}