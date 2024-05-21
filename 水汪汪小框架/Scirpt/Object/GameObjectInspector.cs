using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// 游戏对象可视化面板(编辑模式才用)
/// </summary>
[CustomEditor(typeof(GameObjectInstance))]

public class GameObjectInspector : Editor
{
    private GameObj gameObj;
    private void OnEnable()
    {
        gameObj = (target as GameObjectInstance).gameObj;
    }
    // Start is called before the first frame update
    public override void OnInspectorGUI()
    {
        if (gameObj == null)
        {
            GUILayout.Label("请通过代码创建对象实例！！");
            return;
        }
        base.OnInspectorGUI();

        EditorGUILayout.TextField("对象ID：", gameObj.ID.ToString());
        //对象池信息
        //string PoolInfo = PoolManager.Instance.Dic_Pool.ContainsKey(gameObj.PoolIdentity) ? gameObj.PoolIdentity : "暂未创建池子";
        //string PoolLimit = PoolManager.Instance.Dic_Pool.ContainsKey(gameObj.PoolIdentity) ? $"当前空闲：{PoolManager.Instance.Dic_Pool[gameObj.PoolIdentity].PoolQueue.Count}  使用中：{PoolManager.Instance.Dic_Pool[gameObj.PoolIdentity].UsingQueue.Count}  最大上限：{PoolManager.Instance.Dic_Pool[gameObj.PoolIdentity].maxCount}" : "暂未创建池子";
        //EditorGUILayout.TextField("对象池：", PoolInfo);
        //EditorGUILayout.LabelField("对象池使用：", PoolLimit);

    }
}