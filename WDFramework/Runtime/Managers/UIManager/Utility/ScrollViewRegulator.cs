using UnityEngine;

/// <summary>
/// ������������Ӧ������
/// �ܹ���������������item����ʱ��rect��С
/// </summary>
public  class ScrollViewRegulator
{
    private RectTransform ScrollContent;
    /// <summary>
    /// �����������ݵ�rect
    /// </summary>
    private RectTransform Container;
    /// <summary>
    /// �������������rect
    /// </summary>
    private GameObject ItemPrefab;
  
    public ScrollViewRegulator(RectTransform _Scroll, GameObject itemPrefab)
    {
        this.ScrollContent = _Scroll;
        this.ItemPrefab = itemPrefab;
    }
    public void ResetContainer(RectTransform container)
    {
        Container = container;
    }
    /// <summary>
    /// �Զ�����
    /// </summary>
    public void AutoAdujustContentHeight()
    {
        var layerCount = Container.childCount;
        var itemRect = ItemPrefab.GetComponent<RectTransform>();    
        var singleHeight = itemRect.sizeDelta.y;
        var TotalHeight = singleHeight * layerCount;
        ScrollContent.sizeDelta = new Vector2(ScrollContent.sizeDelta.x, TotalHeight);
    }
}
