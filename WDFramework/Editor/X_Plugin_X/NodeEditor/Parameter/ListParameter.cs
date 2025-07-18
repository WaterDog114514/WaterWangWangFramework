using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

// ���Ͳ���
[Serializable]
public class ListParameter : VNParameter
{
    /// <summary>
    /// �������͵�ģ��
    /// </summary>
    protected VNParameter ParameterTemplate;
    /// <summary>
    /// �ڱ༭���д洢�Ĳ������б�
    /// </summary>
    public List<VNParameter> listParameter = new List<VNParameter>();
    public override E_NodeParameterType ParameterType => E_NodeParameterType.List;
    public override string DropmenuShowName => "�б�List";
    protected override string InspectorParameterName => "List<����>";
    protected override float InspectorParameterHeight => 4.5f;

    //ʹ�����ԣ�Ϊ�˿����ڱ��ʱ����д��ģ��ģ�����
    public E_NodeParameterType ListType
    {

        set
        {
            //���ֵ����Ͳ�����
            if (_listType == value) return;
            //���д��ģ��ģ�����
            _listType = value;
            //���ԭ�����б�
            listParameter.Clear();
            //��������ɸ������õ�ģ��
            ParameterTemplate = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(VNParameter)) && !type.IsAbstract)
            .Select(type => (VNParameter)Activator.CreateInstance(type))
            .FirstOrDefault(instance => instance.ParameterType == value);
        }
        get => _listType;
    }
    //�б����������
    private E_NodeParameterType _listType;

    [NonSerialized]
    private string[] options = null;
    private int optionIndex;

    [NonSerialized]
    private UnityAction deleteParameterCache;
    //��ʱ�Ե���㼶��������
    [NonSerialized]
    private DrawIndexHelper ParameterDrawhelper = new DrawIndexHelper();
    private int newCount = -1;
    public override void Draw_NodeEditor(DrawIndexHelper helper, float ScaleFactor)
    {
        if (ParameterDrawhelper == null) ParameterDrawhelper = new DrawIndexHelper();
        if (newCount == -1) newCount = listParameter.Count;

        //�����������½�
        helper.BeginHorizontalLayout(2, 1.5f);
        GUI.Label(helper.GetNextSingleRect(), "����Ŀ��");
        newCount = EditorGUI.IntField(helper.GetNextSingleRect(), newCount);
        helper.EndHorizontalLayout();
        //���ԭ���������б������Ա�
        //���ھ�����
        if (newCount != listParameter.Count)
        {
            if (newCount > listParameter.Count)
            {
                for (int i = 0; i < newCount - listParameter.Count; i++)
                {
                    var newParameter = (VNParameter)BinaryManager.DeepClone(ParameterTemplate.GetType(), ParameterTemplate);
                    listParameter.Add(newParameter);
                }
            }
            //С�����
            else if (newCount < listParameter.Count)
            {
                for (int i = listParameter.Count - 1; i >= 0; i--)
                {
                    listParameter.RemoveAt(i);
                }
            }
        }
        //�ܻ��Ʋ����б�㼶
        for (int i = 0; i < listParameter.Count; i++)
        {
            helper.BeginHorizontalLayout(3, 1.5f);
            //�Ȼ�ǰ׺
            GUITextScaler.DrawScaledLabel(helper.GetNextSingleRect(), $"Ԫ��{i}��");
            //��������������
            listParameter[i].Draw_NodeEditorControl(helper);
            //ɾ��Ԫ�ط���
            if (GUI.Button(helper.GetNextSingleRect(), "ɾ��"))
            {
                var delete = listParameter[i];
                deleteParameterCache += () =>
                {
                    if (listParameter.Contains(delete))
                    {
                        newCount--;
                        listParameter.Remove(delete);
                    }
                };
            }
            helper.EndHorizontalLayout();

        }
        //ִ��ɾ������
        deleteParameterCache?.Invoke();
        deleteParameterCache = null;
    }
    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
    }



    //��ʼ�������б�ѡ���
    public void ResetSelectedOptions()
    {
        List<string> tempOptions = new List<string>();
        tempOptions.AddRange(Enum.GetNames(typeof(E_NodeParameterType)));
        //ɸѡ����
        tempOptions.Remove("List");
        tempOptions.Remove("Dictionary");
        options = tempOptions.ToArray();

        //�״δ���
        if (ParameterTemplate == null)
            ParameterTemplate = new IntParameter();
    }
    public override void DrawInspectorExtra()
    {
        //��λoptions
        if (options == null) ResetSelectedOptions();
        //����
        DrawHelper.BeginHorizontalLayout(2, 1.5F);
        GUI.Label(DrawHelper.GetNextSingleRect(), "�б�������ͣ�");
        optionIndex = EditorGUI.Popup(DrawHelper.GetNextSingleRect(), optionIndex, options);
        //��ֵ����
        ListType = (E_NodeParameterType)Enum.Parse(typeof(E_NodeParameterType), options[optionIndex]);
        DrawHelper.EndHorizontalLayout();
        //����ģ��Ķ��⹷����
        if (ParameterTemplate != null)
        {
            ParameterTemplate.DrawHelper = DrawHelper;
            ParameterTemplate.DrawInspectorExtra();
        }
    }
    //Ҫɾ��ListԪ�صĻ��淽��  


    /// <summary>
    /// ���л�ʱ�����⴦�����б��
    /// </summary>
    public void SerializeOperator()
    {

    }
}
