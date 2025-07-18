using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 地形编辑器窗口射线检测助手
/// </summary>
public class TopViewRaycastHandler
{
    public TopViewRaycastHandler(TopViewTerrainCellDrawHelper editorInstance)
    {
        this.editorInstance = editorInstance;
    }
    private TopViewTerrainCellDrawHelper editorInstance;
    private Event InputEvent => editorInstance.InputEvent;
    public Dictionary<QuadNode<TerrainCellInfo>, Rect> dic_DrawNodes => editorInstance.dic_DrawNodes;
    public void Update()
    {

        if (InputEvent == null || dic_DrawNodes == null) { return;}
        Debug.Log(editorInstance.State_CurrentDrawTool);
        if(InputEvent.type == EventType.MouseDown&& editorInstance.State_CurrentDrawTool != TopViewTerrainCellDrawHelper.E_DrawToolType.None)
        {
            RaycastCheck();
        }
    }
    //射线检测主要逻辑
    public void RaycastCheck()
    {
        //简单2d射线检测吧
        Vector2 mousePos = InputEvent.mousePosition;
        Debug.Log(mousePos);
        foreach (var node in dic_DrawNodes.Keys)
        {
            if (dic_DrawNodes[node].Contains(mousePos))
            {
                //获取当前画笔序列号
                int DrawIndex = editorInstance.Index_DrawToolPresets;
                // 根据当前工具类型执行不同逻辑
                switch (editorInstance.State_CurrentDrawTool)
                {
                    case TopViewTerrainCellDrawHelper.E_DrawToolType.Terrian:
                        node.data.SetTerrainCellInfo<TerrainCellInfo.E_TerrainCellType>(DrawIndex);
                        break;
                    case TopViewTerrainCellDrawHelper.E_DrawToolType.MonsterSpawn:
                        node.data.SetTerrainCellInfo<TerrainCellInfo.E_MonsterCellType>(DrawIndex);
                        break;
                    case TopViewTerrainCellDrawHelper.E_DrawToolType.eraser:

                        break;
                }
                Debug.Log(node.Rect.rect);
                editorInstance.window.Repaint();
            }
        }


        ////不画模式下直接跳过
        //if (editorInstance.State_CurrentDrawTool == TopViewTerrainCellDrawHelper.E_DrawToolType.None) return;
        ////画的话就继续吧
        //Ray ray = HandleUtility.GUIPointToWorldRay(InputEvent.mousePosition);
        //if (Physics.Raycast(ray, out RaycastHit hitInfo))
        //{
        //    Vector3 hitPoint = hitInfo.point;
        //    //转换成四叉树坐标（射点坐标-地形的世界坐标）
        //    Vector3 cellPosition = hitPoint - editorInstance.QuadTerrainComponent.transform.position;
        //    
        //    
        //    //取得该射线得到节点
        //    var node = editorInstance.QuadTerrainComponent.QuadData.Tree.GetQuadNodeFromPosition      (cellPosition);
        //    // 根据当前工具类型执行不同逻辑
        //    if (editorInstance != null && node != null)
        //    {
        //        switch (editorInstance.State_CurrentDrawTool)
        //        {
        //            case TopViewTerrainCellDrawHelper.E_DrawToolType.Terrian:
        //                node.data.SetTerrainCellInfo<TerrainCellInfo.E_TerrainCellType>(DrawIndex);
        //                break;
        //            case TopViewTerrainCellDrawHelper.E_DrawToolType.MonsterSpawn:
        //                node.data.SetTerrainCellInfo<TerrainCellInfo.E_MonsterCellType>(DrawIndex);
        //                break;
        //            case TopViewTerrainCellDrawHelper.E_DrawToolType.eraser:

        //                break;
        //        }
        //    }
        //}
    }
}
