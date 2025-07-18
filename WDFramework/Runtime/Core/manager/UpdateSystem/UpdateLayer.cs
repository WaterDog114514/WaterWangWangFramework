using System;
using UnityEngine.Events;
/// <summary>
/// 更新层，包含
/// </summary>
public class UpdateLayer
{
    /// <summary>
    /// 是否冻结生命周期
    /// </summary>
    public bool isFreezed;
    public event UnityAction UpdateEvent;
    public event UnityAction FixedUpdateEvent;
    public event UnityAction LateUpdateEvent;

    public void InvokeUpdate()
    {
        if (isFreezed) return;
        UpdateEvent?.Invoke();
    }

    public void InvokeFixedUpdate()
    {
        if (isFreezed) return;
        FixedUpdateEvent?.Invoke();
    }

    public void InvokeLateUpdate()
    {
        if (isFreezed) return;
        LateUpdateEvent?.Invoke();
    }
}
