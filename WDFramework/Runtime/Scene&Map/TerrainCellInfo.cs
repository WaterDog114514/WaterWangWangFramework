using System;
using UnityEngine;
[Serializable]
public class TerrainCellInfo
{
    //设置初值啊
    public TerrainCellInfo()
    {
        CellType = E_TerrainCellType.Empty;
        EnemySpawnInfo = E_MonsterCellType.None;
    }
    public E_TerrainCellType CellType = E_TerrainCellType.Empty;
    public E_MonsterCellType EnemySpawnInfo = E_MonsterCellType.None;

    /// <summary>
    /// 通过序列号设置枚举
    /// </summary>
    /// <param name="index">枚举值的序列号</param>
    public void SetTerrainCellInfo<T>(int index) where T : Enum
    {
        // 获取所有枚举值
        T[] values = (T[])Enum.GetValues(typeof(T));

        // 通过序列号设置枚举
        if (index < 0 || index >= values.Length)
        {
            Debug.LogWarning("序列号无效");
            return;
        }

        if (typeof(T) == typeof(E_TerrainCellType))
        {
            CellType = (E_TerrainCellType)(object)values[index];
        }
        else if (typeof(T) == typeof(E_MonsterCellType))
        {
            EnemySpawnInfo = (E_MonsterCellType)(object)values[index];
        }
        else
        {
            Debug.LogWarning("不支持的枚举类型");
        }
    }

    /// <summary>
    /// 地块信息
    /// </summary>
    public enum E_TerrainCellType
    {
        Empty,
        Tree,
        Plant,
    }
    [Flags]
    /// <summary>
    /// 敌人出生点所占信息 按等级划分，可多选
    /// </summary>
    public enum E_MonsterCellType
    {
        None = 0,
        NormalEnemy = 1 << 0,   // 1
        MiddleEnemy = 1 << 1,   // 2
        HighEnemy = 1 << 2,     // 4
        Neutral = 1 << 4      // 8
    }

}



