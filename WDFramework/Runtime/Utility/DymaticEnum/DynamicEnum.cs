using System.Collections.Generic;

using System;

/// <summary>
/// ��̬ö�ٻ��� - ֧��������չ���ַ���ö��
/// </summary>
/// <typeparam name="T">ʵ��ö������</typeparam>
/// 

//enumValue�Ļ�ȡ
/*
   1. ��ȡ����ֵ������Ϊnull��
    var eventValue = GameEvent.Get("PlayerDeath");
   2. ����ö����ʱԤ����ֵ
    public class GameEvent : DynamicEnum<GameEvent> {
    public static readonly GameEvent PlayerDeath = new("PlayerDeath");
    public static readonly GameEvent LevelComplete = new("LevelComplete");
}

 */
public abstract class DynamicEnum<T> where T : DynamicEnum<T>
{
    private static readonly Dictionary<string, T> _values = new(StringComparer.OrdinalIgnoreCase);

    public string Name { get;private set; }

    // �������캯��
    protected DynamicEnum(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("ö�����Ʋ���Ϊ��");

        Name = name;

        // �Զ�ע��
        if (!_values.ContainsKey(name))
            _values[name] = (T)this;
    }

    // ��̬���֧��
    public static T Get(string name) => _values.TryGetValue(name, out var value) ? value : null;

    // ��̬�����ֵ
    public static T Add(string name)
    {
        if (_values.ContainsKey(name))
            throw new ArgumentException($"'{name}' �Ѵ���");

        // ͨ�����䴴��ʵ��
        var instance = Activator.CreateInstance(typeof(T), name) as T;
        return instance;
    }

    // ��ȡ����ֵ
    public static IEnumerable<T> GetAll() => _values.Values;

    public override string ToString() => Name;
}