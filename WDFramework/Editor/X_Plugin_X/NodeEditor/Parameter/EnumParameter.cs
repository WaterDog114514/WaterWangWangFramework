using System;
using UnityEditor;
using UnityEngine;


// ö�ٲ���
[Serializable]
public class EnumParameter : VNParameter
{
    //ö������
    public string EnumType;
    public int selectedIndex;

    public override E_NodeParameterType ParameterType => E_NodeParameterType.Enum;
    protected override float InspectorParameterHeight => 2.5f;
    protected override string InspectorParameterName => "ö�ٲ���";

    public override string DropmenuShowName => "ö��Enum";

    //����ר��
    [NonSerialized]
    private bool isSearched = false;
    [NonSerialized]
    private string[] options = null;

    public override void DrawInspectorExtra()
    {
        // ö�ٷ��������������
        DrawHelper.BeginHorizontalLayout(3);
        GUI.Label(DrawHelper.GetNextSingleRect(), "ö����������");
        EnumType = EditorGUI.TextField(DrawHelper.GetNextSingleRect(), EnumType);
        if (GUI.Button(DrawHelper.GetNextSingleRect(),"����ö��"))
        {
            Type type = ReflectionHelper.FindTypeInAssemblies(EnumType);
            if (type != null)
            {
                EditorUtility.DisplayDialog("���ҳɹ�", $"�Ѿ�������{type.Assembly.FullName}�µ�{type.Name}ö������", "���");
            }
            else
            {
                EditorUtility.DisplayDialog("���Ҳ���", $"��������{EnumType}ö�����ͣ������¼��", "��Ĳ���");
            }
        }
        DrawHelper.EndHorizontalLayout();
    }

    public override void Draw_NodeEditorControl(DrawIndexHelper helper)
    {
        //��һ�Σ���options��ֵ
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
            GUITextScaler.DrawScaledLabel(helper.GetNextSingleRect(), "ö�ٲ�����");
            return;
        }
        selectedIndex = EditorGUI.Popup(helper.GetNextSingleRect(), selectedIndex, options);
    }
}
