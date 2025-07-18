using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using WDFramework;

/// <summary>
/// ��ɫ����������
/// </summary>
public class CharacterAnimationDriver : CharacterComponent, ICharacterAssetLoad
{
    /// <summary>
    /// �洢ÿ��״̬���֣���hashֵ����ʽ��ʾ
    /// ��Ҫ��ע�ᣬ������
    /// </summary>
    protected Dictionary<E_AnimationHash, int> dic_StateHash = new Dictionary<E_AnimationHash, int>();
    /// <summary>
    /// ��ɫ���ϵ�״̬��
    /// </summary>
    private Animator animator;
    /// <summary>
    /// ״̬����spine���
    /// </summary>
    private SkeletonMecanim mecanim;
    public CharacterAnimationDriver(BaseCharacter baseCharacter, GameObject animatorObj, string skeletonABPath, string ControllerPath) : base(baseCharacter)
    {
        IntiAnimationData(animatorObj, skeletonABPath, ControllerPath);
    }
    //��ʼ����������
    private void IntiAnimationData(GameObject animatorObj, string SkeletonPath, string ControllerPath)
    {
        //ȡ��������Ķ������
        mecanim = animatorObj.GetComponent<SkeletonMecanim>();
        if (mecanim == null)
            mecanim = animatorObj.AddComponent<SkeletonMecanim>();

        animator = animatorObj.GetComponent<Animator>();
        if (animator == null)
            animator = animatorObj.AddComponent<Animator>();
        //ͨ��AB������Controller�͹������ݲ���ɸ�ֵ
        //ResLoader.Instance.LoadAB_Async<SkeletonDataAsset>(E_ABPackName.character, SkeletonPath, (data) =>
        //{
        //    mecanim.skeletonDataAsset = data;
        //    mecanim.Initialize(true);
        //});
        ////�ж��ǲ��Ǹ��ǻ������������
        //ResLoader.Instance.LoadAB_Async<RuntimeAnimatorController>(E_ABPackName.character, ControllerPath, (controller) =>
        //{
        //    animator.runtimeAnimatorController = controller;
        //});
    }

    protected void RegisterParameterHash(E_AnimationHash e_ParameterHashName)
    {

        int hashValue = Animator.StringToHash(e_ParameterHashName.ToString());
        //��Key���
        if (dic_StateHash.ContainsKey(e_ParameterHashName))
            dic_StateHash[e_ParameterHashName] = hashValue;
        //û�����
        else
        {
            dic_StateHash.Add(e_ParameterHashName, hashValue);
        }
    }
    public override void IntializeComponent()
    {
        Inti_AddParameterHash();
        Inti_RegisterAnimationEvent();
    }

    public override void UpdateComponent()
    {

    }
    /// <summary>
    /// ����ĳ����״̬���ɴ˽ű�ͨ��EventManager�ֲ�ע�ᣬ֪ͨ�����ű�ʹ��
    /// </summary>
    // public void EnterAnimationState()
    public void Inti_RegisterAnimationEvent()
    {
        //ע�ᶯ������ӷ������������������ݸ�
        eventManager.AddEventListener<E_AnimationHash>(E_CharacterEvent.SetAnimationTrigger, (Name) =>
        {
            animator.SetTrigger(dic_StateHash[Name]);
        });

        eventManager.AddEventListener<E_AnimationHash, bool>(E_CharacterEvent.SetAnimationBool, (Name, value) =>
        {
            animator.SetBool(dic_StateHash[Name], value);
        });
        eventManager.AddEventListener<E_AnimationHash, float>(E_CharacterEvent.SetAnimationValue, (Name, value) =>
        {
            animator.SetFloat(dic_StateHash[Name], value);
        });

    }
    //ע�ᶯ��������Hash
    private void Inti_AddParameterHash()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Ӧ�ø���ģ���Զ������
        //!!!!!!!!!!!!!!!!!!!!!!!!!!
        //RegisterParameterHash(E_AnimationHash.MoveSpeed);
        //RegisterParameterHash(E_AnimationHash.NormalAttack1);
        //RegisterParameterHash(E_AnimationHash.NormalAttack2);
        //RegisterParameterHash(E_AnimationHash.Idle);
        //RegisterParameterHash(E_AnimationHash.Moving);
    }
    /// <summary>
    /// ����ĳ����״̬��Motion
    /// </summary>
    public void SetAnimatorStateMotion()
    {



    }
    //����״̬����Ϣ
    public void I_LoadCharacterAsset(CharacterAsset asset)
    {
        throw new System.NotImplementedException();
    }
}
