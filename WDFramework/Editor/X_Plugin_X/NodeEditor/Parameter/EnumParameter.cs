using System;
using UnityEditor;
using UnityEngine;


// 枚举参数
[Serializable]
public class EnumParameter : VNParameter
{
    //枚举类型
    public string EnumType;
    public int selectedIndex;

    public override E_NodeParameterType ParameterType => E_NodeParameterType.Enum;
    protected override float InspectorParameterHeight => 2.5f;
    protected override string InspectorParameterName => "枚举参数";

    public override string DropmenuShowName => "枚举Enum";

    //绘制专用
    [NonSerialized]
    private bool isSearched = false;
    [NonSerialized]
    private string[] options = null;

    public override void DrawInspectorExtra()
    {
        // 枚举反射搜索器输入框
        DrawHelper.BeginHorizontalLayout(3);
        GUI.Label(DrawHelper.GetNextSingleRect(), "枚举类型名：");
        EnumType = EditorGUI.TextField(DrawHelper.GetNextSingleRect(), EnumType);
        if (GUI.Button(DrawHelper.GetNextSingleRect(),"检验枚举"))
        {
            Type type = ReflectionHelper.FindTypeInAssemblies(EnumType);
            if (type != null)
            {
                EditorUtility.DisplayDialog("查找成功", $"已经搜索到{type.Assembly.FullName}下的{type.Name}枚举类型", "真好");
            }
            else
            {
                EditorUtility.DisplayDialog("查找不到", $"搜索不到{EnumType}枚举类型，请重新检查", "真的不好");
            }
        }
        DrawHelper.EndHorizontalLayout();
    }

    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
        //第一次，给options赋值
        if (isSearched == false && options == null)
        {
            isSearched = true;
            Type type = ReflectionHelper.FindTypeInAssemblies(EnumType);
            if (type != null)
            {
                options = Enum.GetNames(type);
            }
        }


        if (options == null || options.Length == 0)
        {
            GUITextScaler.DrawScaledLabel(helper.GetNextSingleRect(), "枚举不可用");
            return;
        }
        selectedIndex = EditorGUI.Popup(helper.GetNextSingleRect(), selectedIndex, options);
    }
}
