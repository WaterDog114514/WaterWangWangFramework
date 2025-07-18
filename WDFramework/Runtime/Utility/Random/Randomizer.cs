using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �������
/// </summary>
/// <typeparam name="T"></typeparam>
public class Randomizer<T>
{
    // ʹ�� System.Random ��� UnityEngine.Random
    protected System.Random random;
    // �洢��Ʒ����Ȩ��
    public List<T> items { private set; get; }
    public List<float> weights { private set; get; }
    // ��¼����Ȩ�ص��ܺ�
    private float totalWeight = 0;
    /// <summary>
    /// ���캯������ʼ�����������������������
    /// </summary>
    /// <param name="seed">�������</param>
    public Randomizer(int seed)
    {
        random = new System.Random(seed);
        weights = new List<float>();
        items = new List<T>();
    }
    /// <summary>
    /// ���һ����Ʒ����ָ����Ȩ�ء�
    /// </summary>
    /// <param name="item">��Ʒ����</param>
    /// <param name="weight">����Ʒ��Ȩ�أ��������0</param>
    public void AddItem(T item, float weight = 0.5F)
    {
        if (weight <= 0)
        {
            Debug.LogWarning("Ȩ�ر������0��");
            return;
        }

        if (items.Contains(item))
        {
            // �����Ʒ�Ѵ��ڣ������Ȩ��
            Debug.LogError("�ظ���������Ʒ:" + item);
            return;
        }
        else
        {
            // �����������Ʒ
            items.Add(item);
            weights.Add(weight);
        }
        totalWeight += weight; // ������Ȩ��
    }

    /// <summary>
    /// ����Ȩ�����ѡ��һ����Ʒ
    /// </summary>
    /// <returns>���ѡ�е���Ʒ</returns>
    public T GetWeightRandom()
    {
        if (weights.Count == 0)
        {
            throw new InvalidOperationException("û�п�ѡ�");
        }

        // ʹ�� System.Random �������ֵ
        float randomValue = (float)random.NextDouble() * totalWeight;
        float cumulativeSum = 0;
        Debug.Log(randomValue);
        // �����ֵ��е���Ʒ���������ۻ�Ȩ���ж�ѡ����һ��
        for (int i = 0; i < items.Count; i++)
        {
            cumulativeSum += weights[i]; // �ۼӵ�ǰ��Ʒ��Ȩ��
            if (randomValue < cumulativeSum)
            {
                return items[i]; // ѡ�и���Ʒ
            }
        }
        return default; // �����ϲ��ᵽ������Ǽ������
    }
    public T GetEqualRandom()
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("û�п�ѡ�");
        }
        // ʹ�� System.Random �����������
        int randomIndex = random.Next(0, items.Count);
        return items[randomIndex];
    }
    /// <summary>
    /// �ϸ񰴱�������ָ�������Ľ����˳�������
    /// </summary>
    /// <param name="count">���ɴ���</param>
    /// <returns>����б������ϸ�ƥ�䣬˳�������</returns>
    public List<T> GetProportionRandom(int count)
    {
        if (items.Count == 0 || weights.Count == 0)
        {
            throw new InvalidOperationException("û�п�ѡ�");
        }

        // 1. ����ÿ��itemӦ�ó��ֵľ�ȷ����
        Dictionary<T, int> targetCounts = new Dictionary<T, int>();
        float totalWeight = weights.Sum();

        // �ȷ�����������
        for (int i = 0; i < items.Count; i++)
        {
            float exactCount = (weights[i] / totalWeight) * count;
            targetCounts[items[i]] = Mathf.FloorToInt(exactCount);
        }

        // 2. ����ʣ����������ڸ����������������
        int distributedCount = targetCounts.Values.Sum();
        int remaining = count - distributedCount;

        if (remaining > 0)
        {
            // ��С�����ִӴ�С�������ȷ���ʣ�����
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

        // 3. ���ɽ���б�
        List<T> results = new List<T>();
        foreach (var pair in targetCounts)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                results.Add(pair.Key);
            }
        }

        // 4. �������˳��
        Shuffle(results);

        return results;
    }
    /// <summary>
    /// ��ȡ���Żط��� - ��items�������ȡָ��������Ԫ�أ������ظ���ȡ
    /// </summary>
    /// <param name="drawCount">Ҫ��ȡ������</param>
    /// <returns>��ȡ�Ľ���б�</returns>
    public List<T> GetWithoutReplacementRandom(int drawCount)
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("û�п�ѡ�");
        }

        // ���Ҫ��ȡ����������������������ȫ��Ԫ�ص��������
        if (drawCount >= items.Count)
        {
            List<T> allItems = new List<T>(items);
            Shuffle(allItems);
            return allItems;
        }

        // ����һ����ʱ�б����ڳ�ȡ
        List<T> tempList = new List<T>(items);
        List<T> result = new List<T>(drawCount);

        for (int i = 0; i < drawCount; i++)
        {
            // ���ѡ��һ������
            int randomIndex = random.Next(0, tempList.Count);
            // ��ӵ����
            result.Add(tempList[randomIndex]);
            // ����ʱ�б����Ƴ�
            tempList.RemoveAt(randomIndex);
        }

        return result;
    }
    // Fisher-Yatesϴ���㷨
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
    /// �Ƴ������Ʒ
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
    /// ���������Ʒ
    /// </summary>
    public void ClearItems()
    {
        items.Clear();
        weights.Clear();
    }
}