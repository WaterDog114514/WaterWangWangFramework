using UnityEditor;
using UnityEngine;
using WDEditor;



public class winDraw_TerrainCreator : BaseWindowDraw<winData_TerrainCreator>
{
    public winDraw_TerrainCreator(EditorWindow window, winData_TerrainCreator data) : base(window, data)
    {
    }
    public override void Draw()
    {
        //绘制基本窗口
        data.CreateNumber = EditorGUILayout.IntField("设置几X几的方格块：", data.CreateNumber);
        data.newFileName = EditorGUILayout.TextField("新复制名：", data.newFileName);
        //复制路径
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea(string.IsNullOrEmpty(data.newCopyDirectoryPath) ? "未选择..." : data.newCopyDirectoryPath);
        if (GUILayout.Button("选择路径"))
        {
            data.newCopyDirectoryPath = EditorUtility.SaveFolderPanel("选择副本文件夹", data.LastSaveDirectoryPath, "");
            if (!string.IsNullOrEmpty(data.newCopyDirectoryPath))
                data.LastSaveDirectoryPath = data.newCopyDirectoryPath;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("读取地形文件并复制..."))
            (window as win_TerrainCreator).ReadTerrainData();
        //成功读取后使用
        if (data.newDatasPath != null)
        {
            //预制体显示
            EditorGUILayout.ObjectField("设置地形预设体", data.prefab, typeof(GameObject), false);
            GUILayout.Label("所需生成地形数量为：" + data.newDatasPath.Length);
            if (GUILayout.Button("生成地形"))
                (window as win_TerrainCreator).CreateTerrainArea();
        }


    }
    public override void OnCreated()
    {
    }


}
