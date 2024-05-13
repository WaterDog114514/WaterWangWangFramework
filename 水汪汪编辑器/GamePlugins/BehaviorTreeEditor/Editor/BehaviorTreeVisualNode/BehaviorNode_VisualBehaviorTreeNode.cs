using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 表现动作节点
/// </summary>
[System.Serializable]
public class BehaviorNode_VisualBehaviorTreeNode : VisualBehaviorTreeNode
{
    /// <summary>
    /// 监听数量
    /// </summary>
    public int ListenNum
    {
        set
        {
            //和上一次一样，就走了把
            if (_Num == value) return;
            //搞得想List一样的操作
            string[] temp = new string[value];
            //袁本初有了才进行删减赋值哦
            if (Parameter != null)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    //参数不能丢哦
                    if (i >= Parameter.Length) break;
                    temp[i] = Parameter[i];
                }
            }
            Parameter = temp;
            Size = new Vector2(140, 90 + value * 50);
            //设置存取索引
            selectedIndex.Clear();
            for (int i = 0; i < value; i++)
            {
                selectedIndex.Add(new SelectedIndex());
            }
            _Num = value;
        }
        get => _Num;
    }
    private int _Num;

    public List<SelectedIndex> selectedIndex;
    public GameObject rootAiObj;
    public BehaviorNode_VisualBehaviorTreeNode(string Description, E_BehaviorType type) : base(Description, type)
    {
        switch (NodeType)
        {
            case E_BehaviorType.ActionTreeNode:
                break;
            case E_BehaviorType.ConditionNode:
                //  Parameter = new string[];
                break;

        }
        Parameter = new string[0];
        rootAiObj = RootNode_VisualBehaviorTreeNode.instance.BehaviorObj;
        selectedIndex = new List<SelectedIndex>();
    }
    public override void DrawControlData()
    {
        base.DrawControlData();
        //开头
        GUI.Label(m_getLabelRect(2.0f / 3), "方法数:", FontStyle);
        ListenNum = EditorGUI.IntField(m_getControlDrawRect(1.0F / 3), ListenNum);
        //监听更改Obj并刷新
        if (rootAiObj != RootNode_VisualBehaviorTreeNode.instance.BehaviorObj)
        {
            rootAiObj = RootNode_VisualBehaviorTreeNode.instance.BehaviorObj;
            //清空原参数的所有东西
            if (Parameter != null)
                Parameter = new string[Parameter.Length];
            //更换了，要把所有选择给清空为0
            for (int i = 0; i < selectedIndex.Count; i++)
            {
                selectedIndex[i].Select1 = 0;
                selectedIndex[i].Select2 = 0;
            }
        }

        //行为监听
        for (int i = 0; i < _Num; i++)
        {
            EditorGUI.LabelField(m_getLabelRect(1.0F / 2), $"脚本：", FontStyle);
            EditorGUI.LabelField(m_getControlDrawRect(1.0F / 2), $"方法{i + 1}：", FontStyle);
            if (RootMethodReflection.Instance.list_ComponentName == null)
                EditorGUI.LabelField(m_getControlDrawRect(true), $"请在根节点处设置AI预制体", FontStyle);
            else if (RootMethodReflection.Instance.list_ComponentName.Length == 0)
                EditorGUI.LabelField(m_getControlDrawRect(true), $"该预制体无可用方法", FontStyle);
            else
            {
                selectedIndex[i].Select1 = (EditorGUI.Popup(m_getLabelRect(1.0f / 2), selectedIndex[i].Select1, RootMethodReflection.Instance.list_ComponentName));
                //根据是条件节点，还是动作节点，设置获取到的方法们
                switch (NodeType)
                {
                    //动作节点
                    case E_BehaviorType.ActionTreeNode:
                        selectedIndex[i].Select2 = (EditorGUI.Popup(m_getControlDrawRect(1.0f / 2),
                            selectedIndex[i].Select2,
                           RootMethodReflection.Instance.dic_void_CompoentMethods[RootMethodReflection.Instance.list_ComponentName[selectedIndex[i].Select1]]));
                        //设置参数 ：组件脚本名+方法名
                        Parameter[i] =
                            RootMethodReflection.Instance.list_ComponentName[selectedIndex[i].Select1] + $"&{selectedIndex[i].Select1}" + "|" +
                            RootMethodReflection.Instance.dic_void_CompoentMethods[RootMethodReflection.Instance.list_ComponentName[selectedIndex[i].Select1]][selectedIndex[i].Select2] + $"&{selectedIndex[i].Select2}";
                        break;
                    //条件节点
                    case E_BehaviorType.ConditionNode:
                        selectedIndex[i].Select2 = (EditorGUI.Popup(m_getControlDrawRect(1.0f / 2),
                      selectedIndex[i].Select2,
                  RootMethodReflection.Instance.dic_bool_CompoentMethods[RootMethodReflection.Instance.list_ComponentName[selectedIndex[i].Select1]]));
                        //设置参数
                        Parameter[i] =
                            RootMethodReflection.Instance.list_ComponentName[selectedIndex[i].Select1] + $"&{selectedIndex[i].Select1}" + "|" +
                            RootMethodReflection.Instance.dic_bool_CompoentMethods[RootMethodReflection.Instance.list_ComponentName[selectedIndex[i].Select1]][selectedIndex[i].Select2] + $"&{selectedIndex[i].Select2}";
                        break;
                }

            }
            //   EditorGUI.Popup(m_getControlDrawRect(true),0,);
        }
    }
    public override void m_IntiSize()
    {
        Size = new Vector2(120, 90);
    }
    public override void m_DrawBackground()
    {
        //橙褐色
        switch (NodeType)
        {
            case E_BehaviorType.ActionTreeNode:
                EditorGUI.DrawRect(new Rect(Pos_Draw, Size), new Color(0.86F, 0.63F, 0.111F, 0.7f));
                break;
            case E_BehaviorType.ConditionNode:
                EditorGUI.DrawRect(new Rect(Pos_Draw, Size), new Color(0.41F, 0.556F, 0.137F, 0.7f));
                break;

        }
    }
}