using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

// 整型参数
[Serializable]
public class ListParameter : VNParameter
{
    /// <summary>
    /// 参数类型的模板
    /// </summary>
    protected VNParameter ParameterTemplate;
    /// <summary>
    /// 在编辑器中存储的参数的列表
    /// </summary>
    public List<VNParameter> listParameter = new List<VNParameter>();
    public override E_NodeParameterType ParameterType => E_NodeParameterType.List;
    public override string DropmenuShowName => "列表List";
    protected override string InspectorParameterName => "List<类型>";
    protected override float InspectorParameterHeight => 4.5f;

    //使用属性，为了控制在变的时候进行大规模换模板操作
    public E_NodeParameterType ListType
    {

        set
        {
            //如果值不变就不操作
            if (_listType == value) return;
            //进行大规模换模板操作
            _listType = value;
            //清除原来的列表
            listParameter.Clear();
            //经过反射筛查操作得到模板
            ParameterTemplate = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(VNParameter)) && !type.IsAbstract)
            .Select(type => (VNParameter)Activator.CreateInstance(type))
            .FirstOrDefault(instance => instance.ParameterType == value);
        }
        get => _listType;
    }
    //列表的真正类型
    private E_NodeParameterType _listType;

    [NonSerialized]
    private string[] options = null;
    private int optionIndex;

    [NonSerialized]
    private UnityAction deleteParameterCache;
    //暂时性的里层级绘制助手
    [NonSerialized]
    private DrawIndexHelper ParameterDrawhelper = new DrawIndexHelper();
    private int newCount = -1;
    public override void Draw_NodeEditor(DrawIndexHelper helper, float ScaleFactor)
    {
        if (ParameterDrawhelper == null) ParameterDrawhelper = new DrawIndexHelper();
        if (newCount == -1) newCount = listParameter.Count;

        //设置数量来新建
        helper.BeginHorizontalLayout(2, 1.5f);
        GUI.Label(helper.GetNextSingleRect(), "总数目：");
        newCount = EditorGUI.IntField(helper.GetNextSingleRect(), newCount);
        helper.EndHorizontalLayout();
        //检测原来数量和列表数量对比
        //大于就扩容
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
            //小于则减
            else if (newCount < listParameter.Count)
            {
                for (int i = listParameter.Count - 1; i >= 0; i--)
                {
                    listParameter.RemoveAt(i);
                }
            }
        }
        //总绘制参数列表层级
        for (int i = 0; i < listParameter.Count; i++)
        {
            helper.BeginHorizontalLayout(3, 1.5f);
            //先画前缀
            GUITextScaler.DrawScaledLabel(helper.GetNextSingleRect(), $"元素{i}：");
            //根据其类型来画
            listParameter[i].Draw_NodeEditorControl(helper);
            //删除元素方法
            if (GUI.Button(helper.GetNextSingleRect(), "删除"))
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
        //执行删除方法
        deleteParameterCache?.Invoke();
        deleteParameterCache = null;
    }
    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
    }



    //初始化设置列表选择框
    public void ResetSelectedOptions()
    {
        List<string> tempOptions = new List<string>();
        tempOptions.AddRange(Enum.GetNames(typeof(E_NodeParameterType)));
        //筛选过滤
        tempOptions.Remove("List");
        tempOptions.Remove("Dictionary");
        options = tempOptions.ToArray();

        //首次创建
        if (ParameterTemplate == null)
            ParameterTemplate = new IntParameter();
    }
    public override void DrawInspectorExtra()
    {
        //定位options
        if (options == null) ResetSelectedOptions();
        //绘制
        DrawHelper.BeginHorizontalLayout(2, 1.5F);
        GUI.Label(DrawHelper.GetNextSingleRect(), "列表参数类型：");
        optionIndex = EditorGUI.Popup(DrawHelper.GetNextSingleRect(), optionIndex, options);
        //赋值类型
        ListType = (E_NodeParameterType)Enum.Parse(typeof(E_NodeParameterType), options[optionIndex]);
        DrawHelper.EndHorizontalLayout();
        //绘制模板的额外狗方法
        if (ParameterTemplate != null)
        {
            ParameterTemplate.DrawHelper = DrawHelper;
            ParameterTemplate.DrawInspectorExtra();
        }
    }
    //要删除List元素的缓存方法  


    /// <summary>
    /// 序列化时候特殊处理，将列表的
    /// </summary>
    public void SerializeOperator()
    {

    }
}
