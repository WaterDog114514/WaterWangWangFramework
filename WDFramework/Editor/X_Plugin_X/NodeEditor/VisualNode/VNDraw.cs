using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
/// <summary>
/// ���ӻ��ڵ��������
/// ����ڵ㱾��Ļ��ƣ��Լ����ӻ���ʽ����
/// </summary>
public class VNDraw
{
    //�̶��ڵ���
    private const float NodeWidth = 200;
    private const float TitleAreHeightFactor = 2f;
    private VisualNode visualNode = null;
    private float ScaleFactor;
    //��������
    private DrawIndexHelper DrawHelper = new DrawIndexHelper();
    //������Ϣ����
    private VNDrawInfo drawInfo => visualNode.drawInfo;
    //���Ƶ�λ��
    public Vector2 DrawPosition => drawInfo.Position; //+ drawOffset;
    //���ƵĴ�С
    private Vector2 DrawSize;
    //���Ƶ�����
    public Rect DrawRect;
    public VNDraw(VisualNode node)
    {
        visualNode = node;
        //������������С
        CalculateDrawRect();
    }

    private void CalculateDrawRect()
    {
        var ParameterHeight = visualNode.parameters.Count * 1.5f * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.singleLineHeight;
        var TitleHeight = TitleAreHeightFactor * EditorGUIUtility.singleLineHeight;
        //ȡ�ø߶�
        var Height = TitleHeight + ParameterHeight;
        var width = NodeWidth;
        DrawSize = new Vector2(width, Height);
    }
    /// <summary>
    /// ���ƽڵ����߼�
    /// </summary>
    public void Draw(float scaleFactor)
    {
        //Ӧ�����ű���
        ScaleFactor = scaleFactor;
        DrawRect = new Rect(DrawPosition * scaleFactor, DrawSize * ScaleFactor);
        DrawHelper.Update(DrawRect);
        //���Ʊ���
        DrawBackground();
        //���Ʊ���
        DrawTitle();
        //�������в����Ŀؼ�
        DrawParameters();

        //���ƽ��ںͳ��ں����ǵ�������
        DrawLinkLine(true);
        DrawLinkLine(false);
    }
    /// <summary>
    /// ���Ʊ���
    /// </summary>
    public void DrawBackground()
    {
        EditorGUI.DrawRect(DrawRect, drawInfo.BackgroundColor.color);
    }

    /// <summary>
    /// ����ڵ��Ƿ�ѡ���ˣ�
    /// </summary>
    public bool b_IsSelected;
    /// <summary>
    /// ���Ʊ��ⷽ��
    /// </summary>
    public void DrawTitle()
    {
        //������С
        Rect Rect = DrawHelper.GetNextSingleRect(TitleAreHeightFactor * ScaleFactor);
        //����������ı���ɫ
        EditorGUI.DrawRect(Rect, drawInfo.TitleAreaColor.color);
        //���Ʊ���
        GUITextScaler.DrawScaledLabel(Rect, visualNode.NodeName);
    }
    /// <summary>
    /// ���Ʋ�����Ϣ
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
        //�����ǻ��ĸ�����ѡ��
        var PortType = isEnterPort ? drawInfo.EnterPortPos : drawInfo.ExitPortPos;
        var NodeList = isEnterPort ? visualNode.EnterNodes : visualNode.ExitNodes;

        var PortSize = Vector2.one * 25*ScaleFactor;
        var position = DrawPosition*ScaleFactor;
        //���ƽڵ����ڵľ���
        var drawrect = new Rect();
        //�������λ��
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

        //��������ֵ
        if (isEnterPort)
            drawInfo.EnterPortCenterPos = position;
        else
            drawInfo.ExitPortCenterPos = position;

        //ֻ�û�һ�������͹��ˣ�����ѡ�����Լ����ڵ���Ľڵ���ڵ���
        if(isEnterPort) return;

        //�������Ӽ�ͷ ��postion��
        // �������Ӽ�ͷ
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

        // ���Ƽ�ͷ
        Vector2 direction = (to - from).normalized;
        float arrowHeadAngle = 20.0f;
        float arrowHeadLength = 10.0f;

        Vector2 right = Quaternion.Euler(0, 0, arrowHeadAngle) * -direction * arrowHeadLength;
        Vector2 left = Quaternion.Euler(0, 0, -arrowHeadAngle) * -direction * arrowHeadLength;

        Handles.DrawLine(to, to + right);
        Handles.DrawLine(to, to + left);
    }
}
