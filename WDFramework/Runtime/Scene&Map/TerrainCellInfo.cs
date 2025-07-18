using System;
using UnityEngine;
[Serializable]
public class TerrainCellInfo
{
    //���ó�ֵ��
    public TerrainCellInfo()
    {
        CellType = E_TerrainCellType.Empty;
        EnemySpawnInfo = E_MonsterCellType.None;
    }
    public E_TerrainCellType CellType = E_TerrainCellType.Empty;
    public E_MonsterCellType EnemySpawnInfo = E_MonsterCellType.None;

    /// <summary>
    /// ͨ�����к�����ö��
    /// </summary>
    /// <param name="index">ö��ֵ�����к�</param>
    public void SetTerrainCellInfo<T>(int index) where T : Enum
    {
        // ��ȡ����ö��ֵ
        T[] values = (T[])Enum.GetValues(typeof(T));

        // ͨ�����к�����ö��
        if (index < 0 || index >= values.Length)
        {
            Debug.LogWarning("���к���Ч");
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
            Debug.LogWarning("��֧�ֵ�ö������");
        }
    }

    /// <summary>
    /// �ؿ���Ϣ
    /// </summary>
    public enum E_TerrainCellType
    {
        Empty,
        Tree,
        Plant,
    }
    [Flags]
    /// <summary>
    /// ���˳�������ռ��Ϣ ���ȼ����֣��ɶ�ѡ
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



