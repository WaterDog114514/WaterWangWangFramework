using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 一些牛逼判断代码，伤害判断 一些奇奇怪怪形状判断检测代码
/// </summary>
public static class PhysicsTrigger
{
    //挥砍判断
    //从某个点到某个点的弧形区域，按切分法，分成n个部分，每过多少秒到触发第几部分，然后全部记录下来？

    //检测的累加型，用一个变量存储所有检测数组，到期执行，后面要能跟技能编辑器对接上

    //范围触发检测
    //需求：能延迟执行

    //可视化检测开发。做到每个检测都能可以通过可视化看到

    /// <summary>
    /// 判断世界坐标是否在屏幕内
    /// </summary>
    /// <param name="WorldPos">世界坐标</param>
    /// <param name="camera">用于判断的相机，默认是主相机</param>
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

    //需要优化一下，后期用四叉树八叉树分割再来弄
    public static bool IsInSectorRangeXZ(Vector3 pos, Vector3 forward, Vector3 targetPos, float radius, float angle)
    {
        pos.y = 0;
        forward.y = 0;
        targetPos.y = 0;
        //距离 + 角度
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
