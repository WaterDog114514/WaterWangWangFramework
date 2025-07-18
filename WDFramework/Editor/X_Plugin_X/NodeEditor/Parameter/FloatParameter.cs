using System;
using UnityEditor;
using UnityEngine;

// 整型参数
[Serializable]
public class FloatParameter : VNParameter
{
    public override E_NodeParameterType ParameterType => E_NodeParameterType.Float;

    public override string DropmenuShowName => "浮点型Float";

    protected override string InspectorParameterName => "Float参数";

 
    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
        Value = EditorGUI.FloatField(helper.GetNextSingleRect(), Convert.ToSingle(Value)).ToString();
    }
}
