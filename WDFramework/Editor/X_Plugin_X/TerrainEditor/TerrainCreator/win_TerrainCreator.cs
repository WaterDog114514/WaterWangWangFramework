using System.IO;
using UnityEditor;
using UnityEngine;
using WDEditor;



public class win_TerrainCreator : BaseWindow<winDraw_TerrainCreator, winData_TerrainCreator>
{

    [MenuItem("只因终焉/生成地形")]
    protected static void OpenWindow()
    {
        EditorWindow.GetWindow<win_TerrainCreator>();
    }
    //上次生成的文件夹路径的记忆
    public override void OnOpenWindow()
    {
        base.OnOpenWindow();
        if (!string.IsNullOrEmpty(data.LastSaveDirectoryPath))
        {
            data.newCopyDirectoryPath = data.LastSaveDirectoryPath;
        }
    }


    /// <summary>
    /// 读取并复制地形文件
    /// </summary>
    public void ReadTerrainData()
    {
        var originPath = EditorUtility.OpenFilePanel("选择地形文件", Application.dataPath, "asset");
        // 检查文件是否存在
        if (!File.Exists(originPath))
        {
            UnityEngine.Debug.LogError("原始文件不存在: " + originPath);
            return;
        }
        // 获取原始文件所在目录和文件名
        var directory = Path.GetDirectoryName(originPath);
        //根据是否填了自己想改名字，那就使用这个想改名字
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originPath);
        var fileExtension = Path.GetExtension(originPath);
        if (!string.IsNullOrEmpty(data.newFileName))
        {
            fileNameWithoutExtension = data.newFileName;
        }

        //总生成数量
        var TotalNums = data.CreateNumber * data.CreateNumber;
        data.newDatasPath = new string[TotalNums];
        //读取地形
        var TerrainData = AssetDatabase.LoadAssetAtPath<TerrainData>(EditorPathHelper.GetRelativeAssetPath(originPath));
        //读取地形大小
        data.Size = TerrainData.size.x;
        //自动生成prefab，等生成地形成功后会删除
        data.prefab = new GameObject("待生成地形（别删）");
        var terrainComponent = data.prefab.AddComponent<Terrain>();
        var terrainColliderComponent = data.prefab.AddComponent<TerrainCollider>();
        terrainComponent.terrainData = TerrainData;
        terrainComponent.allowAutoConnect = true;
        terrainColliderComponent.terrainData = TerrainData;
        //加载所需内置material
        var assets = AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
        foreach ( var asset in assets)
        {
            if(asset.name == "Default-Terrain-Standard")
            {
                terrainComponent.materialTemplate = (Material)asset;
            }
        }
        // 遍历并创建指定数量的副本
        for (int i = 1; i <= TotalNums; i++)
        {
            // 生成新的文件名
            var newFileName = $"{fileNameWithoutExtension}_{i}{fileExtension}";
            var newFilePath = Path.Combine(directory, newFileName);
            if (!string.IsNullOrEmpty(data.newCopyDirectoryPath))
                newFilePath = Path.Combine(data.newCopyDirectoryPath, newFileName);
            //复制新地址
            var dataPath = EditorPathHelper.GetRelativeAssetPath(newFilePath);
            // Debug.Log(dataPath);
            data.newDatasPath[i - 1] = dataPath;
            // 复制文件到新路径
            File.Copy(originPath, newFilePath, overwrite: true);
        }
        //刷新
        AssetDatabase.Refresh();
    }
    //在场景中生成地块
    public void CreateTerrainArea()
    {
        data.terrainDatas = new TerrainData[data.newDatasPath.Length];
        //地形编号
        int TerrainIndex = 0;
        //根据大小来创建
        for (int i = 0; i < data.CreateNumber; i++)
        {
            for (int j = 0; j < data.CreateNumber; j++)
            {
                var terrainName = Path.GetFileNameWithoutExtension(data.newDatasPath[TerrainIndex]);
                var terrainData = AssetDatabase.LoadAssetAtPath<TerrainData>(data.newDatasPath[TerrainIndex]);
                //让名称对应，刚复制出来是不对应的啊
                terrainData.name = terrainName;
                //创建地形
                GameObject terrain = GameObject.Instantiate(data.prefab);
                terrain.name = "terrain_" + terrainName;
                //设置坐标
                terrain.transform.position = new Vector3(i * data.Size, 0, j * data.Size);
                //添加必要组件
                var terrainComponent = terrain.GetComponent<Terrain>();
                terrainComponent.terrainData = terrainData;
                //设置长款
                terrainData.size.Set(data.Size, 20, data.Size);
                //设置Collider
                var terrainCollider = terrain.GetComponent<TerrainCollider>();
                terrainCollider.terrainData = terrainData;
                //jiajia
                TerrainIndex++;

            }
        }
        //删除地形临时的Prefab了
        GameObject.DestroyImmediate(data.prefab);
    }
    public override void OnCloseWindow()
    {
        if (data.prefab != null)
            GameObject.DestroyImmediate(data.prefab);

    }

}
