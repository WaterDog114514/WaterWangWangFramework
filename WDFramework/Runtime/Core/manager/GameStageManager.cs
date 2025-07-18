using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 游戏阶段管理器（框架层核心系统）
/// 职责：
/// 1. 管理所有游戏阶段的切换
/// 2. 维护阶段进入/退出事件的优先级回调系统
/// 3. 协调框架层与项目层的阶段衔接
/// </summary>
public class GameStageManager : Singleton<GameStageManager>, IKernelSystem
{
    /// <summary>
    /// 当前所处的游戏阶段（只读）
    /// </summary>
    public GameStage CurrentStage { get; private set; }

    /// <summary>
    /// 初始化内核系统（框架层初始化时调用）
    /// </summary>
    public void InitializedKernelSystem()
    {
        // 注册全局阶段切换事件监听（优先级0最高）
        EventCenterSystem.Instance.AddEventListener<E_FrameworkEvent, GameStage>(
            E_FrameworkEvent.ChangeGameStage,
            SwitchStage,
            0
        );
    }

    /// <summary>
    /// 切换到首个阶段（框架初始化完成后调用）
    /// </summary>
    public void SwitchToFirstStage()
    {
        // 通过反射获取首个阶段实例
        var firstStage = GetInitialStageInstance();
        SwitchStage(firstStage);
    }

    /// <summary>
    /// 执行阶段切换（核心切换逻辑）
    /// </summary>
    /// <param name="NextStage">目标阶段实例</param>
    public void SwitchStage(GameStage NextStage)
    {
        if (NextStage == null) return;
        if (CurrentStage == NextStage) return;

        Debug.Log($"正在切换阶段：{CurrentStage?.GetType().Name} -> {NextStage.GetType().Name}");
        //完成整个阶段性操作
        UpdateSystem.Instance.StartCoroutine(FinishStageOperator(NextStage));

    }
    /// <summary>
    /// 完成整个阶段性操作
    /// </summary>
    /// <returns></returns>
    private IEnumerator FinishStageOperator(GameStage NewStage)
    {

        //触发退出老阶段的全局回调
        EventCenterSystem.Instance.TriggerEvent(E_FrameworkEvent.OnExitGameStage, NewStage);
        //再注册新系统
        EventCenterSystem.Instance.TriggerEvent(E_FrameworkEvent.OnEnterGameStageRegisterGameSystem, NewStage);
        //先等待完成新阶段整个阶段性工作
        yield return UpdateSystem.Instance.StartCoroutine(NewStage.StageOperator());
        //先触发进入新阶段全局回调
        EventCenterSystem.Instance.TriggerEvent(E_FrameworkEvent.OnEnterGameStage, NewStage);
        CurrentStage = NewStage;
        //若存在自动切换下一阶段，那么则切换
        if (NewStage.nextAutoChangeStage != null)
        {
            //此处存在递归
            SwitchStage(NewStage.nextAutoChangeStage);
        }
        yield break;
    }
    /// <summary>
    /// 框架层定义的初始阶段基类（必须被项目层继承）
    /// 继承时必须只有一个
    /// </summary>
    public abstract class InitialStage : GameStage
    {
        /// <summary>
        /// 标记为初始阶段（用于反射识别）
        /// </summary>
        public virtual bool IsInitialStage => true;
    }
 
    /// <summary>
    /// 获取初始阶段实例（框架层调用）
    /// </summary>
    private InitialStage GetInitialStageInstance()
    {
        // 通过反射查找所有InitialStage的子类
        var initialStageTypes = ReflectionHelper.GetSubclasses(typeof(InitialStage));

        if (initialStageTypes.Count == 0)
        {
            Debug.LogWarning("未找到初始阶段实现！请在项目层创建继承自GameStageManager.InitialStage的类");
            return null;
        }
        if (initialStageTypes.Count > 1)
        {
            throw new Exception($"找到多个初始阶段实现：{string.Join(", ", initialStageTypes)}");
        }

        // 创建实例并验证
        var stage = Activator.CreateInstance(initialStageTypes[0]) as InitialStage;

        if (!stage.IsInitialStage)
        {
            throw new Exception($"初始阶段类 {stage.GetType().Name} 必须保持IsInitialStage返回true");
        }

        return stage;
    }
}