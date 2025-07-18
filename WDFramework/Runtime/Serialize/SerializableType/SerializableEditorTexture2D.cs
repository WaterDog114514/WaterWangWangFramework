#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 编辑器专用2D图片。只能在编辑器开发序列化用
/// </summary>
[Serializable]
public class SerializableEditorTexture2D
{
    [NonSerialized]
    private Texture2D _texture; // 非序列化的实际图片缓存
    [SerializeField]
    private string _guid; // 记录图片的GUID，便于序列化
    /// <summary>
    /// 获取或设置Texture2D
    /// </summary>
    public Texture2D texture
    {
        get
        {
            // 如果未加载，且有GUID，尝试加载图片
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
                // 将图片的路径转换为GUID并存储
                string assetPath = AssetDatabase.GetAssetPath(_texture);
                _guid = AssetDatabase.AssetPathToGUID(assetPath);
            }
            else
            {
                // 清空GUID
                _guid = null;
            }
        }
    }

}
#endif