using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 随机化器
/// </summary>
/// <typeparam name="T"></typeparam>
public class Randomizer<T>
{
    // 使用 System.Random 替代 UnityEngine.Random
    protected System.Random random;
    // 存储物品及其权重
    public List<T> items { private set; get; }
    public List<float> weights { private set; get; }
    // 记录所有权重的总和
    private float totalWeight = 0;
    /// <summary>
    /// 构造函数，初始化随机数生成器并设置种子
    /// </summary>
    /// <param name="seed">随机种子</param>
    public Randomizer(int seed)
    {
        random = new System.Random(seed);
        weights = new List<float>();
        items = new List<T>();
    }
    /// <summary>
    /// 添加一个物品，并指定其权重。
    /// </summary>
    /// <param name="item">物品对象</param>
    /// <param name="weight">该物品的权重，必须大于0</param>
    public void AddItem(T item, float weight = 0.5F)
    {
        if (weight <= 0)
        {
            Debug.LogWarning("权重必须大于0！");
            return;
        }

        if (items.Contains(item))
        {
            // 如果物品已存在，则更新权重
            Debug.LogError("重复添加随机物品:" + item);
            return;
        }
        else
        {
            // 否则，添加新物品
            items.Add(item);
            weights.Add(weight);
        }
        totalWeight += weight; // 更新总权重
    }

    /// <summary>
    /// 根据权重随机选择一个物品
    /// </summary>
    /// <returns>随机选中的物品</returns>
    public T GetWeightRandom()
    {
        if (weights.Count == 0)
        {
            throw new InvalidOperationException("没有可选项！");
        }

        // 使用 System.Random 生成随机值
        float randomValue = (float)random.NextDouble() * totalWeight;
        float cumulativeSum = 0;
        Debug.Log(randomValue);
        // 遍历字典中的物品，并根据累积权重判断选中哪一个
        for (int i = 0; i < items.Count; i++)
        {
            cumulativeSum += weights[i]; // 累加当前物品的权重
            if (randomValue < cumulativeSum)
            {
                return items[i]; // 选中该物品
            }
        }
        return default; // 理论上不会到这里，除非计算错误
    }
    public T GetEqualRandom()
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("没有可选项！");
        }
        // 使用 System.Random 生成随机索引
        int randomIndex = random.Next(0, items.Count);
        return items[randomIndex];
    }
    /// <summary>
    /// 严格按比例生成指定数量的结果（顺序随机）
    /// </summary>
    /// <param name="count">生成次数</param>
    /// <returns>结果列表（比例严格匹配，顺序随机）</returns>
    public List<T> GetProportionRandom(int count)
    {
        if (items.Count == 0 || weights.Count == 0)
        {
            throw new InvalidOperationException("没有可选项！");
        }

        // 1. 计算每个item应该出现的精确次数
        Dictionary<T, int> targetCounts = new Dictionary<T, int>();
        float totalWeight = weights.Sum();

        // 先分配整数部分
        for (int i = 0; i < items.Count; i++)
        {
            float exactCount = (weights[i] / totalWeight) * count;
            targetCounts[items[i]] = Mathf.FloorToInt(exactCount);
        }

        // 2. 处理剩余次数（由于浮点数计算可能有误差）
        int distributedCount = targetCounts.Values.Sum();
        int remaining = count - distributedCount;

        if (remaining > 0)
        {
            // 按小数部分从大到小排序，优先分配剩余次数
            var sortedItems = items
                .Select((item, index) => new
                {
                    Item = item,
                    Fraction = (weights[index] / totalWeight) * count - targetCounts[item]
                })
                .OrderByDescending(x => x.Fraction)
                .Take(remaining)
                .ToList();

            foreach (var item in sortedItems)
            {
                targetCounts[item.Item]++;
            }
        }

        // 3. 生成结果列表
        List<T> results = new List<T>();
        foreach (var pair in targetCounts)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                results.Add(pair.Key);
            }
        }

        // 4. 随机打乱顺序
        Shuffle(results);

        return results;
    }
    /// <summary>
    /// 抽取不放回方法 - 从items中随机抽取指定数量的元素，不会重复抽取
    /// </summary>
    /// <param name="drawCount">要抽取的数量</param>
    /// <returns>抽取的结果列表</returns>
    public List<T> GetWithoutReplacementRandom(int drawCount)
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("没有可选项！");
        }

        // 如果要抽取的数量超过总数量，返回全部元素的随机排序
        if (drawCount >= items.Count)
        {
            List<T> allItems = new List<T>(items);
            Shuffle(allItems);
            return allItems;
        }

        // 创建一个临时列表用于抽取
        List<T> tempList = new List<T>(items);
        List<T> result = new List<T>(drawCount);

        for (int i = 0; i < drawCount; i++)
        {
            // 随机选择一个索引
            int randomIndex = random.Next(0, tempList.Count);
            // 添加到结果
            result.Add(tempList[randomIndex]);
            // 从临时列表中移除
            tempList.RemoveAt(randomIndex);
        }

        return result;
    }
    // Fisher-Yates洗牌算法
    private void Shuffle(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public void Additems(List<T> items, List<int> weights = null)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (weights != null)
                AddItem(items[i], weights[i]);
            else
                AddItem(items[i], 0.5f);
        }
    }
    /// <summary>
    /// 移除随机物品
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(T item)
    {
        int index = items.IndexOf(item);
        items.Remove(item);
        weights.RemoveAt(index);
    }
    public void ResetSeed(int seed)
    {
        random = new System.Random(seed);
    }
    /// <summary>
    /// 清空所有物品
    /// </summary>
    public void ClearItems()
    {
        items.Clear();
        weights.Clear();
    }
}