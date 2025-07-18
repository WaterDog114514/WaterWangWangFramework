using UnityEngine;
/// <summary>
/// 角色单位的器件  每个组件只作为单位整体的
/// </summary>
public abstract class CharacterComponent
{
    /// <summary>
    /// 和其他模块进行交互的核心玩意，观察者本体
    /// </summary>
    protected EventManager<E_CharacterEvent> eventManager;
    protected BaseCharacter SelfCharacter;
    /// <summary>
    /// 为角色器件初始化
    /// </summary>
    public abstract void IntializeComponent();
    /// <summary>
    /// 挂件每帧更新逻辑方法
    /// </summary>
    public abstract void UpdateComponent();

    public CharacterComponent(BaseCharacter baseCharacter)
    {
        eventManager = baseCharacter.eventManager;
        SelfCharacter = baseCharacter;
        IntializeComponent() ;
    }
}
