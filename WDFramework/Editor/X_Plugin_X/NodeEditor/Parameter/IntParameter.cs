using System;
using UnityEditor;
using UnityEngine;

// 整型参数
[Serializable]
public class IntParameter : VNParameter
{
    public override E_NodeParameterType ParameterType => E_NodeParameterType.Int;

    public override string DropmenuShowName => "整数型Int";

    protected override string InspectorParameterName => "Int参数：";
    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
        Value = EditorGUI.IntField(helper.GetNextSingleRect(), Convert.ToInt32(Value)).ToString();
    }
}
