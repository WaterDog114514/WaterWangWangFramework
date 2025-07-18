using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 公共mono管理器,直属生命周期管理器管辖
/// 开启关闭协程直接UpdateSystem.Instance.Start/Stop Coroutine
/// </summary>
public class UpdateSystem : MonoSingleton<UpdateSystem>, IKernelSystem
{
    private Dictionary<E_UpdateLayer, UpdateLayer> updateLayers;
    public void InitializedKernelSystem()
    {
        updateLayers = new Dictionary<E_UpdateLayer, UpdateLayer>();
        //添加四大更新
        updateLayers[E_UpdateLayer.GameSystem] = new UpdateLayer();
        updateLayers[E_UpdateLayer.FrameworkSystem] = new UpdateLayer();
        updateLayers[E_UpdateLayer.UI] = new UpdateLayer();
        updateLayers[E_UpdateLayer.Voice] = new UpdateLayer();
    }
    public UpdateSystem()
    {
        //注册当阶段变更时候，自动启动新阶段的所有update

    }
    /// <summary>
    /// 添加监听函数（三合一）
    /// </summary>
    /// <param name="layer">更新层</param>
    /// <param name="updateFun">更新函数（可以是 IMonoUpdate、IMonoFixedUpdate 或 IMonoLastUpdate）</param>
    public void AddUpdateListener(E_UpdateLayer layer, IUpdate updateFun)
    {
        if (!(updateFun is IUpdate))
        {
            Debug.LogError("传入的对象未实现 IMonoUpdate、IMonoFixedUpdate 或 IMonoLastUpdate 接口");
            return;
        }
        if (!updateLayers.ContainsKey(layer))
        {
            Debug.LogError($"更新层 {layer} 不存在");
            return;
        }
        var updateLayer = updateLayers[layer];
        if (updateFun is IMonoUpdate monoUpdate)
        {
            updateLayer.UpdateEvent += monoUpdate.MonoUpdate;
        }
        if (updateFun is IMonoFixedUpdate fixedUpdate)
        {
            updateLayer.FixedUpdateEvent += fixedUpdate.MonoFixedUpdate;
        }
        if (updateFun is IMonoLastUpdate lastUpdate)
        {
            updateLayer.LateUpdateEvent += lastUpdate.MonoLastUpdate;
        }
    }

    /// <summary>
    /// 移除监听函数（三合一）
    /// </summary>
    /// <param name="layer">更新层</param>
    /// <param name="updateFun">更新函数（可以是 IMonoUpdate、IMonoFixedUpdate 或 IMonoLastUpdate）</param>
    public void RemoveUpdateListener(E_UpdateLayer layer, IUpdate updateFun)
    {
        if(!(updateFun is IUpdate))
        {
            Debug.LogError("传入的对象未实现 IMonoUpdate、IMonoFixedUpdate 或 IMonoLastUpdate 接口");
            return;
        }
        if (!updateLayers.ContainsKey(layer))
        {
            Debug.LogError($"更新层 {layer} 不存在");
            return;
        }

        var updateLayer = updateLayers[layer];
        if (updateFun is IMonoUpdate monoUpdate)
        {
            updateLayer.UpdateEvent -= monoUpdate.MonoUpdate;
        }
        if (updateFun is IMonoFixedUpdate fixedUpdate)
        {
            updateLayer.FixedUpdateEvent -= fixedUpdate.MonoFixedUpdate;
        }
        if (updateFun is IMonoLastUpdate lastUpdate)
        {
            updateLayer.LateUpdateEvent -= lastUpdate.MonoLastUpdate;
        }
    }

    /// <summary>
    /// 启用所有更新
    /// </summary>
    public void StartAllUpdate()
    {
        foreach (var layer in updateLayers.Values)
        {
            layer.isFreezed = false;
        }
    }
    /// <summary>
    /// 冻结所有更新
    /// </summary>
    public void FreezeAllUpdate()
    {
        foreach (var layer in updateLayers.Values)
        {
            layer.isFreezed = true;
        }
    }
    private void Update()
    {
        foreach (var layer in updateLayers.Values)
        {
            layer.InvokeUpdate();
        }
    }

    private void FixedUpdate()
    {
        foreach (var layer in updateLayers.Values)
        {
            layer.InvokeFixedUpdate();
        }
    }

    private void LateUpdate()
    {
        foreach (var layer in updateLayers.Values)
        {
            layer.InvokeLateUpdate();
        }
    }

}

