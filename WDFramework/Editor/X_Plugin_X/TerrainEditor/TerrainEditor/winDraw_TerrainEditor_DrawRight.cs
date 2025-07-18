using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using WDEditor;



public partial class winDraw_TerrainEditor : BaseWindowDraw<WinData_TerrainEditor>
{
    //���ڻ��ƶ������鰴ť��Index����
    private DrawIndexHelper topToolIndexHelper = new DrawIndexHelper();
    //������������ť��������Rect
    private Rect ToolButtonRect;
    //����ͼ����Rect
    public Rect ImageDrawRect;
    //���ڼ�ⴰ�ڴ�С�Ƿ����
    private Rect LastWindowSize;
    private TopViewTerrainCellDrawHelper TopViewTerrainCellDrawHelper => window.TopViewDrawHelper;
    private TerrainTopViewCamera topCameraComponent => window.topViewCamera;
    //������ͼ
    public void DrawRightPanel()
    {
        //�ұ����򣬶��ڻ�û�м��سɹ�����£�ʲô�����ù�
        if(!isLoadedTerrain)return;
        //�ȹ滮�������
        UpdateAllRect();
        //���¶���������
        topToolIndexHelper.Update(ToolButtonRect);
        RightPanelDrawHelper.Update(LeftPanelRect);
        //���ƶ�����ť
        DrawTopButton();
        //������Ⱦ��ͼ
        DrawRenderTexture();
        //���л���ͼƬ���
        TopViewTerrainCellDrawHelper.Draw();

    }
    //���ƶ���������
    public void DrawTopButton()
    {

        topToolIndexHelper.BeginHorizontalLayout(6);
        GUI.Label(topToolIndexHelper.GetNextSingleRect(), "����߶�ֵ:");
        topCameraComponent.CameraHeight = EditorGUI.FloatField(topToolIndexHelper.GetNextSingleRect(), topCameraComponent.CameraHeight);
        GUI.Label(topToolIndexHelper.GetNextSingleRect(), "����ӿڴ�С:");
        topCameraComponent.CameraViewSize = EditorGUI.FloatField(topToolIndexHelper.GetNextSingleRect(), topCameraComponent.CameraViewSize);
        if (GUI.Button(topToolIndexHelper.GetNextSingleRect(), "ˢ�������"))
        {
            topCameraComponent.RenderCamera();
        }
        if (GUI.Button(topToolIndexHelper.GetNextSingleRect(), "ˢ���߿��"))
        {
            TopViewTerrainCellDrawHelper.LoadDrawNodeDic(ImageDrawRect);
        }
        topToolIndexHelper.EndHorizontalLayout();
    }
    //������ͼ
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
    //�������¼���ȫ�������С����������ͼ�����С��������ť�����С
    private void UpdateAllRect()
    {

        ToolButtonRect = RightPanelRect;
        ToolButtonRect.height = 20;
        ImageDrawRect = RightPanelRect;
        ImageDrawRect.y = ToolButtonRect.height;
        ImageDrawRect.height = RightPanelRect.height - ToolButtonRect.height;
        //��ͼƬ�������������ȣ�ȡ����С��
        ImageDrawRect.size = Vector2.one * Mathf.Min(ImageDrawRect.width, ImageDrawRect.height);
        //���¿������Ĵ�С
        if (LastWindowSize != ImageDrawRect)
        {
            LastWindowSize = ImageDrawRect;
            //����δ����ʱ�򣬲��������
            if (window.SelectionTerrainComponent == null) return;
            TopViewTerrainCellDrawHelper.LoadDrawNodeDic(ImageDrawRect);
        }
    }
}
