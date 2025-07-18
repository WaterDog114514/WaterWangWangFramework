using System;
using UnityEditor;
using UnityEngine;
using WDEditor;

public class VNEditorCheck
{
    //����״̬�����ݵ�ǰ״̬��ʹ��ĳЩ�߼�
    private enum E_OperatorState
    {
        //����ɶ������
        Idle,
        //���ڱ༭�ڵ����
        EditParameter,
        //�������ӽڵ�
        LinkingNode,
        //����ȡ�����ӽڵ�
        UnLinkingNode,
    }
    private E_OperatorState OperatorState = E_OperatorState.Idle;
    private win_NodeEditor window;
    private winData_NodeEditor data => window.data;
    private VNEditorDropmenu dropmenu => window.Dropmenu;
    public VNEditorCheck(win_NodeEditor win)
    {
        this.window = win;
    }
    public VisualNode selectedNode;
    //����ִ�����ӵĿ�ʼ�ڵ� 
    private VisualNode LinkingBeginNode;
    private bool IsEnterEditNodeState;
    /// <summary>
    /// �����ʵλ��
    /// </summary>
    private Vector2 MouseActualPosition => Event.current.mousePosition;
    public void CheckUpdate_Window()
    {
        // Debug.Log("���λ�ã�" + MouseActualPosition);
        //����Ƿ����ӿ��У����ھͲ��ܽ��м������ڵ�����ļ����
        IsEnterViewCheck = true;
        if (!CheckMouseInRightPanel)
        {
            IsEnterViewCheck = false;
            return;
        }
    }
    //�Ƿ�ִ�нڵ���ͼ�ļ���߼�
    private bool IsEnterViewCheck = false;
    // Ӧ�ýڵ�����ϵ�ļ��
    public void CheckUpdate_NodeView()
    {
        if (!IsEnterViewCheck) return;
        // Debug.Log("���λ�ã�" + MouseActualPosition);
        //����Ƿ����ӿ��У����ھͲ��ܽ��м��
        Check_MouseScroll();
        Check_SingleRightClick();
        Check_DragSingleNode();
        Check_SingleLeftClick();
    }
    /// <summary>
    /// �Ƿ��ڱ༭ģʽ�£��ڵ�༭ģʽ�Ƿ�ֹ�Ҽ��ڵ� �㲻��
    /// </summary>
    public bool IsInEditing;
    /// <summary>
    /// ������ק�ӿڣ�
    /// </summary>
    public bool IsDragingView;
    /// <summary>
    /// �������Ƿ�λ���ӿ���
    /// </summary>
    private bool CheckMouseInRightPanel
    {
        get => data.RightPanelRect.rect.Contains(MouseActualPosition);

    }
    /// <summary>
    /// �����˵���⣬�Ҽ��������˵�
    /// </summary>
    public void Check_SingleRightClick()
    {
        if (Event.current.button == 1 && Event.current.delta == Vector2.zero)
        {
            var ClickNode = VNColliderCheck();
            //�ȼ���Ƿ�������״̬���еĻ���ȡ��״̬
            if(OperatorState != E_OperatorState.Idle)
            {
                ExitLinkNodeState();
                EnterIdleState();
            }

            //���в˵��߼�����
            if (ClickNode == null)
                //��ʾ��ͼ�����˵�
                window.Dropmenu.ShowViewDropmenu(MouseActualPosition);
            else
                //��ʾ�ڵ�����༭�˵�
                window.Dropmenu.ShowOperatorNodeMenu(MouseActualPosition, ClickNode);
        }
    }
    private double lastClickTime;
    // ��굥�������ײ���
    public void Check_SingleLeftClick()
    {
        //ѡȡ���
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            var ClickNode = VNColliderCheck();
            //���ݲ�ͬ״ִ̬���߼�
            //�������¾�ѡ��
            if (OperatorState == E_OperatorState.Idle)
            {
                //��������������
                if (ClickNode == null) 
                    selectedNode = null;
                else
                    selectedNode = ClickNode;
            }
            //�������ӵĻ����������߼�
            else if (OperatorState == E_OperatorState.LinkingNode && LinkingBeginNode != null)
            {
                //���ӵ���λ�þͲ�������
                if(ClickNode== null)
                    return;

                //������ɺ��˳�״̬
                window.Operator.LinkNode(LinkingBeginNode, ClickNode);
                ExitLinkNodeState();
            }
        }

    }
    //���˫��������
    public void Check_MouseTwoLeftClick()
    {
        //˫�����
        if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
        {
            //����˫����������
            if (EditorApplication.timeSinceStartup - lastClickTime < 0.3)
            {
                IsEnterEditNodeState = true;
            }
            lastClickTime = EditorApplication.timeSinceStartup;
        }
    }
    //�����ּ�⡪����ק��������ͼ
    public void Check_MouseScroll()
    {
        //���������ֵ������ƶ�
        if (Event.current.type == EventType.ScrollWheel)
        {
            //������ʱ�����������������������
            Event.current.Use();

            // �����߼�
            var zoomPivot = Event.current.mousePosition;
            float oldScale = data.ScaleValue;
            data.ScaleValue -= Event.current.delta.y * 0.05f;
            data.ScaleValue = Mathf.Clamp(data.ScaleValue, 0.5f, 1.5f);
            // �����������ĵ�ƫ��
            Vector2 delta = (data.ViewportPosition.vector2 + zoomPivot) * (data.ScaleValue / oldScale) - (data.ViewportPosition.vector2 + zoomPivot);
            data.ViewportPosition.vector2 += delta;
        }

        //��ק��ͼ���
        if (!Event.current.control && Event.current.button == 2 && Event.current.type == EventType.MouseDrag && Event.current.delta != Vector2.zero)
        {
            data.ViewportPosition.vector2 += Event.current.delta;
            IsDragingView = true;
        }
        else IsDragingView = false;
    }
    /// <summary>
    /// ��ק�����ڵ�
    /// </summary>
    public void Check_DragSingleNode()
    {
        //�����������ʱ�򣬲����ƶ�
        //  if (GUIUtility.keyboardControl != 0) return;
        //��ק�������
        //1.��ѡ�����岻Ϊ��ʱ
        //2.��ס������
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 && selectedNode != null && !IsEnterEditNodeState)
        {
            selectedNode.drawInfo.Position += Event.current.delta / data.ScaleValue;
        }

    }
    //�������Ƿ���ĳ�ڵ�������������ڽڵ�
    public VisualNode VNColliderCheck()
    {
        // Debug.Log("���λ�ã�"+MouseActualPosition);
        foreach (var visualNode in data.dic_Nodes.Values)
        {
            //   Debug.Log("�ڵ�����λ�ã�"+visualNode.vnDraw.DrawRect);
            if (visualNode.vnDraw.DrawRect.Contains(MouseActualPosition))
            {
                return visualNode;
            }
        }
        return null;
    }


    //�������ӽڵ�״̬
    public void EnterLinkNodeState(VisualNode BeginNode)
    {
        LinkingBeginNode = BeginNode;
        OperatorState = E_OperatorState.LinkingNode;
    }
    //�˳����ӽڵ�״̬
    public void ExitLinkNodeState()
    {
        LinkingBeginNode = null;
        EnterIdleState();
    }
    public void EnterIdleState()
    {
        OperatorState = E_OperatorState.Idle;
    }

}
