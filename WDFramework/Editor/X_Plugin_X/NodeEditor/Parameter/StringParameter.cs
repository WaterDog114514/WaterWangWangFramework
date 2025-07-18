// 字符串参数
using System;
using UnityEditor;
using UnityEngine;
[Serializable]
public class StringParameter : VNParameter
{
    public override E_NodeParameterType ParameterType => E_NodeParameterType.String;

    public override string DropmenuShowName => "字符串String";

    protected override string InspectorParameterName => "String参数";
    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
        Value = EditorGUI.TextField(helper.GetNextSingleRect(), Value);
    }
}
