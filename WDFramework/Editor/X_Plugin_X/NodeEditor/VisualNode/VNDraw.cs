using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 可视化节点绘制助手
/// 负责节点本身的绘制，以及可视化样式设置
/// </summary>
public class VNDraw
{
    //固定节点宽度
    private const float NodeWidth = 200;
    private const float TitleAreHeightFactor = 2f;
    private VisualNode visualNode = null;
    private float ScaleFactor;
    //绘制助手
    private DrawIndexHelper DrawHelper = new DrawIndexHelper();
    //绘制信息引用
    private VNDrawInfo drawInfo => visualNode.drawInfo;
    //绘制的位置
    public Vector2 DrawPosition => drawInfo.Position; //+ drawOffset;
    //绘制的大小
    private Vector2 DrawSize;
    //绘制的区域
    public Rect DrawRect;
    public VNDraw(VisualNode node)
    {
        visualNode = node;
        //计算绘制区域大小
        CalculateDrawRect();
    }

    private void CalculateDrawRect()
    {
        var ParameterHeight = visualNode.parameters.Count * 1.5f * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.singleLineHeight;
        var TitleHeight = TitleAreHeightFactor * EditorGUIUtility.singleLineHeight;
        //取得高度
        var Height = TitleHeight + ParameterHeight;
        var width = NodeWidth;
        DrawSize = new Vector2(width, Height);
    }
    /// <summary>
    /// 绘制节点总逻辑
    /// </summary>
    public void Draw(float scaleFactor)
    {
        //应用缩放比例
        ScaleFactor = scaleFactor;
        DrawRect = new Rect(DrawPosition * scaleFactor, DrawSize * ScaleFactor);
        DrawHelper.Update(DrawRect);
        //绘制背景
        DrawBackground();
        //绘制标题
        DrawTitle();
        //绘制所有参数的控件
        DrawParameters();

        //绘制进口和出口和他们的连接线
        DrawLinkLine(true);
        DrawLinkLine(false);
    }
    /// <summary>
    /// 绘制背景
    /// </summary>
    public void DrawBackground()
    {
        EditorGUI.DrawRect(DrawRect, drawInfo.BackgroundColor.color);
    }

    /// <summary>
    /// 这个节点是否被选中了？
    /// </summary>
    public bool b_IsSelected;
    /// <summary>
    /// 绘制标题方法
    /// </summary>
    public void DrawTitle()
    {
        //标题块大小
        Rect Rect = DrawHelper.GetNextSingleRect(TitleAreHeightFactor * ScaleFactor);
        //画标题区块的背景色
        EditorGUI.DrawRect(Rect, drawInfo.TitleAreaColor.color);
        //绘制标题
        GUITextScaler.DrawScaledLabel(Rect, visualNode.NodeName);
    }
    /// <summary>
    /// 绘制参数信息
    /// </summary>
    public void DrawParameters()
    {
        for (int i = 0; i < visualNode.parameters.Count; i++)
        {
            var parameter = visualNode.parameters[i];
            parameter.Draw_NodeEditor(DrawHelper, ScaleFactor);
        }
    }

    public void DrawLinkLine(bool isEnterPort)
    {
        //根据是画哪个口来选择
        var PortType = isEnterPort ? drawInfo.EnterPortPos : drawInfo.ExitPortPos;
        var NodeList = isEnterPort ? visualNode.EnterNodes : visualNode.ExitNodes;

        var PortSize = Vector2.one * 25*ScaleFactor;
        var position = DrawPosition*ScaleFactor;
        //绘制节点出入口的矩形
        var drawrect = new Rect();
        //计算进口位置
        switch (PortType)
        {
          
        
            case VNDrawInfo.E_PortPos.left:
                position += Vector2.up * DrawRect.height / 2;
                drawrect = new Rect(position - PortSize, PortSize);
                break;
            case VNDrawInfo.E_PortPos.top:
                position += Vector2.right * DrawRect.width / 2;
                drawrect = new Rect(position - PortSize, PortSize);
                break;
            case VNDrawInfo.E_PortPos.right:
                position += Vector2.right * DrawRect.width + Vector2.up * DrawRect.height / 2;
                drawrect = new Rect(position , PortSize);
                break;
            case VNDrawInfo.E_PortPos.bottom:
                position += Vector2.right * DrawRect.width / 2 + Vector2.up * DrawRect.height;
                drawrect = new Rect(position, PortSize);
                break;
        }
        Handles.DrawSolidRectangleWithOutline(drawrect, Color.blue, Color.cyan);

        //赋予中心值
        if (isEnterPort)
            drawInfo.EnterPortCenterPos = position;
        else
            drawInfo.ExitPortCenterPos = position;

        //只用画一根相连就够了，所以选择了自己出口到别的节点进口的线
        if(isEnterPort) return;

        //绘制连接箭头 由postion到
        // 绘制连接箭头
        for (int i = 0; i < NodeList.Count; i++)
        {
            var targetNode = NodeList[i];
            var targetPosition = targetNode.drawInfo.EnterPortCenterPos;

            if (targetPosition != Vector2.zero)
            {
                DrawArrow(position, targetPosition, Color.white);
            }
        }
    }

    private void DrawArrow(Vector2 from, Vector2 to, Color color)
    {
        Handles.color = color;
        Handles.DrawLine(from, to);

        // 绘制箭头
        Vector2 direction = (to - from).normalized;
        float arrowHeadAngle = 20.0f;
        float arrowHeadLength = 10.0f;

        Vector2 right = Quaternion.Euler(0, 0, arrowHeadAngle) * -direction * arrowHeadLength;
        Vector2 left = Quaternion.Euler(0, 0, -arrowHeadAngle) * -direction * arrowHeadLength;

        Handles.DrawLine(to, to + right);
        Handles.DrawLine(to, to + left);
    }
}
