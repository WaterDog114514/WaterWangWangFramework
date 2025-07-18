/// <summary>
/// ����UIģ��Ļ���
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
