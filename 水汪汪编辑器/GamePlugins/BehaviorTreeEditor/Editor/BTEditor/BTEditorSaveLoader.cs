using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

//为避免类太过于臃肿，分一个模块来实现BT节点编辑器的加载存储功能
public class BTEditorSaveLoader
{
    private BehaviorTreeNodeEditorWindow win;
    public BTEditorSaveLoader(BehaviorTreeNodeEditorWindow win)
    {
        this.win = win;
    }
    /// <summary>
    /// 保存所有数据
    /// </summary>
    public void m_SaveAllData()
    {
        //搞阴谋论
        BTNodeJsonData jsonData = new BTNodeJsonData();
        jsonData.dic_Info = new Dictionary<string, BTNodeInfo>();
        //遍历保存到新数据
        foreach (VisualBaseNode node in win.dic_Nodes.Values)
        {
            VisualBehaviorTreeNode Node = node as VisualBehaviorTreeNode;
            BTNodeInfo info = new BTNodeInfo();
            //基本赋值
            switch (Node.NodeType)
            {
                case E_BehaviorType.RootNode:
                    info.childsID.Add((Node as RootNode_VisualBehaviorTreeNode).ChildID);
                    break;
                case E_BehaviorType.SelectTreeNode:
                case E_BehaviorType.SequeneTreeNode:
                case E_BehaviorType.ParallelTreeNode:
                    info.childsID = (Node as ControlNode_VisualBehaviorTreeNode).ChildsId;
                    break;
                case E_BehaviorType.ActionTreeNode:
                case E_BehaviorType.ConditionNode:
                    break;
                case E_BehaviorType.DelayDecoratorNode:
                case E_BehaviorType.ReverseDecoratorNode:
                case E_BehaviorType.RepeatDecoratorNode:
                    info.childsID.Add((Node as DecoratorNode_VisualBehaviorTreeNode).ChildID);
                    break;
            }
            //普通赋值
            info.Description = Node.Description;
            info.ID = Node.ID;
            info.NodeType = Node.NodeType;
            info.Parameters = Node.Parameter;
            //保存
            jsonData.dic_Info.Add(info.ID.ToString(), info);
        }
        // 保存数据
        string path = EditorUtility.SaveFilePanel("保存行为树数据", Application.dataPath, null, null);
        string pathAsset = "Assets" + path.Replace(Application.dataPath, null) + ".asset";
        JsonManager.Instance.SaveDataToPath(jsonData, path + ".json");
        //保存小配置文件
        BTNodeData btNodeData = ScriptableObject.CreateInstance<BTNodeData>();
        btNodeData.BehaviorTreePrefab = win.RootNode.BehaviorObj;
        btNodeData.TreeNodeDataPath = "Assets/" + path.Replace(Application.dataPath, null) + ".json";
        AssetDatabase.CreateAsset(btNodeData, pathAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //稍微设置一下绑定的数据文件
        BTNodeData importData = AssetDatabase.LoadAssetAtPath<BTNodeData>(pathAsset);
        importData.BehaviorTreeData = AssetDatabase.LoadAssetAtPath<TextAsset>(btNodeData.TreeNodeDataPath);

        //给预制体加组件
        GameObject prefab = RootNode_VisualBehaviorTreeNode.instance.BehaviorObj;
        if (prefab != null)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets")) + "/Library/ScriptAssemblies/Assembly-CSharp.dll");
            BTNodeObjcetDriver bTNodeObjcetDriver = prefab.GetComponent(assembly.GetType("BTNodeObjcetDriver")) as BTNodeObjcetDriver;
            if (bTNodeObjcetDriver == null) bTNodeObjcetDriver = prefab.AddComponent(assembly.GetType("BTNodeObjcetDriver")) as BTNodeObjcetDriver;
            bTNodeObjcetDriver.data = importData;

        }

    }
    /// <summary>
    /// 动态加载数据，直接在运行中加载行为树
    /// </summary>
    /// <param name="dic"></param>
    public void m_LoadDynamicData(Dictionary<string, BTNodeInfo> dic)
    {
        if (win.dic_Nodes.Count > 1) win.m_ClearAllNodes();
        Read_Dic = dic;
        tempData = win.CheckingDirver.data;
        LoadAllNode();
        win.b_IsCheckingMode = true;
        foreach (var node in win.dic_Nodes.Values)
        {
            (node as VisualBehaviorTreeNode).b_IsCheckingMode = true;

        }
        tempData = null;
        //装卸工
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    /// <summary>
    /// 暂停时候需要关闭 行为树编辑器的监听
    /// </summary>
    /// <param name="state"></param>
    private  void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            win.m_ClearAllNodes();
            EditorWindow.GetWindow<Win_BehaviorTree>().Close();
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
    }
    private BTNodeData tempData;
    /// <summary>
    /// 静态加载数据
    /// </summary>
    public void m_LoadStaticData(BTNodeData data)
    {
        //避免重复加载有BUG
        if (win.dic_Nodes.Count > 1) win.m_ClearAllNodes();
        Read_Dic = JsonMapper.ToObject<BTNodeJsonData>(data.BehaviorTreeData.text).dic_Info;
        tempData = data;
        LoadAllNode();
        //释放
        tempData = null;
    }

    private Dictionary<string, BTNodeInfo> Read_Dic;
    /// <summary>
    /// 绑定爸爸拥有的孩子，静态加载才用！
    /// </summary>


    /// <summary>
    /// 节点找爸爸
    /// </summary>
    public void m_NodeFindSelfFather(VisualBehaviorTreeNode node, BTNodeInfo info)
    {
        //给节点找爸爸，因为有了爸爸，儿子才能连线
        foreach (var FatherInfo in Read_Dic.Values)
        {
            if (FatherInfo.ID == node.ID) continue;
            switch (FatherInfo.NodeType)
            {
                case E_BehaviorType.RootNode:
                    if (FatherInfo.childsID.Count > 0)
                        if (FatherInfo.childsID[0] == node.ID)
                            node.FatherID = FatherInfo.ID;
                    break;
                case E_BehaviorType.SelectTreeNode:
                case E_BehaviorType.SequeneTreeNode:
                case E_BehaviorType.ParallelTreeNode:

                    if (FatherInfo.childsID.Count != 0)
                    {
                        for (int i = 0; i < FatherInfo.childsID.Count; i++)
                        {
                            if (FatherInfo.childsID[i] == node.ID)
                                node.FatherID = FatherInfo.ID;
                        }
                    }
                    break;

                case E_BehaviorType.ActionTreeNode:
                case E_BehaviorType.ConditionNode:
                    //这两货不能当爸
                    break;
                case E_BehaviorType.DelayDecoratorNode:
                case E_BehaviorType.ReverseDecoratorNode:
                case E_BehaviorType.RepeatDecoratorNode:
                    if (FatherInfo.childsID.Count > 0 && info != FatherInfo)
                        if (FatherInfo.childsID[0] == node.ID)
                            node.FatherID = FatherInfo.ID;
                    break;
            }

        }
    }
    /// <summary>
    /// 加载节点
    /// </summary>
    public void LoadAllNode()
    {
        win.dic_Nodes.Clear();
        VisualBehaviorTreeNode node;
        foreach (var info in Read_Dic.Values)
        {
            node = win.CreateBehaviorNode(info.NodeType, false);
            node.Description = info.Description;
            node.BehaviorNode = info.Node;
            node.Parameter = info.Parameters;
            node.ID = info.ID;
            win.m_AddNode(node, node.ID);
            //设置爸爸就行，动态读取的根本就不需要找儿子
            //设置参数
            switch (node.NodeType)
            {
                case E_BehaviorType.RootNode:
                    //win绑定根节点
                    // Debug.Log( EditorAssetsLoader.Instance.FindWithID<GameObject>(int.Parse(node.Parameter[0])));
                    win.RootNode = node as RootNode_VisualBehaviorTreeNode;
                    //跟物体绑定可以直接绑data的
                    if (tempData.BehaviorTreePrefab != null)
                        RootNode_VisualBehaviorTreeNode.instance.BehaviorObj = tempData.BehaviorTreePrefab;
                    //设置儿子
                    win.RootNode.ChildID = info.childsID[0];
                    break;
                case E_BehaviorType.ActionTreeNode:
                case E_BehaviorType.ConditionNode:
                    //加载动作节点和条件节点
                    m_LoadBNode(node, info);
                    break;
                case E_BehaviorType.ReverseDecoratorNode:
                case E_BehaviorType.DelayDecoratorNode:
                case E_BehaviorType.RepeatDecoratorNode:
                    //设置孩子
                    (node as DecoratorNode_VisualBehaviorTreeNode).ChildID = info.childsID[0];
                    break;

                case E_BehaviorType.SelectTreeNode:
                case E_BehaviorType.SequeneTreeNode:
                case E_BehaviorType.ParallelTreeNode:
                    //设置孩子们
                    (node as ControlNode_VisualBehaviorTreeNode).ChildsId = info.childsID;
                    break;
            }
            //绑定好爸爸
            m_NodeFindSelfFather(node, info);
        }
        //排序一波
        win.m_ClickAutoArrange();
    }
    /// <summary>
    ///加载条件节点，动作节点专用
    /// </summary>
    public void m_LoadBNode(VisualBehaviorTreeNode node, BTNodeInfo info)
    {
        BehaviorNode_VisualBehaviorTreeNode bNode = node as BehaviorNode_VisualBehaviorTreeNode;
        bNode.ListenNum = info.Parameters.Length;

        //设置每个选项
        for (int i = 0; i < bNode.ListenNum; i++)
        {
            string[] TempParameter = info.Parameters[i].Split('|');
            string componentName = TempParameter[0];
            string methodName = TempParameter[1];
            //截取处理得到所选的
            int Select1 = int.Parse(componentName.Substring(componentName.IndexOf('&') + 1, componentName.Length - componentName.IndexOf('&') - 1));
            int Select2 = int.Parse(methodName.Substring(methodName.IndexOf('&') + 1, methodName.Length - methodName.IndexOf('&') - 1));
            bNode.selectedIndex[i].Select1 = Select1;
            bNode.selectedIndex[i].Select2 = Select2;
        }
    }
}

public class BTImportSetting : AssetPostprocessor
{
    private void OnPostprocessAssetbundleNameChanged(string assetPath, string previousAssetBundleName, string newAssetBundleName)
    {
        if (assetPath.EndsWith(".asset"))
        {
            string assetName = Path.GetFileNameWithoutExtension(assetPath);

            Debug.Log("Asset imported: " + assetName);
        }
    }
}