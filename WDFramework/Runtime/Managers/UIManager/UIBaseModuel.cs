/// <summary>
/// 所有UI模块的基类
/// </summary>
public abstract class UIBaseModuel
{
    public UIBaseModuel()
    {
        InitializeModuel();
    }
    protected EventManager<E_UIManagerEvent> eventManager => UIManagementSystem.Instance.eventManager;
    public abstract void InitializeModuel();
}
