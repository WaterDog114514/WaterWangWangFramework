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
        //���ƻ�������
        data.CreateNumber = EditorGUILayout.IntField("���ü�X���ķ���飺", data.CreateNumber);
        data.newFileName = EditorGUILayout.TextField("�¸�������", data.newFileName);
        //����·��
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.TextArea(string.IsNullOrEmpty(data.newCopyDirectoryPath) ? "δѡ��..." : data.newCopyDirectoryPath);
        if (GUILayout.Button("ѡ��·��"))
        {
            data.newCopyDirectoryPath = EditorUtility.SaveFolderPanel("ѡ�񸱱��ļ���", data.LastSaveDirectoryPath, "");
            if (!string.IsNullOrEmpty(data.newCopyDirectoryPath))
                data.LastSaveDirectoryPath = data.newCopyDirectoryPath;
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("��ȡ�����ļ�������..."))
            (window as win_TerrainCreator).ReadTerrainData();
        //�ɹ���ȡ��ʹ��
        if (data.newDatasPath != null)
        {
            //Ԥ������ʾ
            EditorGUILayout.ObjectField("���õ���Ԥ����", data.prefab, typeof(GameObject), false);
            GUILayout.Label("�������ɵ�������Ϊ��" + data.newDatasPath.Length);
            if (GUILayout.Button("���ɵ���"))
                (window as win_TerrainCreator).CreateTerrainArea();
        }


    }
    public override void OnCreated()
    {
    }


}
