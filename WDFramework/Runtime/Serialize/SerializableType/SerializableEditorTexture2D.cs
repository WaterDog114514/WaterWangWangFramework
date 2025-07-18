#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
/// <summary>
/// �༭��ר��2DͼƬ��ֻ���ڱ༭���������л���
/// </summary>
[Serializable]
public class SerializableEditorTexture2D
{
    [NonSerialized]
    private Texture2D _texture; // �����л���ʵ��ͼƬ����
    [SerializeField]
    private string _guid; // ��¼ͼƬ��GUID���������л�
    /// <summary>
    /// ��ȡ������Texture2D
    /// </summary>
    public Texture2D texture
    {
        get
        {
            // ���δ���أ�����GUID�����Լ���ͼƬ
            if (_texture == null && !string.IsNullOrEmpty(_guid))
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(_guid);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                }
            }
            return _texture;
        }
        set
        {
            _texture = value;

            if (_texture != null)
            {
                // ��ͼƬ��·��ת��ΪGUID���洢
                string assetPath = AssetDatabase.GetAssetPath(_texture);
                _guid = AssetDatabase.AssetPathToGUID(assetPath);
            }
            else
            {
                // ���GUID
                _guid = null;
            }
        }
    }

}
#endif