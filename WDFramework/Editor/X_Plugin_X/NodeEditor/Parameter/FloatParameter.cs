using System;
using UnityEditor;
using UnityEngine;

// ���Ͳ���
[Serializable]
public class FloatParameter : VNParameter
{
    public override E_NodeParameterType ParameterType => E_NodeParameterType.Float;

    public override string DropmenuShowName => "������Float";

    protected override string InspectorParameterName => "Float����";

 
    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
        Value = EditorGUI.FloatField(helper.GetNextSingleRect(), Convert.ToSingle(Value)).ToString();
    }
}
