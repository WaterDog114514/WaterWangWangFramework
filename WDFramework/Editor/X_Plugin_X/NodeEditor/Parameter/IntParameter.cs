using System;
using UnityEditor;
using UnityEngine;

// ���Ͳ���
[Serializable]
public class IntParameter : VNParameter
{
    public override E_NodeParameterType ParameterType => E_NodeParameterType.Int;

    public override string DropmenuShowName => "������Int";

    protected override string InspectorParameterName => "Int������";
    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
        Value = EditorGUI.IntField(helper.GetNextSingleRect(), Convert.ToInt32(Value)).ToString();
    }
}
