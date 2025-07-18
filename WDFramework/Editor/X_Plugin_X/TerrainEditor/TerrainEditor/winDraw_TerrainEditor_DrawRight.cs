using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WDEditor;



public partial class winDraw_TerrainEditor : BaseWindowDraw<WinData_TerrainEditor>
{
    //用于绘制顶部区块按钮的Index助手
    private DrawIndexHelper topToolIndexHelper = new DrawIndexHelper();
    //顶部工具栏按钮整个区域Rect
    private Rect ToolButtonRect;
    //俯视图区域Rect
    public Rect ImageDrawRect;
    //用于检测窗口大小是否更新
    private Rect LastWindowSize;
    private TopViewTerrainCellDrawHelper TopViewTerrainCellDrawHelper => window.TopViewDrawHelper;
    private TerrainTopViewCamera topCameraComponent => window.topViewCamera;
    //绘制贴图
    public void DrawRightPanel()
    {
        //右边区域，对于还没有加载成功情况下，什么都不用管
        if(!isLoadedTerrain)return;
        //先规划区域更新
        UpdateAllRect();
        //更新顶部栏区域
        topToolIndexHelper.Update(ToolButtonRect);
        RightPanelDrawHelper.Update(LeftPanelRect);
        //绘制顶部按钮
        DrawTopButton();
        //绘制渲染贴图
        DrawRenderTexture();
        //进行绘制图片框格
        TopViewTerrainCellDrawHelper.Draw();

    }
    //绘制顶部工具栏
    public void DrawTopButton()
    {

        topToolIndexHelper.BeginHorizontalLayout(6);
        GUI.Label(topToolIndexHelper.GetNextSingleRect(), "相机高度值:");
        topCameraComponent.CameraHeight = EditorGUI.FloatField(topToolIndexHelper.GetNextSingleRect(), topCameraComponent.CameraHeight);
        GUI.Label(topToolIndexHelper.GetNextSingleRect(), "相机视口大小:");
        topCameraComponent.CameraViewSize = EditorGUI.FloatField(topToolIndexHelper.GetNextSingleRect(), topCameraComponent.CameraViewSize);
        if (GUI.Button(topToolIndexHelper.GetNextSingleRect(), "刷新摄像机"))
        {
            topCameraComponent.RenderCamera();
        }
        if (GUI.Button(topToolIndexHelper.GetNextSingleRect(), "刷新线框格"))
        {
            TopViewTerrainCellDrawHelper.LoadDrawNodeDic(ImageDrawRect);
        }
        topToolIndexHelper.EndHorizontalLayout();
    }
    //绘制贴图
    public void DrawRenderTexture()
    {
        var texture = topCameraComponent.GetRenderTexture();
        if (texture != null)
        {
            GUI.DrawTexture(ImageDrawRect, texture);
        }
    }
    public void RefreshCamera()
    {
        topCameraComponent.RenderCamera();
    }
    //更新重新计算全部区域大小，包括俯视图区域大小、顶部按钮区域大小
    private void UpdateAllRect()
    {

        ToolButtonRect = RightPanelRect;
        ToolButtonRect.height = 20;
        ImageDrawRect = RightPanelRect;
        ImageDrawRect.y = ToolButtonRect.height;
        ImageDrawRect.height = RightPanelRect.height - ToolButtonRect.height;
        //将图片区域比例缩放相等，取得最小者
        ImageDrawRect.size = Vector2.one * Mathf.Min(ImageDrawRect.width, ImageDrawRect.height);
        //更新框格区域的大小
        if (LastWindowSize != ImageDrawRect)
        {
            LastWindowSize = ImageDrawRect;
            //地形未加载时候，不给予更新
            if (window.SelectionTerrainComponent == null) return;
            TopViewTerrainCellDrawHelper.LoadDrawNodeDic(ImageDrawRect);
        }
    }
}
