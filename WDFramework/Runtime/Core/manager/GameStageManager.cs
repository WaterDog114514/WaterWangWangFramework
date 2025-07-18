using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ��Ϸ�׶ι���������ܲ����ϵͳ��
/// ְ��
/// 1. ����������Ϸ�׶ε��л�
/// 2. ά���׶ν���/�˳��¼������ȼ��ص�ϵͳ
/// 3. Э����ܲ�����Ŀ��Ľ׶��ν�
/// </summary>
public class GameStageManager : Singleton<GameStageManager>, IKernelSystem
{
    /// <summary>
    /// ��ǰ��������Ϸ�׶Σ�ֻ����
    /// </summary>
    public GameStage CurrentStage { get; private set; }

    /// <summary>
    /// ��ʼ���ں�ϵͳ����ܲ��ʼ��ʱ���ã�
    /// </summary>
    public void InitializedKernelSystem()
    {
        // ע��ȫ�ֽ׶��л��¼����������ȼ�0��ߣ�
        EventCenterSystem.Instance.AddEventListener<E_FrameworkEvent, GameStage>(
            E_FrameworkEvent.ChangeGameStage,
            SwitchStage,
            0
        );
    }

    /// <summary>
    /// �л����׸��׶Σ���ܳ�ʼ����ɺ���ã�
    /// </summary>
    public void SwitchToFirstStage()
    {
        // ͨ�������ȡ�׸��׶�ʵ��
        var firstStage = GetInitialStageInstance();
        SwitchStage(firstStage);
    }

    /// <summary>
    /// ִ�н׶��л��������л��߼���
    /// </summary>
    /// <param name="NextStage">Ŀ��׶�ʵ��</param>
    public void SwitchStage(GameStage NextStage)
    {
        if (NextStage == null) return;
        if (CurrentStage == NextStage) return;

        Debug.Log($"�����л��׶Σ�{CurrentStage?.GetType().Name} -> {NextStage.GetType().Name}");
        //��������׶��Բ���
        UpdateSystem.Instance.StartCoroutine(FinishStageOperator(NextStage));

    }
    /// <summary>
    /// ��������׶��Բ���
    /// </summary>
    /// <returns></returns>
    private IEnumerator FinishStageOperator(GameStage NewStage)
    {

        //�����˳��Ͻ׶ε�ȫ�ֻص�
        EventCenterSystem.Instance.TriggerEvent(E_FrameworkEvent.OnExitGameStage, NewStage);
        //��ע����ϵͳ
        EventCenterSystem.Instance.TriggerEvent(E_FrameworkEvent.OnEnterGameStageRegisterGameSystem, NewStage);
        //�ȵȴ�����½׶������׶��Թ���
        yield return UpdateSystem.Instance.StartCoroutine(NewStage.StageOperator());
        //�ȴ��������½׶�ȫ�ֻص�
        EventCenterSystem.Instance.TriggerEvent(E_FrameworkEvent.OnEnterGameStage, NewStage);
        CurrentStage = NewStage;
        //�������Զ��л���һ�׶Σ���ô���л�
        if (NewStage.nextAutoChangeStage != null)
        {
            //�˴����ڵݹ�
            SwitchStage(NewStage.nextAutoChangeStage);
        }
        yield break;
    }
    /// <summary>
    /// ��ܲ㶨��ĳ�ʼ�׶λ��ࣨ���뱻��Ŀ��̳У�
    /// �̳�ʱ����ֻ��һ��
    /// </summary>
    public abstract class InitialStage : GameStage
    {
        /// <summary>
        /// ���Ϊ��ʼ�׶Σ����ڷ���ʶ��
        /// </summary>
        public virtual bool IsInitialStage => true;
    }
 
    /// <summary>
    /// ��ȡ��ʼ�׶�ʵ������ܲ���ã�
    /// </summary>
    private InitialStage GetInitialStageInstance()
    {
        // ͨ�������������InitialStage������
        var initialStageTypes = ReflectionHelper.GetSubclasses(typeof(InitialStage));

        if (initialStageTypes.Count == 0)
        {
            Debug.LogWarning("δ�ҵ���ʼ�׶�ʵ�֣�������Ŀ�㴴���̳���GameStageManager.InitialStage����");
            return null;
        }
        if (initialStageTypes.Count > 1)
        {
            throw new Exception($"�ҵ������ʼ�׶�ʵ�֣�{string.Join(", ", initialStageTypes)}");
        }

        // ����ʵ������֤
        var stage = Activator.CreateInstance(initialStageTypes[0]) as InitialStage;

        if (!stage.IsInitialStage)
        {
            throw new Exception($"��ʼ�׶��� {stage.GetType().Name} ���뱣��IsInitialStage����true");
        }

        return stage;
    }
}