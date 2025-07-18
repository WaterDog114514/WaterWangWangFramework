using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using WDFramework;

/// <summary>
/// 角色动画播放者
/// </summary>
public class CharacterAnimationDriver : CharacterComponent, ICharacterAssetLoad
{
    /// <summary>
    /// 存储每个状态名字，以hash值的形式表示
    /// 需要先注册，才能用
    /// </summary>
    protected Dictionary<E_AnimationHash, int> dic_StateHash = new Dictionary<E_AnimationHash, int>();
    /// <summary>
    /// 角色身上的状态机
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 状态机的spine组件
    /// </summary>
    private SkeletonMecanim mecanim;
    public CharacterAnimationDriver(BaseCharacter baseCharacter, GameObject animatorObj, string skeletonABPath, string ControllerPath) : base(baseCharacter)
    {
        IntiAnimationData(animatorObj, skeletonABPath, ControllerPath);
    }
    //初始化动画数据
    private void IntiAnimationData(GameObject animatorObj, string SkeletonPath, string ControllerPath)
    {
        //取得最基本的动画组件
        mecanim = animatorObj.GetComponent<SkeletonMecanim>();
        if (mecanim == null)
            mecanim = animatorObj.AddComponent<SkeletonMecanim>();

        animator = animatorObj.GetComponent<Animator>();
        if (animator == null)
            animator = animatorObj.AddComponent<Animator>();
        //通过AB包加载Controller和骨骼数据并完成赋值
        //ResLoader.Instance.LoadAB_Async<SkeletonDataAsset>(E_ABPackName.character, SkeletonPath, (data) =>
        //{
        //    mecanim.skeletonDataAsset = data;
        //    mecanim.Initialize(true);
        //});
        ////判断是不是覆盖机，还是正版机
        //ResLoader.Instance.LoadAB_Async<RuntimeAnimatorController>(E_ABPackName.character, ControllerPath, (controller) =>
        //{
        //    animator.runtimeAnimatorController = controller;
        //});
    }

    protected void RegisterParameterHash(E_AnimationHash e_ParameterHashName)
    {

        int hashValue = Animator.StringToHash(e_ParameterHashName.ToString());
        //有Key情况
        if (dic_StateHash.ContainsKey(e_ParameterHashName))
            dic_StateHash[e_ParameterHashName] = hashValue;
        //没有情况
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
    /// 进入某动画状态，由此脚本通过EventManager局部注册，通知其他脚本使用
    /// </summary>
    // public void EnterAnimationState()
    public void Inti_RegisterAnimationEvent()
    {
        //注册动画的添加方法，才能让外在内容给
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
    //注册动画参数的Hash
    private void Inti_AddParameterHash()
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!!!
        //应该根据模板自定义添加
        //!!!!!!!!!!!!!!!!!!!!!!!!!!
        //RegisterParameterHash(E_AnimationHash.MoveSpeed);
        //RegisterParameterHash(E_AnimationHash.NormalAttack1);
        //RegisterParameterHash(E_AnimationHash.NormalAttack2);
        //RegisterParameterHash(E_AnimationHash.Idle);
        //RegisterParameterHash(E_AnimationHash.Moving);
    }
    /// <summary>
    /// 设置某动画状态的Motion
    /// </summary>
    public void SetAnimatorStateMotion()
    {



    }
    //加载状态机信息
    public void I_LoadCharacterAsset(CharacterAsset asset)
    {
        throw new System.NotImplementedException();
    }
}
