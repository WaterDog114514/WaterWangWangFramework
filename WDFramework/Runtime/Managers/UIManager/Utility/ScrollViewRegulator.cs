using UnityEngine;

/// <summary>
/// 滚动区域自适应调节器
/// 能够调整当滚动区域item增减时的rect大小
/// </summary>
public  class ScrollViewRegulator
{
    private RectTransform ScrollContent;
    /// <summary>
    /// 整个滚动内容的rect
    /// </summary>
    private RectTransform Container;
    /// <summary>
    /// 单个滚动物体的rect
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
    /// 自动调节
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
