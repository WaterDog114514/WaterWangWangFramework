using UnityEngine;

/// <summary>
/// �Ĳ�������
/// </summary>
public class QuadObject : MonoBehaviour
{
    /// <summary>
    /// ���õ�ʵ�ʵ���Ϸ����
    /// </summary>
    public GameObject GameObj;

    /// <summary>
    /// ��ʾһ���������Ĳ����е����꣬�Լ���ײ��С
    /// </summary>
    public SerializableRect ObjRect;

    /// <summary>
    /// �Ƿ��Ƕ�̬���壬��̬������ҪƵ������
    /// </summary>
    public bool IsDynamic;
    public QuadObject(GameObject gameObject, Rect bounds, bool isDynamic)
    {
        GameObj = gameObject;
        ObjRect.rect = bounds;
        IsDynamic = isDynamic;
    }
}

