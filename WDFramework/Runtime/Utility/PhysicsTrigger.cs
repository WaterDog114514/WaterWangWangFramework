using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// һЩţ���жϴ��룬�˺��ж� һЩ����ֹ���״�жϼ�����
/// </summary>
public static class PhysicsTrigger
{
    //�ӿ��ж�
    //��ĳ���㵽ĳ����Ļ������򣬰��зַ����ֳ�n�����֣�ÿ�������뵽�����ڼ����֣�Ȼ��ȫ����¼������

    //�����ۼ��ͣ���һ�������洢���м�����飬����ִ�У�����Ҫ�ܸ����ܱ༭���Խ���

    //��Χ�������
    //�������ӳ�ִ��

    //���ӻ���⿪��������ÿ����ⶼ�ܿ���ͨ�����ӻ�����

    /// <summary>
    /// �ж����������Ƿ�����Ļ��
    /// </summary>
    /// <param name="WorldPos">��������</param>
    /// <param name="camera">�����жϵ������Ĭ���������</param>
    /// <returns></returns>
    public static bool IsWorldPosInScreen(Vector3 WorldPos, Camera camera = null)
    {
        if (camera == null) camera = Camera.main;
        Vector3 screenPos = camera.WorldToScreenPoint(WorldPos);
        if (screenPos.x >= 0 && screenPos.x <= Screen.width
            && screenPos.y >= 0 && screenPos.y <= Screen.height)
            return true;
        return false;
    }

    //��Ҫ�Ż�һ�£��������Ĳ����˲����ָ�����Ū
    public static bool IsInSectorRangeXZ(Vector3 pos, Vector3 forward, Vector3 targetPos, float radius, float angle)
    {
        pos.y = 0;
        forward.y = 0;
        targetPos.y = 0;
        //���� + �Ƕ�
        return Vector3.Distance(pos, targetPos) <= radius && Vector3.Angle(forward, targetPos - pos) <= angle / 2f;
    }

    public static void RayCast(Ray ray, float distance, UnityAction<RaycastHit> callback, params string[] layer)
    {
        int Layer = 0;
        for (int i = 0; i < layer.Length; i++)
        {
            Layer = Layer | 1 << LayerMask.NameToLayer(layer[i]);
        }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distance, Layer))
            callback?.Invoke(hit);
    }
    public static void RayCast(Vector3 pos, Vector3 direction, float Distance, UnityAction<RaycastHit> callback, params string[] layer)
    {
        Ray ray = new Ray(pos, direction);
        RayCast(ray, Distance, callback, layer);
    }
}
