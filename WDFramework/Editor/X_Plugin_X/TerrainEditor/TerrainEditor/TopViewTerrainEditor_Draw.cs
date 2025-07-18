//绘制部分
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class TopViewTerrainCellDrawHelper
{
    private float CellSideLength;
    private Rect TopViewDrawRect;
    public Dictionary<QuadNode<TerrainCellInfo>, Rect> dic_DrawNodes;
    //添加节点
    public void LoadDrawNodeDic(Rect TopViewDrawRect)
    {
        dic_DrawNodes = new Dictionary<QuadNode<TerrainCellInfo>, Rect>();
        // 更新绘制区域和单元格大小
        this.TopViewDrawRect = TopViewDrawRect;
        CellSideLength = TopViewDrawRect.width / (Mathf.Pow(2, QuadTerrainComponent.QuadData.Tree.MaxDepth));
        AddDrawNodesToDic(QuadTerrainComponent.QuadData.Tree.rootNode);
    }
    private void AddDrawNodesToDic(QuadNode<TerrainCellInfo> node)
    {
        //只画最小节点，这样省去多余的大节点了
        if (node.isDivided)
        {
            foreach (var child in node.childNodes)
            {
                AddDrawNodesToDic(child);
            }
            return;
        }
        if (dic_DrawNodes.ContainsKey(node)) return;
        dic_DrawNodes.Add(node, GetNodeDrawRect(node));

    }
    //进行坐标转换
    private Rect GetNodeDrawRect(QuadNode<TerrainCellInfo> node)
    {
        //得到单位坐标
        Vector2 UnitPosition = node.Rect.rect.position / node.Rect.width;
        //坐标换算 先乘rect的单格长宽值+rect偏移
        Vector2 DrawPosition = UnitPosition * CellSideLength + TopViewDrawRect.position;
        return new Rect(DrawPosition, Vector2.one * CellSideLength);
    }
    /// <summary>
    /// 递归绘制节点
    /// </summary>
    /// <param name="node">要绘制的节点</param>
    private void DrawNodeCell()
    {
        foreach (var node in dic_DrawNodes.Keys)
        {
            Handles.DrawSolidRectangleWithOutline(dic_DrawNodes[node], Color.clear, Color.green);
        }
    }
    private void DrawTerrainTagCell()
    {
        ////只画最小节点，这样省去多余的大节点了
        foreach (var node in dic_DrawNodes.Keys)
        {

            Handles.DrawSolidRectangleWithOutline(dic_DrawNodes[node], dic_PresetColors[node.data.CellType.ToString()].color, Color.clear);


        }
    }
}
